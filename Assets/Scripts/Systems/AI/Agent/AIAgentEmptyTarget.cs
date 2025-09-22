using UnityEngine;

public class AIAgentEmptyTarget : IAIAgentTarget
{
    public bool IsValid => false;
    public Vector3 Position => Vector3.zero;
    public Collider Collider => null;
}