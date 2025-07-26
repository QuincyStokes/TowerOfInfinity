using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MenuButtons : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void ResumeGame(GameObject pauseMenu)
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void PauseGame(GameObject pauseMenu)
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
}
