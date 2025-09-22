using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayStageExitHUD : MonoBehaviour
{
    [Header("Settings")]
    public string DistancePrefixText = "<size=50%>Distance:</size>\n<b>";
    public string DistancePostfixText = "</b>";

    [Header("References")]
    public GameplayManager GameplayManager;
    public CanvasGroup CanvasGroup;
    public Image Compass;
    public TMP_Text DistanceText;

    private bool IsShown;

    private PlayerCharacter PlayerCharacter;
    private GameObject StageExitZoneGameObject;
    private Camera Cam;

    private void Start()
    {
        Hide();

        GameplayManager.OnRunStart += GameplayRunStart;
        GameplayManager.OnRunEnd += GameplayRunEnd;
        GameplayManager.OnStageStart += GameplayStageStart;
        GameplayManager.OnStageEnd += GameplayStageEnd;
    }

    private void OnDestroy()
    {
        GameplayManager.OnRunStart -= GameplayRunStart;
        GameplayManager.OnRunEnd -= GameplayRunEnd;
        GameplayManager.OnStageStart -= GameplayStageStart;
        GameplayManager.OnStageEnd -= GameplayStageEnd;
    }

    private void Update()
    {
        if (!IsShown)
        {
            return;
        }

        UpdateInfo();
    }

    public void Show()
    {
        // As these are instantiated/destroyed during runtime, we need to dynamically find these
        PlayerCharacter = GameplayManager.PlayerManager.Player.Character;
        StageExitZoneGameObject = GameplayManager.LevelResult.ExitZoneGameObject;
        Cam = Camera.main;

        UpdateInfo();

        HUDUtils.SetCanvasGroupState(CanvasGroup, true);
        IsShown = true;
    }

    public void Hide()
    {
        PlayerCharacter = null;
        StageExitZoneGameObject = null;

        HUDUtils.SetCanvasGroupState(CanvasGroup, false);
        IsShown = false;
    }

    private void UpdateInfo()
    {
        Vector3 diff = StageExitZoneGameObject.transform.position - PlayerCharacter.transform.position;
        float length = diff.magnitude;
        Vector3 dir = diff / length;

        // TODO: calculate this!
        float projectedAngle = MathF.Sin(Time.time) * 360.0f;
        Compass.rectTransform.localRotation = Quaternion.Euler(0.0f, projectedAngle, 0.0f);

        DistanceText.text = DistancePrefixText + length.ToString("F2") + DistancePostfixText;
    }

    private void GameplayRunStart()
    {
        Hide();
    }

    private void GameplayRunEnd()
    {
        Hide();
    }

    private void GameplayStageStart()
    {
        Show();
    }

    private void GameplayStageEnd(StageEndReason reason)
    {
        Hide();
    }
}
