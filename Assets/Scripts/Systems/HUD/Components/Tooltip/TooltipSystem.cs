using UnityEngine;

public class TooltipSystem : ASingleton<TooltipSystem>
{
    public T StartTooltip<T>(T tooltipPrefab)
        where T : Tooltip
    {
        var tooltipInstance = Instantiate(tooltipPrefab, transform);
        return tooltipInstance;
    }

    public void EndTooltip<T>(T tooltipInstance)
        where T : Tooltip
    {
        Destroy(tooltipInstance.gameObject);
    }
}