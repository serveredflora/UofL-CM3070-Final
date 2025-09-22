using UnityEngine;

public class AIAgentTransformColliderTarget : IAIAgentTarget
{
    public bool IsValid => Transform != null && Collider != null;
    public Vector3 Position => Transform.position;
    Collider IAIAgentTarget.Collider => Collider;

    public Transform Transform;
    public Collider Collider;
}
