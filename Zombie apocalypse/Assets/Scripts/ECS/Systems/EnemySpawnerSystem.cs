using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class EnemySpawnerSystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem commandBuffer;
    float timeToNextSpawn = 0;
    public Unity.Mathematics.Random Random;

    protected override void OnCreate()
    {
        commandBuffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        var seed = new System.Random();
        Random = new Unity.Mathematics.Random((uint)seed.Next());
    }

    public void Setup()
    {
        timeToNextSpawn = GameDataManager.instance.Difficulty.SpawnInterval;
    }

    protected override void OnUpdate()
    {
        if (this.IsLevel())
        {
            if (timeToNextSpawn <= 0)
            {
                var commands = commandBuffer.CreateCommandBuffer();

                var baseHealth = GameDataManager.instance.EnemyBaseHealth;
                var spawnEnemyCount = GameDataManager.instance.Difficulty.NumberOfEnemiesToSpawn;
                //Debug.Log($"Spawn enemy count: {spawnEnemyCount}");

                //Create random position for enemies
                var randomSpawnPositions = new NativeArray<float3>(spawnEnemyCount, Allocator.TempJob);
                for (int i = 0; i < spawnEnemyCount; ++i)
                    randomSpawnPositions[i] = RandomWithinRadius(GameDataManager.instance.SpawnRadius);

                //Create random color for enemies
                var randomColors = new NativeArray<Color>(spawnEnemyCount, Allocator.TempJob);
                for (int i = 0; i < spawnEnemyCount; ++i)
                    randomColors[i] = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), 0.6f, 0.6f);

                var isDebugMode = GameDataManager.instance.debugSpawn;

                //TODO: extract to job struct and Execute
                Entities
                    .WithBurst(synchronousCompilation: true)
                    .ForEach((Entity entity, int entityInQueryIndex, ref SpawnerData spawner) =>
                {
                    if (isDebugMode)
                    {
                        var rndPos = randomSpawnPositions[0];
                        var color = randomColors[0];
                        var deferredEnemy = commands.Instantiate(spawner.enemyPrefab);
                        commands.SetComponent(deferredEnemy, new Translation { Value = new float3(25, 0, 0) });
                        commands.SetComponent(deferredEnemy, new EnemyData { currentHealth = baseHealth });
                        //destroy after spawn
                        commands.DestroyEntity(entity);
                    }
                    else
                    {
                        for (int i = 0; i < spawnEnemyCount; ++i)
                        {
                            var deferredEnemy = commands.Instantiate(spawner.enemyPrefab);

                            var spawnPosition = randomSpawnPositions[i];
                            //fix model rotation
                            var targetPoint = new float3(0, 0, 0);
                            float3 newForward = targetPoint - spawnPosition;
                            quaternion targetDirection = quaternion.LookRotation(newForward, math.up());
                            commands.SetComponent(deferredEnemy, new Rotation { Value = targetDirection });

                            //set random position
                            commands.SetComponent(deferredEnemy, new Translation { Value = spawnPosition });
                            //Debug.Log($"Spawning at ({randomSpawnPositions[i].x},{randomSpawnPositions[i].y},{randomSpawnPositions[i].z})");
                            //set random material color
                            var color = randomColors[i];
                            commands.SetComponent(deferredEnemy, new URPMaterialPropertyBaseColor { Value = new float4(color.r, color.g, color.b, color.a) });
                        }
                    }
                })
                .WithDisposeOnCompletion(randomSpawnPositions)
                .WithDisposeOnCompletion(randomColors)
                .Schedule();

                commandBuffer.AddJobHandleForProducer(this.Dependency);

                //reset next wave spawn timer
                timeToNextSpawn = GameDataManager.instance.Difficulty.SpawnInterval;

                //Update game progres
                var gameProgressEntity = GetSingletonEntity<GameProgressData>();
                var gameProgressData = GetComponent<GameProgressData>(gameProgressEntity);
                gameProgressData.enemiesAlive += spawnEnemyCount;
                SetComponent(gameProgressEntity, gameProgressData);
            }
            else
            {
                timeToNextSpawn -= Time.DeltaTime;
                //Debug.Log($"TimeSpawn:{timeToNextSpawn}");
            }
        }

    }
    float3 RandomWithinRadius(float radius)
    {
        //player position 0,0,0
        Vector3 center = Vector3.zero;
        var angle = Random.NextFloat(0, 360);
        float3 position;
        position.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        position.y = 0;//enemy offset above ground
        position.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        return position;
    }
}