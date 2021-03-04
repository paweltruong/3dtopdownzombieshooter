using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Assertions;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class EnemyCleanerSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem commandBuffer;

    protected override void OnCreate()
    {
        commandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        if (this.IsLevel())
        {
            var commands = commandBuffer.CreateCommandBuffer();
            var query = GetEntityQuery(ComponentType.ReadWrite<EnemyData>());
            var entities = query.ToEntityArray(Allocator.TempJob);
            NativeList<bool> enemiesToCleanStatus = new NativeList<bool>(Allocator.TempJob);

            var jobHandle = Entities
                  .ForEach((Entity entity, ref EnemyData enemy) =>
                  {
                      if (enemy.alreadyExploded)
                      {
                          //Add true if reached player and exploded
                          enemiesToCleanStatus.Add(true);
                          commands.DestroyEntity(entity);
                      }
                      else if (enemy.currentHealth <= 0)
                      {
                          //Add false if was killed by player
                          enemiesToCleanStatus.Add(false);
                          commands.DestroyEntity(entity);
                      }
                  })
                .Schedule(Dependency);

            commandBuffer.AddJobHandleForProducer(this.Dependency);
            jobHandle.Complete();

            if (enemiesToCleanStatus.Length > 0)
            {
                enemiesToCleanStatus.Sort();
                int exploded = GetExplodedCount(enemiesToCleanStatus.ToArray());

                //Update game progres
                var gameProgressEntity = GetSingletonEntity<GameProgressData>();
                var gameProgressData = GetComponent<GameProgressData>(gameProgressEntity);
                gameProgressData.enemiesKilled += enemiesToCleanStatus.Length - exploded;
                gameProgressData.enemiesAlive -= enemiesToCleanStatus.Length;
                SetComponent(gameProgressEntity, gameProgressData);
                //Debug.Log($"toDel:{enemiesToClean.Length}");
            }

            enemiesToCleanStatus.Dispose();
            entities.Dispose();
        }
    }

    public int GetExplodedCount(bool[] enemiesToCleanStatus)
    {
        int foundExplodedIndex = -1;
        for (int i = 0; i < enemiesToCleanStatus.Length; ++i)
        {
            if (enemiesToCleanStatus[i])
            {
                foundExplodedIndex = i;
                break;
            }
        }
        if (foundExplodedIndex == 0)
            return enemiesToCleanStatus.Length;
        else if (foundExplodedIndex > 0)
            return enemiesToCleanStatus.Length - foundExplodedIndex;
        return 0;
    }
}