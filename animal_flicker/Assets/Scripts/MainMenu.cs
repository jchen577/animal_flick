using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject panel;

    public void StartGame()
    {
        SceneManager.LoadScene("Flick the Animal");
    }

    public void ShowCredits()
    {
        panel.SetActive(true);
    }

    public void HideCredits()
    {
        panel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}

