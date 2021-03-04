using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class PlayerShootingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (this.IsLevel() && !GameController.instance.isPaused)
        {
            float deltaTime = Time.DeltaTime;
            var muzzleOffset = new float3(0, GameDataManager.instance.EnemyLevelY / 2, 1);

            var shooting = Input.GetMouseButtonDown(0);
            if (shooting)
            {
                //TODO:ad buffer
                Entities
                    .WithoutBurst()
                    .WithStructuralChanges()
                    .WithName(nameof(PlayerShootingSystem))
                    .ForEach((ref PlayerData player, ref Translation position, ref Rotation rotation) =>
                    {

                        var bullet = World.EntityManager.Instantiate(player.bulletPrefab);
                        this.SetCreatedProjectileTransforms(
                                projectileEntity: bullet,
                                muzzleOffset: muzzleOffset,
                                shooterPostion: position.Value,
                                shooterRotation: rotation.Value);
                    })
                .Run();
            }
        }
    }
}
