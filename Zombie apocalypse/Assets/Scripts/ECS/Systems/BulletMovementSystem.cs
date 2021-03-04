using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class BulletMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (this.IsLevel())
        {
            float deltaTime = Time.DeltaTime;

            var baseSpeed = 40f * 4;

            var jobHandle = Entities
                .WithName(nameof(BulletMovementSystem))
                .ForEach((ref BulletData bullet, ref Translation position, ref Rotation rotation, ref PhysicsVelocity physics) =>
                {
                    physics.Angular = float3.zero;
                    physics.Linear = baseSpeed * deltaTime * math.forward(rotation.Value);
                })
                .ScheduleParallel(Dependency);

            jobHandle.Complete();            
        }
    }
}
