using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("UI")]
    public GameObject pausePanel;

    public bool isPaused;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetPauseState(false);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.gameOver)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPauseState(!isPaused);
        }
    }

    public void Pause()
    {
        SetPauseState(true);
    }

    public void Resume()
    {
        SetPauseState(false);
    }

    void SetPauseState(bool state)
    {
        isPaused = state;

        Time.timeScale = state ? 0f : 1f;

        pausePanel.SetActive(state);

        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;

        PlayClickSound();
    }

    void PlayClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(
                AudioManager.Instance.buttonClip
            );
        }
    }
}