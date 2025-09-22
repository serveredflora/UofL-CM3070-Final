using UnityEngine;
using UnityEngine.UI;

public class SimpleSegmentedProgressBarSegment : ASegmentedProgressBarSegment
{
    [Header("Settings")]
    [SerializeField]
    private Color ActiveColor;
    [SerializeField]
    private Color InactiveColor;

    [Header("References")]
    [SerializeField]
    private Image Image;

    public override void SetActive(bool value)
    {
        Active = value;

        Image.color = Active ? ActiveColor : InactiveColor;
    }
}
