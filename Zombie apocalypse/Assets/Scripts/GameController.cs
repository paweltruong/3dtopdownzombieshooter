using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// responsible for general game/zombie level logic
/// </summary>
public class GameController : MonoBehaviour
{
    public bool isPaused;
    public static GameController instance;
    UILevelController uiLevelController;
    ECSManager ecsManager;
    NavigationController navigation;


    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        uiLevelController = FindObjectOfType<UILevelController>();
        if (uiLevelController == null)
            Debug.LogError($"{nameof(UILevelController)} not found in the scene");

        ecsManager = FindObjectOfType<ECSManager>();
        navigation = FindObjectOfType<NavigationController>();

        ecsManager.PerformLevelSetUp();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            TogglePause();

        if (!ecsManager.IsPlayerAlive())
            ShowEndScreen();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }
    public void ShowEndScreen()
    {
        if (!isPaused)
            TogglePause();
        uiLevelController.ShowGameResults(Time.timeSinceLevelLoad);
    }

    public void EndGame()
    {
        ecsManager.PerformLevelCleanUp();
        if(isPaused)
            TogglePause();
        navigation.GotoMenu();
    }
}
