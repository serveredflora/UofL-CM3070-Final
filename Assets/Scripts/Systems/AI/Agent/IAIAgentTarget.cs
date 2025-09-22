using UnityEngine;

public interface IAIAgentTarget
{
    bool IsValid { get; }
    Vector3 Position { get; }
    Collider Collider { get; }
}
