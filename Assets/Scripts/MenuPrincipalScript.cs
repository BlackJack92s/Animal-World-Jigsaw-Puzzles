using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class menuScript_ : MonoBehaviour
{
    public void Home()
    { 
        SceneManager.LoadScene("MainMenu");
    }

    public void Levels()
    { 
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void QuitBJT ()
    { 
        Application.Quit();
    }

    public void EASY()
    {
        //PlayerPrefs.SetInt("Level", LevelNumber);
        SceneManager.LoadScene("Gameeasy");
    }

    public void iNTERMEDIATE()
    {
        //PlayerPrefs.SetInt("Level", LevelNumber);
        SceneManager.LoadScene("GameInt");
    }

    public void ADVANCED()
    {
        //PlayerPrefs.SetInt("Level", LevelNumber);
        SceneManager.LoadScene("Gameadv");
    }

    public void EXPERT()
    {
        //PlayerPrefs.SetInt("Level", LevelNumber);
        SceneManager.LoadScene("Game");
    }
}
