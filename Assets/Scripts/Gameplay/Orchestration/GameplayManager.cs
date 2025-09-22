using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

public class GameplayManager : MonoBehaviour
{
    public event Action OnRunStart;
    public event Action OnRunEnd;

    public event Action OnStageStart;
    public event Action<StageEndReason> OnStageEnd;

    [Header("References")]
    [Required]
    [SceneObjectsOnly]
    public LevelManager LevelManager;
    [Required]
    [SceneObjectsOnly]
    public GameplayTimer GameplayTimer;
    [Required]
    [SceneObjectsOnly]
    public GameplayMoney GameplayMoney;

    [Required]
    [AssetsOnly]
    public GameObject PlayerHUDComboPrefab;

    [Required]
    [AssetsOnly]
    public GameObject InBetweenStagePrefab;

    public int CurrentStageNumber => StageConfigStack.Count;

    private GameObject PlayerHUDComboInstance;
    public PlayerManager PlayerManager { get; private set; }
    private Canvas PlayerHUDCanvas;
    public LevelResult LevelResult { get; private set; }
    private GameObject InBetweenStageInstance;

    private bool IsInRun;
    private bool IsInStage;

    private RunConfig RunConfig;
    private Stack<StageConfig> StageConfigStack = new();

    private UnityAction GameplayTimerTimeoutAction;

    private void Start()
    {
        GameplayTimerTimeoutAction = () => EndStage(StageEndReason.Timeout);
        GameplayTimer.OnTimeout.AddListener(GameplayTimerTimeoutAction);

        InBetweenStageInstance = Instantiate(InBetweenStagePrefab);
    }

    private void OnDestroy()
    {
        GameplayTimer.OnTimeout.RemoveListener(GameplayTimerTimeoutAction);

        if (InBetweenStageInstance)
        {
            Destroy(InBetweenStageInstance);
        }

        LevelResult?.Dispose();
        LevelManager.Cleanup();
    }

    [PropertySpace(20)]
    [Button]
    [EnableInPlayMode]
    [DisableIf(nameof(IsInRun))]
    public void StartRun(RunConfig runConfig)
    {
        if (IsInRun)
        {
            return;
        }

        Debug.Log("[GameplayManager] Start run!");

        LevelResult?.Dispose();
        LevelManager.Cleanup();

        RunConfig = runConfig;
        StageConfigStack.Clear();
        IsInRun = true;
        OnRunStart?.Invoke();
    }

    [Button]
    [EnableInPlayMode]
    [EnableIf(nameof(IsInRun))]
    public void EndRun()
    {
        if (!IsInRun)
        {
            return;
        }

        if (IsInStage)
        {
            EndStage(StageEndReason.EndRun);
        }

        Debug.Log("[GameplayManager] End run!");

        IsInRun = false;
        OnRunEnd?.Invoke();
    }

    [PropertySpace(10)]
    [Button]
    [EnableInPlayMode]
    [EnableIf(nameof(IsInRun))]
    [DisableIf(nameof(IsInStage))]
    public async Task StartStage(StageConfig stageConfig)
    {
        if (!IsInRun || IsInStage)
        {
            return;
        }

        Debug.Log("[GameplayManager] Start stage!");

        if (InBetweenStageInstance)
        {
            Destroy(InBetweenStageInstance);
        }

        LevelResult?.Dispose();
        LevelManager.Cleanup();

        PlayerHUDComboInstance = Instantiate(PlayerHUDComboPrefab);
        PlayerManager = PlayerHUDComboInstance.GetComponentInChildren<PlayerManager>();
        PlayerHUDCanvas = PlayerHUDComboInstance.GetComponentInChildren<Canvas>();

        var player = PlayerManager.Player;
        player.Character.SetIsActive(false);

        LevelResult = LevelManager.Generate(RunConfig.Seed + CurrentStageNumber);
        LevelResult.ExitZone.OnTrigger += LevelExitZoneTriggered;

        PlayerManager.transform.position = LevelResult.SpawnBounds.center + Vector3.up;

        GameplayMoney.Initialize(new PlayerInventoryMoney(player.Inventory));
        player.Stats.Health.OnDeplete.AddListener(() => EndStage(StageEndReason.Death));

        GameplayTimer.Begin(stageConfig.Duration);

        StageConfigStack.Push(stageConfig);
        IsInStage = true;
        OnStageStart?.Invoke();

        NextFrameSetPlayerActive(player.Character, true);
    }

    private async void NextFrameSetPlayerActive(PlayerCharacter character, bool value)
    {
        await Awaitable.NextFrameAsync();
        character.SetIsActive(value);
    }

    [Button]
    [EnableInPlayMode]
    [EnableIf(nameof(IsInRun))]
    [EnableIf(nameof(IsInStage))]
    public void EndStage(StageEndReason reason)
    {
        if (!IsInRun || !IsInStage)
        {
            return;
        }

        Debug.Log("[GameplayManager] End stage!");

        Destroy(PlayerHUDComboInstance);
        GameplayTimer.End();

        InBetweenStageInstance = Instantiate(InBetweenStagePrefab);

        IsInStage = false;
        OnStageEnd?.Invoke(reason);
    }

    private void LevelExitZoneTriggered()
    {
        EndStage(StageEndReason.ReachExit);
    }
}
