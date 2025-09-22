using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractionHUDGridCell : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PointerEvents PointerEvents;
    [SerializeField]
    private PlayerInteractionHUDGridCellTooltip TooltipPrefab;
    [SerializeField]
    private TMPro.TMP_Text QuantityText;

    public Item Item;

    private PlayerInteractionHUDGridCellTooltip TooltipInstance;

    private void Start()
    {
        // Disable quantity text as items aren't stacked (for now...)
        QuantityText.gameObject.SetActive(false);

        PointerEvents.OnEnter += PointerEntered;
        PointerEvents.OnExit += PointerExited;
    }

    private void OnDestroy()
    {
        PointerEvents.OnEnter -= PointerEntered;
        PointerEvents.OnExit -= PointerExited;
    }

    private void PointerEntered()
    {
        TooltipInstance = TooltipSystem.Instance.StartTooltip(TooltipPrefab);
        TooltipInstance.Item = Item;
        TooltipInstance.Show();
    }

    private void PointerExited()
    {
        TooltipInstance.Hide();
        TooltipSystem.Instance.EndTooltip(TooltipInstance);
    }
}