using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject[] UI;
    private GameObject countdown;

    private void Start()
    {
        UI = GameObject.FindGameObjectsWithTag("UI");
        countdown = GameObject.FindGameObjectWithTag("Countdown");
    }

    void Update()
    {
        if (countdown.activeSelf == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        if (gameIsPaused == true)
        {
            pauseMenuUI.SetActive(false);
            for (int i = 0; i < UI.Length; i++)
                UI[i].SetActive(true);
            Time.timeScale = 1f;
            gameIsPaused = false;
        }
    }

    void Pause()
    {
        for (int i = 0; i < UI.Length; i++)
            UI[i].SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}