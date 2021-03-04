using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.LowLevel;

public class ECSManager : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    EntityManager manager;
    BlobAssetStore blobAssetStore;
    GameStateSystem gameStateSystem;

    private void Awake()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();

        gameStateSystem = manager.World.GetExistingSystem<GameStateSystem>();        
    }

    void Start()
    {        
        if (gameStateSystem == null)
            Debug.LogError($"System {nameof(GameStateSystem)} not found");
    }
    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }

    public void PerformLevelCleanUp()
    {
        gameStateSystem.levelCleanUpPending = true;
    }
    public void PerformLevelSetUp()
    {
        gameStateSystem.levelSetUpPending = true;
    }
    
    public bool IsPlayerAlive()
    {
        var query = manager.CreateEntityQuery(typeof(PlayerData));
        if (gameStateSystem.isLevelInitialized && !query.IsEmpty)
        {
            var playerEntity = manager.CreateEntityQuery(typeof(PlayerData)).GetSingletonEntity();
            var playerData = manager.GetComponentData<PlayerData>(playerEntity);
            return playerData.currentHealth > 0;
        }
        return true;
    }
}
