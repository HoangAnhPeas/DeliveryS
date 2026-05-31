using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject hintUI;
    public TMP_Text hintText;

    public TMP_Text scoreText;
    public TMP_Text timerText;

    public GameObject gameOverPanel;

    void Awake()
    {
        Instance = this;
        hintUI.SetActive(false);
    }

    public void ShowHint(string text)
    {
        hintUI.SetActive(true);
        hintText.text = text;
    }

    public void HideHint()
    {
        hintUI.SetActive(false);
    }

    void Update()
    {
        GameManager gm = GameManager.Instance;

        scoreText.text =
            "Score: " + gm.score;

        timerText.text =
            "Time: " + Mathf.Ceil(gm.timeLeft);

        gameOverPanel.SetActive(gm.gameOver);
    }
}