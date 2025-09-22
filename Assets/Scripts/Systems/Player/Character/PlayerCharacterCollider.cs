using UnityEngine;

public class PlayerCharacterCollider : MonoBehaviour
{
    [Header("References")]
    public PlayerCharacter Character;
    [SerializeField]
    private Rigidbody Rigidbody;
    public CapsuleCollider CapsuleCollider;

    public void Initialize()
    {
        Character.OnTeleport += CharacterTeleport;
    }

    public void Cleanup()
    {
        Character.OnTeleport -= CharacterTeleport;
    }

    public void Copy()
    {
        // TODO: check if this allows the enemy melee attack to finally work
        Rigidbody.MovePosition(Character.transform.position);
        Rigidbody.MoveRotation(Character.transform.rotation);
        transform.localScale = Character.transform.localScale;

        CapsuleCollider.height = Character.Height;
        CapsuleCollider.center = new Vector3(0.0f, Character.Height * 0.5f, 0.0f);
    }

    private void CharacterTeleport()
    {
        Copy();
    }
}
