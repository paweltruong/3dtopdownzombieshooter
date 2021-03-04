using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;


public abstract class CastSpellSystemSystem : SystemBase
{
    public float cooldownTimer = 0;

    protected override void OnUpdate()
    {
        if (this.IsLevel() && !GameController.instance.isPaused)
        {
            float deltaTime = Time.DeltaTime;
            var muzzleOffset = new float3(0, GameDataManager.instance.EnemyLevelY, 1);
            var casting = Input.GetKeyDown(KeyCode.Alpha1);

            if (cooldownTimer <= 0 && casting)
            {
                Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .WithName(nameof(PlayerShootingSystem))
                .ForEach((ref PlayerData player, ref Translation position, ref Rotation rotation) =>
                {
                    var fireStrikeEntity = World.EntityManager.Instantiate(player.fireStrikePrefab);
                    this.SetCreatedProjectileTransforms(
                        projectileEntity: fireStrikeEntity,
                        muzzleOffset: muzzleOffset,
                        shooterPostion: position.Value,
                        shooterRotation: rotation.Value);

                    EntityManager.AddBuffer<EnemyBySpellIndexBufferElement>(fireStrikeEntity);

                    cooldownTimer = GameDataManager.instance.FireStrikeCooldown;
                })
                .Run();
            }
            else
            {
                cooldownTimer -= Time.DeltaTime;
            }
        }
    }

    public void CastSpell(KeyCode keyForSpell, SpellIds spell, float spellCooldown )
    {
        if (this.IsLevel() && !GameController.instance.isPaused)
        {
            float deltaTime = Time.DeltaTime;
            var muzzleOffset = new float3(0, GameDataManager.instance.EnemyLevelY, 1);
            var casting = Input.GetKeyDown(keyForSpell);

            if (cooldownTimer <= 0 && casting)
            {
                Entities
                .WithoutBurst()
                .WithStructuralChanges()
                .ForEach((ref PlayerData player, ref Translation position, ref Rotation rotation) =>
                {
                    Entity prefab;
                    switch (spell)
                    {
                        case SpellIds.FireStrike:
                            prefab = player.fireStrikePrefab;
                            break;
                        case SpellIds.IceBlast:
                            prefab = player.iceBlastPrefab;
                            break;
                        default:
                            throw new System.ArgumentException($"Unknown spell: {spell}");
                    }

                    var spellEntity = World.EntityManager.Instantiate(prefab);
                    this.SetCreatedProjectileTransforms(
                        projectileEntity: spellEntity,
                        muzzleOffset: muzzleOffset,
                        shooterPostion: position.Value,
                        shooterRotation: rotation.Value);

                    EntityManager.AddBuffer<EnemyBySpellIndexBufferElement>(spellEntity);

                    cooldownTimer = spellCooldown;
                })
                .Run();
            }
            else
            {
                cooldownTimer -= Time.DeltaTime;
            }
        }
    }
}
