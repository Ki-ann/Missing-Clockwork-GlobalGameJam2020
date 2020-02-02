using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public PlayableDirector playable;
    public float TotalTime = 0;
    public float TotalJumps = 0;
    public Text timetext;
    public Text jumptext;
    public Animator mainCamera;
    public GameObject Pause;
    public void ResetLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
    public void StartGame()
    {
        playable.Play();
    }
    public void GoCredits()
    {
        mainCamera.SetTrigger("Credits");
    }
    public void GoMenu()
    {
        mainCamera.SetTrigger("Menu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0.0f;
            Pause.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        Pause.SetActive(false);
    }
}
