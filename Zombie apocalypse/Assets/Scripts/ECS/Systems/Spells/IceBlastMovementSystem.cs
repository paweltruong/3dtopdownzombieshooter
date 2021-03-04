using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class IceBlastMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (this.IsLevel())
        {
            float deltaTime = Time.DeltaTime;
            var baseSpeed = GameDataManager.instance.IceBlastSpeed * GameDataManager.instance.debugEnemySpeedMultiplier;//10units

            Entities
                .WithBurst(synchronousCompilation: true)
                .WithName(nameof(IceBlastMovementSystem))
                .ForEach((ref IceBlastData fireStrike, ref Translation position, ref Rotation rotation, ref PhysicsVelocity physics) =>
                {
                    physics.Angular = float3.zero;

                    float3 linearForce = math.forward(rotation.Value) * baseSpeed * deltaTime;
                    var newPosition = position.Value;
                    newPosition.x += linearForce.x;
                    newPosition.z += linearForce.z;

                    position.Value = newPosition;
                })
                .ScheduleParallel();
        }
    }
}
