using UnityEngine;

public interface IPhysicsTeleportable
{
    Vector3 Position { get; }

    void Teleport(Vector3 position, bool killVelocity = true);
}