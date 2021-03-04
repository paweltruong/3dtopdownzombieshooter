using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class IceBlastTickSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (this.IsLevel())
        {
            var deltaTime = Time.DeltaTime;

            Entities
                .WithBurst(synchronousCompilation: true)
                .ForEach((Entity entity, int entityInQueryIndex, ref EnemyData enemy) =>
            {
                if (enemy.iceBlastDebuffTimer > 0)
                    enemy.iceBlastDebuffTimer -= deltaTime;
            })
            .ScheduleParallel();
        }
    }
}