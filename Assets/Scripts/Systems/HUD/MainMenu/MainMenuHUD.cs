using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private MainMenu MainMenu;
    [SerializeField]
    private Button PlayButton;
    [SerializeField]
    private Button QuitButton;

    private void Start()
    {
        PlayButton.onClick.AddListener(PlayButtonClick);
        QuitButton.onClick.AddListener(QuitButtonClick);
    }

    private void OnDestroy()
    {
        PlayButton.onClick.RemoveListener(PlayButtonClick);
        QuitButton.onClick.RemoveListener(QuitButtonClick);
    }

    private void PlayButtonClick()
    {
        MainMenu.StartGame();
    }

    private void QuitButtonClick()
    {
        ApplicationUtils.Quit();
    }
}