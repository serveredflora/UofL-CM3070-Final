using TriInspector;
using UnityEngine;

public class WorldItemRepresentation : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    public Bounds Bounds;

    [Header("References")]
    [SerializeField]
    private PlayerInteractableAccessor _PlayerInteractableAccessor;

    public PlayerInteractableAccessor PlayerInteractableAccessor => _PlayerInteractableAccessor;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Bounds.center, Bounds.size);
    }
}