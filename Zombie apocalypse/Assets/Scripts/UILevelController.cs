using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// responsible for UI logic in zombie level
/// </summary>
public class UILevelController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI enemiesKilledTextField;
    [SerializeField] TextMeshProUGUI enemiesLeftTextField;
    [SerializeField] TextMeshProUGUI fireStrikeCooldownTextField;
    [SerializeField] TextMeshProUGUI iceBlastCooldownTextField;
    [SerializeField] GameObject endGamePanel;
    [SerializeField] TextMeshProUGUI endGameTextField;

    public UnityEvent onFireStrikeActive;
    public UnityEvent onIceBlastActive;

    EntityManager manager;
    bool iceBlastFlash = false;
    bool fireStrikeFlash = false;

    void Awake()
    {
        if (enemiesKilledTextField == null || enemiesLeftTextField == null
            || fireStrikeCooldownTextField == null || iceBlastCooldownTextField == null
            || endGamePanel == null || endGameTextField == null)
            Debug.LogError("UI fields not set");
    }

    void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    void Update()
    {
        UpdateEnemyStats();
        UpdateFireStrikeUI();
        UpdateIceBlastUI();
    }

    void UpdateEnemyStats()
    {
        var query = manager.CreateEntityQuery(typeof(GameProgressData));
        if (!query.IsEmpty)
        {
            var gameProgressEntity = query.GetSingletonEntity();
            var gameProgressData = manager.GetComponentData<GameProgressData>(gameProgressEntity);
            enemiesKilledTextField.text = gameProgressData.enemiesKilled.ToString();
            enemiesLeftTextField.text = gameProgressData.enemiesAlive.ToString();
        }
    }


    void UpdateFireStrikeUI()
    {
        var fireStrikeSystem = manager.World.GetOrCreateSystem<PlayerCastFireStrikeSystem>();
        fireStrikeCooldownTextField.text = fireStrikeSystem.cooldownTimer.ToCooldownString();
        if (fireStrikeSystem.cooldownTimer > 0)
            fireStrikeFlash = true;
        else if (fireStrikeFlash)
        {
            onFireStrikeActive?.Invoke();
            fireStrikeFlash = false;
        }
    }
    void UpdateIceBlastUI()
    {
        var iceBlastSystem = manager.World.GetOrCreateSystem<PlayerCastIceBlastSystem>();
        iceBlastCooldownTextField.text = iceBlastSystem.cooldownTimer.ToCooldownString();
        if (iceBlastSystem.cooldownTimer > 0)
            iceBlastFlash = true;
        else if (iceBlastFlash)
        {
            onIceBlastActive?.Invoke();
            iceBlastFlash = false;
        }
    }

    public void ShowGameResults(float survivedTime)
    {
        endGameTextField.text = string.Format(Constants.Messages.EndGameTextFormat, survivedTime.ToCooldownString());
        endGamePanel.SetActive(true);
    }
}
