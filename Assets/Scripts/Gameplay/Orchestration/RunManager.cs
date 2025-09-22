using UnityEngine;

public class RunManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private StageManager StageManager;

    public bool IsInRun { get; private set; }
    public RunConfig CurrentConfig { get; private set; }
}
