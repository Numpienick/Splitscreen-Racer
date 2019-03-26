using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private ModeSelector modeSelect;

    private void Start()
    {
        modeSelect = FindObjectOfType<ModeSelector>();
    }
    public void PlayGameAI()
    {
        modeSelect.mode = "ai";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGameVS()
    {
        modeSelect.mode = "vs";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}