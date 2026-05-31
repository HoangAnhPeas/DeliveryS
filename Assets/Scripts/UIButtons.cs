using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class UIButtons : MonoBehaviour
{
    [Header("Audio")]
    public float clickDelay = 0.2f;

    public void RestartGame()
    {
        RunWithClickDelay(() =>
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        });
    }

    public void LoadMenu()
    {
        RunWithClickDelay(() =>
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene("Menu");
        });
    }

    public void LoadGameScene()
    {
        RunWithClickDelay(() =>
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene("Game");
        });
    }

    public void QuitGame()
    {
        RunWithClickDelay(() =>
        {
            Debug.Log("Quit Game");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    void RunWithClickDelay(Action action)
    {
        StartCoroutine(ClickCoroutine(action));
    }

    IEnumerator ClickCoroutine(Action action)
    {
        PlayButtonAudio();

        yield return new WaitForSecondsRealtime(clickDelay);

        action?.Invoke();
    }

    public void PlayButtonAudio()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(
                AudioManager.Instance.buttonClip
            );
        }
    }
}