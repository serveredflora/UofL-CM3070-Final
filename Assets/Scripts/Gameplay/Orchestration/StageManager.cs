using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public bool IsInStage { get; private set; }

    public IReadOnlyList<StageConfig> Configs => _Configs;
    private List<StageConfig> _Configs = new();
}
