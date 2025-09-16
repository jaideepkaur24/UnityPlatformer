 using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // Keyboard input for pause (Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }
#endif

#if UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif
    }

    // Touch input zones
    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch t in Input.touches)
            {
                // Top-left corner = toggle pause
                if (t.phase == TouchPhase.Began && t.position.x < Screen.width * 0.2f && t.position.y > Screen.height * 0.8f)
                {
                    if (pauseScreen.activeInHierarchy)
                        PauseGame(false);
                    else
                        PauseGame(true);
                }

                // Top-right corner = return to main menu
                if (t.phase == TouchPhase.Began && t.position.x > Screen.width * 0.8f && t.position.y > Screen.height * 0.8f)
                {
                    MainMenu();
                }

                // Middle-right area = restart level
                if (t.phase == TouchPhase.Began && t.position.x > Screen.width * 0.7f && t.position.y < Screen.height * 0.7f)
                {
                    Restart();
                }
            }
        }
    }

    #region Game Over Functions
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);
        Time.timeScale = status ? 0 : 1;
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    #endregion
}
