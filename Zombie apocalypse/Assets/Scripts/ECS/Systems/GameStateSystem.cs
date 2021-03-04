using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameStateSystem : SystemBase
{
    public bool levelCleanUpPending;
    public bool levelSetUpPending;
    public bool isLevel;
    public bool isLevelInitialized;

    protected override void OnUpdate()
    {
        if (levelCleanUpPending)
        {
            EntityManager.DestroyEntity(EntityManager.UniversalQuery);

            levelCleanUpPending = false;
            isLevel = false;
            isLevelInitialized = false;
            ToggleLevelSystems(false);
        }
        if (levelSetUpPending)
        {
            var playerInitialHp = GameDataManager.instance.Difficulty.PlayerMaxHp;
            Entities.WithAll<PlayerData>().ForEach((Entity player, ref PlayerData playerData) =>
            {
                playerData.currentHealth = playerInitialHp;


            }).Schedule();
            levelSetUpPending = false;

            ToggleLevelSystems(true);
            isLevel = true;
            isLevelInitialized = true;
        }        
    }

    void ToggleLevelSystems(bool enabled)
    {
        if(enabled)
        {
            EntityManager.World.GetOrCreateSystem<EnemySpawnerSystem>().Setup();
        }

        EntityManager.World.GetOrCreateSystem<PlayerRotationSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<PlayerCastFireStrikeSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<PlayerCastIceBlastSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<BulletCleanerSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<BulletCollisionSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<BulletMovementSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<EnemyCleanerSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<EnemyExplodeSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<EnemyMovementSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<EnemySpawnerSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<FireStrikeCollisionSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<FireStrikeMovementSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<FireStrikeCleanerSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<IceBlastCollisionSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<IceBlastMovementSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<IceBlastTickSystem>().Enabled = enabled;
        EntityManager.World.GetOrCreateSystem<IceBlastCleanerSystem>().Enabled = enabled;
    }
}