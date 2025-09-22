using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayPauseHUD : MonoBehaviour
{
    [Header("References")]
    public GameplayTimeManager GameplayTimeManager;

    [Space(10)]
    public CanvasGroup CanvasGroup;
    public Slider TimeScaleSlider;
    public Button ResumeButton;
    public Button ExitToMainMenuButton;
    public Button QuitButton;

    private void Start()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, false);

        GameplayTimeManager.OnPause += GameplayPause;
        GameplayTimeManager.OnResume += GameplayResume;

        TimeScaleSlider.onValueChanged.AddListener(TimeScaleSliderValueChange);
        ResumeButton.onClick.AddListener(ResumeButtonClick);
        ExitToMainMenuButton.onClick.AddListener(ExitToMainMenuButtonClick);
        QuitButton.onClick.AddListener(QuitButtonClick);
    }

    private void OnDestroy()
    {
        GameplayTimeManager.OnPause -= GameplayPause;
        GameplayTimeManager.OnResume -= GameplayResume;

        TimeScaleSlider.onValueChanged.RemoveListener(TimeScaleSliderValueChange);
        ResumeButton.onClick.RemoveListener(ResumeButtonClick);
        ExitToMainMenuButton.onClick.RemoveListener(ExitToMainMenuButtonClick);
        QuitButton.onClick.RemoveListener(QuitButtonClick);
    }

    private void GameplayPause()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, true);
    }

    private void GameplayResume()
    {
        HUDUtils.SetCanvasGroupState(CanvasGroup, false);
    }

    private void TimeScaleSliderValueChange(float value)
    {
        GameplayTimeManager.SetTimeScale(value);
    }

    private void ResumeButtonClick()
    {
        GameplayTimeManager.Resume();
    }

    private void ExitToMainMenuButtonClick()
    {
        GameplayTimeManager.Resume();

        // TODO: get scene build index for main menu scene (somehow)
        SceneManager.LoadScene("MainMenu");
    }

    private void QuitButtonClick()
    {
        GameplayTimeManager.Resume();
        ApplicationUtils.Quit();
    }
}
