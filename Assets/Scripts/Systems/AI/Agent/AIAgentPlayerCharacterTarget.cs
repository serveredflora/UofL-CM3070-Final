using UnityEngine;

public class AIAgentPlayerCharacterTarget : IAIAgentTarget
{
    public bool IsValid => PlayerCharacter != null;
    public Vector3 Position => PlayerCharacter.CameraTarget.transform.position;
    public Collider Collider => PlayerCharacterCollider.CapsuleCollider;

    private PlayerCharacter PlayerCharacter;
    private PlayerCharacterCollider PlayerCharacterCollider;

    public AIAgentPlayerCharacterTarget(Player player)
    {
        PlayerCharacter = player.Character;
        PlayerCharacterCollider = player.CharacterCollider;
    }
}
