using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private CityGenerator CityGenerator;

    private void Start()
    {
        CityGenerator.Generate();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
