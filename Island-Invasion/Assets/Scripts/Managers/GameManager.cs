using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static bool gameStarted = false;
    public static bool gameEnded = false;
    public static bool gamePaused = false;

    public int levelToUnlock = 2;

    UIManager UIManager;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Gamemanager in scene");
            return;
        }
        instance = this;

        UIManager = GetComponent<UIManager>();
    }

    float timer;
    private void Start()
    {
        UIManager.SetTimerText(string.Format("{0:D3}", 00), string.Format("{0:D2}:{1:D2}:{2:D2}", 00, 00, 00));
    }
    private void Update()
    {
        if (Input.GetKeyDown(InputManager.instance.pauseGame))
        {
            if (gamePaused)
                ResumeGame();
            else
                PauseGame();
        }
        if (gameStarted)
        {
            timer += Time.deltaTime;
            System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
            string timerFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
            string msFormatted = string.Format("{0:D3}", t.Milliseconds);
            UIManager.SetTimerText(msFormatted, timerFormatted);
        }
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            Time.timeScale = 1f;
            AudioManager.instance.Stop("Theme");

            Debug.Log("TODO ADD JINGLE");

            AudioManager.instance.Play("Fight");
            gameStarted = true;
        }
    }

    public void WinLevel()
    {
        Time.timeScale = 0f;
        UIManager.EndScreen("Win");
        gameEnded = true;
        gameStarted = false;
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
    }
    public void EndGame()
    {
        if (!gameEnded && gameStarted)
        {
            UIManager.EndScreen("Lose");

            AudioManager.instance.Stop("Fight");

            Debug.Log("TODO ADD JINGLE");

            AudioManager.instance.Play("Lose");

            gameEnded = true;
            gameStarted = false;
            Time.timeScale = 0f;
        }
    }

    public void NexLevel()
    {
        gameStarted = false;
        gameEnded = false;
        WaveSpawner.EnemiesAlive = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Retry()
    {
        gameStarted = false;
        gameEnded = false;
        WaveSpawner.EnemiesAlive = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GotToMainMenu()
    {
        gameStarted = false;
        gameEnded = false;
        gamePaused = false;
        WaveSpawner.EnemiesAlive = 0;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        gamePaused = true;
        UIManager.TogglePauseMenu();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        gamePaused = false;
        UIManager.TogglePauseMenu();
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
