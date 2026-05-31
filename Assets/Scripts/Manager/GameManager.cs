using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;

    public float timeLeft = 60f;

    public bool gameOver;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (gameOver)
            return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = 0;

            EndGame();
        }
    }

    public void CompleteDelivery()
    {
        if (gameOver)
            return;

        score++;

        timeLeft += 10f;
    }

    void EndGame()
    {
        gameOver = true;

        AudioManager.Instance.FadeOutMusic(1.5f);

        AudioManager.Instance.PlaySFX(
            AudioManager.Instance.gameOverClip
        );

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}