using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class EnemyMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (this.IsLevel())
        {
            float deltaTime = Time.DeltaTime;
            var baseSpeed = GameDataManager.instance.debugEnemySpeedMultiplier * GameDataManager.instance.EnemyBaseSpeed;//45f
            var iceBlastSlowFactor = GameDataManager.instance.IceBlastSlow;

            Entities
                .WithBurst(synchronousCompilation: true)
                .WithName(nameof(EnemyMovementSystem))
                .ForEach((Entity entity, ref EnemyData enemy, ref Translation position, ref Rotation rotation, ref PhysicsMass mass, ref PhysicsVelocity physics) =>
                {
                    //rotate towards player
                    var targetPoint = new float3(0, 0, 0);
                    float3 newForward = targetPoint - position.Value;
                    quaternion targetDirection = quaternion.LookRotation(newForward, math.up());
                    rotation.Value = targetDirection;

                    //set rotation speed to zero
                    physics.Angular = float3.zero;

                    //move forward
                    float3 linearForce = math.forward(rotation.Value) * baseSpeed * deltaTime;
                    if (enemy.iceBlastDebuffTimer > 0)
                        linearForce *= iceBlastSlowFactor;

                    var newPosition = position.Value;
                    newPosition.x += linearForce.x;
                    newPosition.z += linearForce.z;
                    position.Value = newPosition;
                    
                    //freeze rotation on X and Z
                    mass.InverseInertia[0] = 0;
                    mass.InverseInertia[2] = 0;
                })
                .ScheduleParallel();
        }
    }
}
