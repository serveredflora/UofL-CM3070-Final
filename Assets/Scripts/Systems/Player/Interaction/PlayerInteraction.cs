using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float MaxFocusAngle = 30.0f;
    [SerializeField]
    private float MaxFocusDistance = 5.0f;

    [Header("References")]
    [SerializeField]
    private Collider BroadDetectionCollider;
    [SerializeField]
    private ColliderTriggerEvents BroadDetectionColliderTriggerEvents;

    public event Action<IPlayerInteractable, IPlayerInteractableAction> OnFocusInteractable;
    public event Action<IPlayerInteractable, IPlayerInteractableAction> OnUnfocusInteractable;

    public event Action<IPlayerInteractable, IPlayerInteractableAction> OnCycleToAction;
    public event Action<IPlayerInteractable, IPlayerInteractableAction, PlayerInteractableActionPerformResult> OnExecuteAction;

    public IPlayerInteractable FocusedInteractable => _FocusedInteractable?.Interactable;
    private SpatialInteractable? _FocusedInteractable;

    public IEnumerable<IPlayerInteractable> AvailableInteractables => _AvailableInteractables.Select(v => v.Interactable);
    private List<SpatialInteractable> _AvailableInteractables = new();

    public IPlayerInteractableAction CycledAction { get; private set; }
    private int _CycledActionIndex = -1;

    private List<SpatialInteractable> _InteractablesToRemove = new();

    private Player Player;
    private RaycastHit[] RaycastResults = new RaycastHit[4];

    private void Start()
    {
        BroadDetectionColliderTriggerEvents.OnColliderTriggerEnter += ColliderTriggerEntered;
        BroadDetectionColliderTriggerEvents.OnColliderTriggerExit += ColliderTriggerExited;
    }

    private void OnDestroy()
    {
        BroadDetectionColliderTriggerEvents.OnColliderTriggerEnter -= ColliderTriggerEntered;
        BroadDetectionColliderTriggerEvents.OnColliderTriggerExit -= ColliderTriggerExited;
    }

    public void Initialize(Player player, Transform cameraTransform)
    {
        Player = player;
        UpdateTransform(cameraTransform);
    }

    public void UpdateInput(PlayerInteractionInput input)
    {
        if (FocusedInteractable == null)
        {
            return;
        }

        if (input.PerformAction)
        {
            Perform(FocusedInteractable, CycledAction);
        }
        else if (input.ActionCycleOffset != 0)
        {
            OffsetCycledActionIndex(input.ActionCycleOffset);
        }
    }

    private void Perform(IPlayerInteractable focusedOn, IPlayerInteractableAction cycledAction)
    {
        var result = cycledAction.Perform(Player);
        OnExecuteAction?.Invoke(focusedOn, cycledAction, result);
    }

    public void UpdateTransform(Transform cameraTransform)
    {
        BroadDetectionCollider.transform.position = cameraTransform.position;
        BroadDetectionCollider.transform.rotation = cameraTransform.rotation;

        EvaluateFocusedInteractable(cameraTransform);
    }

    private void EvaluateFocusedInteractable(Transform cameraTransform)
    {
        SpatialInteractable? newFocusedInteractable = null;
        float bestAngle = float.MaxValue;
        foreach (var interactable in _AvailableInteractables)
        {
            if (interactable.Collider == null)
            {
                // TODO: remove this interactable
                _InteractablesToRemove.Add(interactable);
                continue;
            }

            // TODO: maybe add disabled/timeout/cooldown state?
            if (interactable.Interactable.Actions.Count == 0)
            {
                continue;
            }

            Vector3 point = interactable.Collider.ClosestPoint(cameraTransform.position);

            Vector3 diffVec = point - cameraTransform.position;
            float distance = diffVec.magnitude;
            if (distance > MaxFocusDistance)
            {
                continue;
            }

            Vector3 dirVec = diffVec.normalized;
            float angle = Vector3.Angle(cameraTransform.forward, dirVec);
            if (angle > MaxFocusAngle)
            {
                continue;
            }

            if (angle >= bestAngle)
            {
                continue;
            }

            Ray ray = new Ray(cameraTransform.position, dirVec);
            int hitCount = PhysicsUtils.SortedRaycast(ray, RaycastResults, maxDistance: MaxFocusDistance, triggerOpt: QueryTriggerInteraction.Collide);

            bool isInteractableUnobstructed = true;
            for (int hitIndex = 0; hitIndex < hitCount; ++hitIndex)
            {
                RaycastHit hit = RaycastResults[hitIndex];
                if (hit.collider == null)
                {
                    continue;
                }

                if (hit.collider == interactable.Collider)
                {
                    break;
                }

                if (hit.collider.isTrigger)
                {
                    continue;
                }

                if (hit.collider.tag == "Player")
                {
                    continue;
                }

                isInteractableUnobstructed = false;
                break;
            }

            if (!isInteractableUnobstructed)
            {
                continue;
            }

            newFocusedInteractable = interactable;
            bestAngle = angle;
        }

        ProcessInteractablesToRemove();

        SetFocusedInteractable(newFocusedInteractable);
    }

    private void ProcessInteractablesToRemove()
    {
        foreach (var interactable in _InteractablesToRemove)
        {
            _AvailableInteractables.Remove(interactable);
        }

        _InteractablesToRemove.Clear();
    }

    private void SetFocusedInteractable(SpatialInteractable? newFocusedInteractable)
    {
        if (!newFocusedInteractable.Equals(_FocusedInteractable))
        {
            if (_FocusedInteractable.HasValue)
            {
                OnUnfocusInteractable?.Invoke(_FocusedInteractable.Value.Interactable, CycledAction);
            }

            _FocusedInteractable = newFocusedInteractable;

            if (_FocusedInteractable.HasValue)
            {
                SetCycledActionIndex(0, false);
                OnFocusInteractable?.Invoke(_FocusedInteractable.Value.Interactable, CycledAction);
            }
        }
    }

    private void SetCycledActionIndex(int index, bool invokeEvent = true)
    {
        CycledAction = _FocusedInteractable.Value.Interactable.Actions[index];
        _CycledActionIndex = index;

        if (invokeEvent)
        {
            OnCycleToAction?.Invoke(FocusedInteractable, CycledAction);
        }
    }

    private void OffsetCycledActionIndex(int offset)
    {
        int count = _FocusedInteractable.Value.Interactable.Actions.Count;
        int newIndex = MathUtils.Mod(_CycledActionIndex + offset, count);

        SetCycledActionIndex(newIndex);
    }

    private void ColliderTriggerEntered(Collider collider)
    {
        if (!collider.gameObject.TryGetComponent<PlayerInteractableAccessor>(out var interactableAccessor))
        {
            return;
        }

        var spatialInteractable = new SpatialInteractable(collider, interactableAccessor.Interactable);
        _AvailableInteractables.Add(spatialInteractable);
    }

    private void ColliderTriggerExited(Collider collider)
    {
        if (!collider.gameObject.TryGetComponent<PlayerInteractableAccessor>(out var interactableAccessor))
        {
            return;
        }

        var spatialInteractable = new SpatialInteractable(collider, interactableAccessor.Interactable);
        _AvailableInteractables.Remove(spatialInteractable);
    }

    private readonly struct SpatialInteractable : IEquatable<SpatialInteractable>
    {
        public readonly Collider Collider;
        public readonly IPlayerInteractable Interactable;

        public SpatialInteractable(Collider collider, IPlayerInteractable interactable) : this()
        {
            Collider = collider;
            Interactable = interactable;
        }

        public override bool Equals(object obj)
        {
            return obj is SpatialInteractable other && Equals(other);
        }

        public bool Equals(SpatialInteractable other)
        {
            return Collider == other.Collider && Interactable == other.Interactable;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Collider.GetHashCode(), Interactable.GetHashCode());
        }
    }
}
