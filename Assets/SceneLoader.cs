using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevelSelectionScreen()
    {
        SceneManager.LoadScene("LevelSelectionScreen");
    }

public void QuitGame()
    {
Application.Quit();
Debug.Log("Quit!");
    }
}