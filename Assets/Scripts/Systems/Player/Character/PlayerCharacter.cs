using System;
using System.Security.Cryptography;
using TriInspector;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPhysicsTeleportable
{
    public event Action OnTeleport;

    public Vector3 Position => transform.position;
    public Transform CameraTarget => _CameraTarget;
    public PlayerCharacterState State => _State;

    public bool IsActive { get; private set; } = true;
    public float Height => CharacterController.height;

    [Header("Settings")]
    [SerializeField]
    private float WalkSpeed = 10.0f;
    [SerializeField]
    private float CrouchSpeed = 6.5f;
    [SerializeField]
    private float WalkResponse = 25.0f;
    [SerializeField]
    private float CrouchResponse = 20.0f;

    [Space(10)]
    [SerializeField]
    private float AirSpeed = 15.0f;
    [SerializeField]
    private float AirAcceleration = 70.0f;

    [Space(10)]
    [SerializeField]
    private float JumpSpeed = 20.0f;
    [SerializeField]
    private float CoyoteTime = 0.2f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float JumpSustainGravity = 0.4f;
    [SerializeField]
    private float Gravity = -90.0f;

    [Space(10)]
    [SerializeField]
    private float SlideStartSpeed = 25.0f;
    [SerializeField]
    private float SlideEndSpeed = 15.0f;
    [SerializeField]
    private float SlideFriction = 0.8f;
    [SerializeField]
    private float SlideSteerAcceleration = 5.0f;
    [SerializeField]
    private float SlideGravity = -90.0f;

    [Space(10)]
    [SerializeField]
    private float StandHeight = 2f;
    [SerializeField]
    private float CrouchHeight = 1f;
    [SerializeField]
    private float CrouchHeightResponse = 15.0f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float StandCameraTargetHeight = 0.9f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float CrouchCameraTargetHeight = 0.9f;

    [Header("References")]
    [SerializeField]
    private CharacterController CharacterController;
    [SerializeField]
    private Transform Root;
    [SerializeField]
    private Transform _CameraTarget;

    private PlayerCharacterState _State;
    private PlayerCharacterState _PreviousState;
    private PlayerCharacterState _TempState;

    private Vector3 _InputMovement;
    private Quaternion _InputRotation;
    private bool _InputJump;
    private bool _InputJumpSustained;
    private bool _InputCrouch;
    private bool _InputCrouchInAir;

    private float _TimeSinceUngrounded;
    private float _TimeSinceJumpRequest;
    private bool _UngroundedDueToJump;

    private float _IntendedVelocitySpeed;

    private Collider[] _UncrouchOverlapResults;
    private RaycastHit[] _GroundCheckHits;

    public void Initialize()
    {
        _State.StanceState = PlayerCharacterStanceState.Standing;
        _PreviousState = _State;

        _UncrouchOverlapResults = new Collider[4];
        _GroundCheckHits = new RaycastHit[4];
    }

    public void UpdateInput(PlayerCharacterInput input)
    {
        if (!IsActive)
        {
            return;
        }

        _InputMovement = new Vector3(input.Move.x, 0.0f, input.Move.y);
        _InputMovement = Vector3.ClampMagnitude(_InputMovement, 1.0f);
        _InputMovement = ExtractOnlyYRotation(input.Rotation) * _InputMovement;

        _InputRotation = input.Rotation;

        bool wasRequestingJump = _InputJump;
        _InputJump |= input.Jump;
        if (_InputJump && !wasRequestingJump)
        {
            _TimeSinceJumpRequest = 0.0f;
        }

        _InputJumpSustained = input.JumpSustain;

        bool wasRequestingCrouch = _InputCrouch;
        _InputCrouch = input.Crouch ? !_InputCrouch : _InputCrouch;
        if (_InputCrouch && !wasRequestingCrouch)
        {
            _InputCrouchInAir = !_State.Grounded;
        }
        else if (!_InputCrouch && wasRequestingCrouch)
        {
            _InputCrouchInAir = false;
        }
    }

    public void PerformUpdate(float deltaTime)
    {
        if (!IsActive)
        {
            return;
        }

        PrePerformMovement(deltaTime);
        PerformRotation(deltaTime);
        PerformMovement(deltaTime);
        PostPerformMovement(deltaTime);

        float currentHeight = Height;
        float normalizedHeight = currentHeight / StandHeight;

        float cameraTargetHeight = currentHeight * (_State.StanceState is PlayerCharacterStanceState.Standing ? StandCameraTargetHeight : CrouchCameraTargetHeight);
        var rootTargetScale = new Vector3(1.0f, normalizedHeight, 1.0f);

        CameraTarget.localPosition = Vector3.Lerp(
            a: CameraTarget.localPosition,
            b: new Vector3(0.0f, cameraTargetHeight, 0.0f),
            t: 1.0f - Mathf.Exp(-CrouchHeightResponse * deltaTime)
        );
        Root.localScale = Vector3.Lerp(
            a: Root.localScale,
            b: rootTargetScale,
            t: 1.0f - Mathf.Exp(-CrouchHeightResponse * deltaTime)
        );
    }

    public void Teleport(Vector3 center, bool killVelocity = true)
    {
        transform.position = center;
        OnTeleport?.Invoke();

        if (killVelocity)
        {
            CharacterController.Move(Vector3.zero);
        }
    }

    public void SetIsActive(bool value)
    {
        IsActive = value;
    }

    private Quaternion ExtractOnlyYRotation(Quaternion source)
    {
        Quaternion rotation = source;
        var forward = Vector3.ProjectOnPlane(source * Vector3.forward, transform.up);
        if (forward == Vector3.zero)
        {
            forward = transform.forward;
        }
        rotation = Quaternion.LookRotation(forward, transform.up);

        return rotation;
    }

    private void PerformRotation(float deltaTime)
    {
        Quaternion currentRotation = transform.rotation;

        var forward = Vector3.ProjectOnPlane(_InputRotation * Vector3.forward, transform.up);

        if (forward != Vector3.zero)
        {
            currentRotation = Quaternion.LookRotation(forward, transform.up);
        }

        transform.rotation = currentRotation;
    }

    private void PrePerformMovement(float deltaTime)
    {
        _TempState = _State;
        if (_InputCrouch && _State.StanceState is PlayerCharacterStanceState.Standing)
        {
            _State.StanceState = PlayerCharacterStanceState.Crouching;
            SetCapsuleHeight(CrouchHeight);
        }
    }

    private void PostPerformMovement(float deltaTime)
    {
        if (!_InputCrouch && _State.StanceState is not PlayerCharacterStanceState.Standing)
        {
            SetCapsuleHeight(StandHeight);

            if (PerformCapsuleOverlapCheck(_UncrouchOverlapResults) > 0)
            {
                _InputCrouch = true;
                SetCapsuleHeight(CrouchHeight);
            }
            else
            {
                _State.StanceState = PlayerCharacterStanceState.Standing;
            }
        }
    }

    private void SetCapsuleHeight(float value)
    {
        CharacterController.height = value;
        CharacterController.center = new Vector3(0.0f, value * 0.5f, 0.0f);
    }

    private Vector3 GetDirectionTangentToSurface(Vector3 direction, Vector3 surfaceNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(surfaceNormal, directionRight).normalized;
    }

    private void PerformMovement(float deltaTime)
    {
        Vector3 initialVelocity = CharacterController.velocity;
        Vector3 currentVelocity = initialVelocity;

        CalculateGroundState(out bool foundAnyGround, out bool isStableOnGround, out Vector3 groundNormal, out Vector3 groundVelocity);
        // Debug.Log($"AnyGround: {foundAnyGround}, IsStableGround: {isStableOnGround}, GroundNormal: {groundNormal}");

        if (!isStableOnGround && _State.StanceState is PlayerCharacterStanceState.Sliding)
        {
            _State.StanceState = PlayerCharacterStanceState.Crouching;
        }

        _State.Acceleration = Vector3.zero;

        if (isStableOnGround)
        {
            _TimeSinceUngrounded = 0.0f;
            _UngroundedDueToJump = false;

            var groundedMovement = GetDirectionTangentToSurface(
                direction: _InputMovement,
                surfaceNormal: groundNormal
            );

            {
                bool moving = groundedMovement.sqrMagnitude > 0.0f;
                bool crouching = _State.StanceState is PlayerCharacterStanceState.Crouching;
                bool wasStanding = _PreviousState.StanceState is PlayerCharacterStanceState.Standing;
                bool wasInAir = !_PreviousState.Grounded;
                if (moving && crouching && (wasStanding || wasInAir))
                {
                    _State.StanceState = PlayerCharacterStanceState.Sliding;

                    if (wasInAir)
                    {
                        currentVelocity = Vector3.ProjectOnPlane(
                            vector: _PreviousState.Velocity,
                            planeNormal: groundNormal
                        );
                    }

                    float effectiveStartSlideSpeed = SlideStartSpeed;
                    if (!_PreviousState.Grounded && !_InputCrouchInAir)
                    {
                        effectiveStartSlideSpeed = 0.0f;
                        _InputCrouchInAir = false;
                    }

                    float slideSpeed = Mathf.Max(effectiveStartSlideSpeed, currentVelocity.magnitude);
                    currentVelocity = GetDirectionTangentToSurface(
                        direction: currentVelocity,
                        surfaceNormal: groundNormal
                    ) * slideSpeed;
                }
            }

            if (_State.StanceState is PlayerCharacterStanceState.Standing or PlayerCharacterStanceState.Crouching)
            {
                float speed = _State.StanceState switch
                {
                    PlayerCharacterStanceState.Standing => WalkSpeed,
                    PlayerCharacterStanceState.Crouching => CrouchSpeed,
                    _ => 0.0f,
                };
                float response = _State.StanceState switch
                {
                    PlayerCharacterStanceState.Standing => WalkResponse,
                    PlayerCharacterStanceState.Crouching => CrouchResponse,
                    _ => 0.0f,
                };

                var targetVelocity = groundedMovement * speed;

                var moveVelocity = Vector3.Lerp(
                    a: currentVelocity,
                    b: targetVelocity,
                    t: 1.0f - Mathf.Exp(-response * deltaTime)
                );

                _State.Acceleration = (moveVelocity - currentVelocity) / deltaTime;

                currentVelocity = moveVelocity;
            }
            else
            {
                currentVelocity -= currentVelocity * (SlideFriction * deltaTime);

                {
                    var force = Vector3.ProjectOnPlane(
                        vector: -transform.up,
                        planeNormal: groundNormal
                    ) * SlideGravity;

                    currentVelocity -= force * deltaTime;
                }

                {
                    var currentSpeed = currentVelocity.magnitude;
                    var targetVelocity = groundedMovement * currentSpeed;
                    var steerVelocity = currentVelocity;
                    var steerForce = (targetVelocity - currentVelocity) * SlideSteerAcceleration * deltaTime;

                    steerVelocity += steerForce;
                    steerVelocity = Vector3.ClampMagnitude(steerVelocity, currentSpeed);

                    _State.Acceleration = (steerVelocity - currentVelocity) / deltaTime;

                    currentVelocity = steerVelocity;
                }

                if (currentVelocity.magnitude < SlideEndSpeed)
                {
                    _State.StanceState = PlayerCharacterStanceState.Crouching;
                }
            }
        }
        else
        {
            _TimeSinceUngrounded += deltaTime;

            if (_InputMovement.sqrMagnitude > 0.0f)
            {
                var planarMovement = Vector3.ProjectOnPlane(
                    vector: _InputMovement,
                    planeNormal: transform.up
                );
                planarMovement.Normalize();
                planarMovement *= _InputMovement.magnitude;

                var currentPlanarVelocity = Vector3.ProjectOnPlane(
                    vector: currentVelocity,
                    planeNormal: transform.up
                );

                var movementForce = planarMovement * AirAcceleration * deltaTime;

                if (currentPlanarVelocity.magnitude < AirSpeed)
                {
                    var targetPlanarVelocity = currentPlanarVelocity + movementForce;
                    targetPlanarVelocity = Vector3.ClampMagnitude(targetPlanarVelocity, AirSpeed);

                    movementForce = targetPlanarVelocity - currentPlanarVelocity;
                }
                else if (Vector3.Dot(currentPlanarVelocity, movementForce) > 0.0f)
                {
                    var constrainedMovementForce = Vector3.ProjectOnPlane(
                        vector: movementForce,
                        planeNormal: currentPlanarVelocity.normalized
                    );

                    movementForce = constrainedMovementForce;
                }

                if (foundAnyGround)
                {
                    if (Vector3.Dot(movementForce, currentVelocity + movementForce) > 0.0f)
                    {
                        var obstructionNormal = Vector3.Cross(
                            transform.up,
                            Vector3.Cross(
                                transform.up,
                                groundNormal
                            )
                        ).normalized;

                        movementForce = Vector3.ProjectOnPlane(movementForce, obstructionNormal);
                    }
                }

                currentVelocity += movementForce;
            }

            float effectiveGravity = Gravity;
            float verticalSpeed = Vector3.Dot(currentVelocity, transform.up);
            if (_InputJumpSustained && verticalSpeed > 0.0f)
            {
                effectiveGravity *= JumpSustainGravity;
            }

            currentVelocity += transform.up * effectiveGravity * deltaTime;
        }

        currentVelocity += groundVelocity;

        if (_InputJump)
        {
            bool grounded = isStableOnGround;
            bool canCoyoteJump = _TimeSinceUngrounded < CoyoteTime && !_UngroundedDueToJump;

            if (grounded || canCoyoteJump)
            {
                _InputJump = false;
                _InputCrouch = false;
                _InputCrouchInAir = false;

                // Motor.ForceUnground(0.0f);
                _UngroundedDueToJump = true;

                float currentVerticalSpeed = Vector3.Dot(currentVelocity, transform.up);
                float targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, JumpSpeed);
                currentVelocity += transform.up * (targetVerticalSpeed - currentVerticalSpeed);
            }
            else
            {
                _TimeSinceJumpRequest += deltaTime;

                bool canJumpLater = _TimeSinceJumpRequest < CoyoteTime;
                _InputJump = canJumpLater;
            }
        }

        _State.Grounded = isStableOnGround;
        _State.Velocity = currentVelocity;
        _PreviousState = _TempState;

        _IntendedVelocitySpeed = currentVelocity.magnitude;

        CharacterController.Move(currentVelocity * deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, hit.moveDirection.y, hit.moveDirection.z);

        body.AddForceAtPosition(pushDir * _IntendedVelocitySpeed, hit.point, ForceMode.Impulse);
    }

    private void GetCapsuleLocalPoints(out Vector3 capsuleLocalBottom, out Vector3 capsuleLocalTop)
    {
        Vector3 halfCapsuleUp = transform.up * (Height * 0.5f - CharacterController.radius);
        capsuleLocalBottom = CharacterController.center - halfCapsuleUp;
        capsuleLocalTop = CharacterController.center + halfCapsuleUp;
    }

    private int PerformCapsuleOverlapCheck(Collider[] results)
    {
        Vector3 halfCapsuleUp = transform.up * (Height * 0.5f - CharacterController.radius);
        Vector3 capsuleLocalBottom = CharacterController.center - halfCapsuleUp;
        Vector3 capsuleLocalTop = CharacterController.center + halfCapsuleUp;

        int resultsCount = Physics.OverlapCapsuleNonAlloc(transform.position + capsuleLocalBottom, transform.position + capsuleLocalTop, CharacterController.radius, results, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        int selfIndex = -1;
        for (int i = 0; i < resultsCount; i++)
        {
            if (results[i] == CharacterController)
            {
                selfIndex = i;
                break;
            }
        }

        if (selfIndex != -1)
        {
            --resultsCount;
            results[selfIndex] = results[resultsCount];
        }

        return resultsCount;
    }

    private void CalculateGroundState(out bool foundAnyGround, out bool isStableOnGround, out Vector3 groundNormal, out Vector3 groundVelocity)
    {
        GetCapsuleLocalPoints(out Vector3 capsuleLocalBottom, out Vector3 capsuleLocalTop);

        float probingBackstepDistance = 0.1f;
        Vector3 probingDirection = -transform.up;

        Vector3 capsulePointA = transform.position + capsuleLocalBottom - (probingDirection * probingBackstepDistance);
        Vector3 capsulePointB = transform.position + capsuleLocalTop - (probingDirection * probingBackstepDistance);

        float probingDistance = 0.2f;

        float totalDistance = probingDistance + probingBackstepDistance;

        int groundCheckHitsCount = Physics.CapsuleCastNonAlloc(
            capsulePointA,
            capsulePointB,
            CharacterController.radius,
            probingDirection,
            _GroundCheckHits,
            totalDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore);

        RaycastHit closestGroundHit = new();
        bool foundValidHit = false;
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < groundCheckHitsCount; i++)
        {
            RaycastHit hit = _GroundCheckHits[i];
            float hitDistance = hit.distance;

            // Find the closest valid hit
            if (hitDistance > 0f)
            {
                if (hitDistance < closestDistance && hit.collider != CharacterController)
                {
                    closestGroundHit = hit;
                    closestGroundHit.distance -= probingBackstepDistance;
                    closestDistance = hitDistance;

                    foundValidHit = true;
                }
            }
        }

        if (!foundValidHit)
        {
            foundAnyGround = false;
            isStableOnGround = false;
            groundNormal = Vector3.up;
            groundVelocity = Vector3.zero;
            return;
        }

        float maxStableAngle = CharacterController.slopeLimit;

        foundAnyGround = true;
        groundNormal = closestGroundHit.normal;
        groundVelocity = closestGroundHit.rigidbody?.GetPointVelocity(closestGroundHit.point) ?? Vector3.zero;
        isStableOnGround = Vector3.Angle(transform.up, groundNormal) <= maxStableAngle;
    }
}
