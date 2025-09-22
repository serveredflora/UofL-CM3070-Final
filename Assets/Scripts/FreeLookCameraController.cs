using UnityEngine;

public class FreeLookCameraController : MonoBehaviour
{
    [Header("Settings")]
    public float BaseMoveSpeed = 10.0f;
    public float FastSpeedMultiplier = 2.5f;
    public float SlowSpeedMultiplier = 0.4f;

    [Space(10)]
    public float MouseRotateSpeed = 0.02f;

    [Space(10)]
    public float PositionDampening = 8.0f;
    public float RotationDampening = 4.0f;

    private Vector3 GoalPosition;

    private float GoalYaw;
    private float GoalPitch;

    private float CurrentYaw;
    private float CurrentPitch;

    private bool IsFocused => Cursor.lockState == CursorLockMode.Locked;

    private void Start()
    {
        GoalPosition = transform.position;
        GoalPitch = CurrentPitch = 0.0f;
        GoalYaw = CurrentYaw = 0.0f;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Cursor.lockState = IsFocused ? CursorLockMode.None : CursorLockMode.Locked;
        }

        if (IsFocused)
        {
            ProcessInput();
        }

        UpdateTransform();
    }

    private void ProcessInput()
    {
        ProcessMovementInput();
        ProcessRotationInput();
    }

    private void ProcessMovementInput()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("UpDown"), Input.GetAxisRaw("Vertical"));

        float moveSpeed = BaseMoveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed *= FastSpeedMultiplier;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            moveSpeed *= SlowSpeedMultiplier;
        }

        moveInput *= moveSpeed * Time.deltaTime;

        GoalPosition += transform.right * moveInput.x;
        GoalPosition += transform.up * moveInput.y;
        GoalPosition += transform.forward * moveInput.z;
    }

    private void ProcessRotationInput()
    {
        Vector2 lookInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        lookInput *= MouseRotateSpeed;

        GoalYaw += lookInput.x;
        GoalPitch = Mathf.Clamp(GoalPitch - lookInput.y, -89.9f, 89.9f);
    }

    private void UpdateTransform()
    {
        float posDamp = MathUtils.ExpDamp(PositionDampening, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, GoalPosition, posDamp);

        float rotDamp = MathUtils.ExpDamp(RotationDampening, Time.deltaTime);
        CurrentYaw = Mathf.Lerp(CurrentYaw, GoalYaw, rotDamp);
        CurrentPitch = Mathf.Lerp(CurrentPitch, GoalPitch, rotDamp);

        transform.rotation = Quaternion.AngleAxis(CurrentYaw, Vector3.up);
        transform.rotation = Quaternion.AngleAxis(CurrentPitch, transform.right) * transform.rotation;
    }
}
