using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayPressed()
    {
        SceneManager.LoadScene("Game");
        Destroy(GameObject.Find("Audio Source"));
    }

    public void MenuPressed()
    {
        SceneManager.LoadScene("Menu");
        Destroy(GameObject.Find("Audio Source"));
    }

    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }
}
