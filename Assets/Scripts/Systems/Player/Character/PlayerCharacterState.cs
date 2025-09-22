using UnityEngine;

public struct PlayerCharacterState
{
    public bool Grounded;
    public PlayerCharacterStanceState StanceState;
    public Vector3 Velocity;
    public Vector3 Acceleration;
}
