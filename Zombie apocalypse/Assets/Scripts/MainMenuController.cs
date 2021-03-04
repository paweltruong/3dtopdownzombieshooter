using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// responsible for UI logic in Menu Scene
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("UI logic")]
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject difficultyPanel;

    [Header("Difficulty settings")]
    [SerializeField] GameDifficultySettings easy;
    [SerializeField] GameDifficultySettings medium;
    [SerializeField] GameDifficultySettings hard;

    NavigationController navigation;

    void Start()
    {
        navigation = FindObjectOfType<NavigationController>();

        if (navigation == null)
            Debug.LogError($"Scene does not contain {nameof(NavigationController)}");
        if (easy == null || medium == null || hard == null)
            Debug.LogError($"Difficulty settings not set");
        if (mainMenuPanel == null || difficultyPanel == null)
            Debug.LogError($"UI not set");

        difficultyPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowNewGame()
    {
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        difficultyPanel.SetActive(false);
    }

    public void PlayOnEasy()
    {
        PlayOnDifficulty(easy);
    }
    public void PlayOnMedium()
    {
        PlayOnDifficulty(medium);
    }
    public void PlayOnHard()
    {
        PlayOnDifficulty(hard);
    }

    void PlayOnDifficulty(GameDifficultySettings difficulty)
    {

        GameDataManager.instance.Difficulty = difficulty;
        navigation.GotoGame();

    }
}
