using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// responsible for navigating between scenes
/// </summary>
public class NavigationController : MonoBehaviour
{
    public void GotoGame()
    {
        SceneManager.LoadScene(Constants.Scenes.LevelScene);
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene(Constants.Scenes.MenuScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
