using UnityEngine;

public class PlayerInteractionHUDGridCellTooltip : Tooltip
{
    [Header("References")]
    [SerializeField]
    private TMPro.TMP_Text NameText;
    [SerializeField]
    private TMPro.TMP_Text DescriptionText;
    [SerializeField]
    private TMPro.TMP_Text MoneyValueText;

    public Item Item;

    protected override bool TryShowInternal()
    {
        NameText.text = Item.Definition.DisplayName;
        DescriptionText.text = Item.Definition.DisplayDescription;
        MoneyValueText.text = "<size=75%><color=#ffffff7f>Sell Value:</color></size> " + Item.Definition.SellValue.ToString();

        return true;
    }

    protected override bool TryHideInternal()
    {
        // TODO: ...

        return true;
    }
}