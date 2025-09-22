using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float RotateSensitivity = 0.1f;

    private Vector3 _EulerAngles;

    public void Initialize(Transform target)
    {
        transform.position = target.position;
        transform.eulerAngles = _EulerAngles = target.eulerAngles;
    }

    public void UpdateRotation(PlayerCameraInput input)
    {
        _EulerAngles += new Vector3(-input.Look.y, input.Look.x, 0.0f) * RotateSensitivity;

        const float maxValue = 89.9f;
        _EulerAngles.x = Mathf.Clamp(_EulerAngles.x, -maxValue, maxValue);

        transform.eulerAngles = _EulerAngles;
    }

    public void UpdatePosition(Transform target)
    {
        transform.position = target.position;
    }
}
