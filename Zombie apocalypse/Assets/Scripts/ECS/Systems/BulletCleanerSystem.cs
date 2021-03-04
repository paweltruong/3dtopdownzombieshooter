﻿using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class BulletCleanerSystem : SystemBase
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
            var commands = commandBuffer.CreateCommandBuffer().AsParallelWriter();
            var maxDistance = GameDataManager.instance.MaxPlayerDistanceForObject;

            Entities
                .WithBurst(synchronousCompilation: true)
                .ForEach((Entity entity, int entityInQueryIndex, ref BulletData bullet, ref Translation position) =>
            {
                //TODO: destroy bullets outside 25 range
                if (bullet.alreadyUsedOnEnemy || math.distance(position.Value, float3.zero) > maxDistance)
                    commands.DestroyEntity(entityInQueryIndex, entity);

            })
            .ScheduleParallel();
            commandBuffer.AddJobHandleForProducer(this.Dependency);
        }
    }
}