using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class FireStrikeMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (this.IsLevel())
        {
            float deltaTime = Time.DeltaTime;
            var baseSpeed = GameDataManager.instance.FireStrikeSpeed * GameDataManager.instance.debugEnemySpeedMultiplier;//15units

            Entities
                .WithBurst(synchronousCompilation: true)
                .WithName(nameof(FireStrikeMovementSystem))
                .ForEach((ref FireStrikeData fireStrike, ref Translation position, ref Rotation rotation, ref PhysicsVelocity physics) =>
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
