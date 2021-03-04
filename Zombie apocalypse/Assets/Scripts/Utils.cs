using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// extensions and helper methods
/// </summary>
public static class Utils
{
    /// <summary>
    /// set position/translation and rotation for projectile entity based on shooter position/translation and rotation
    /// </summary>
    /// <param name="system"></param>
    /// <param name="projectileEntity"></param>
    /// <param name="muzzleOffset">offset from shooter base position f.e gun's muzzle position relative to shooter position</param>
    /// <param name="shooterPostion"></param>
    /// <param name="shooterRotation"></param>
    public static void SetCreatedProjectileTransforms(this ComponentSystemBase system, Entity projectileEntity, float3 muzzleOffset, float3 shooterPostion, quaternion shooterRotation)
    {
        system.EntityManager.SetComponentData(projectileEntity, new Translation { Value = shooterPostion + math.mul(shooterRotation, muzzleOffset) });
        system.EntityManager.SetComponentData(projectileEntity, new Rotation { Value = shooterRotation });
    }

    /// <summary>
    /// checks if game is in zombie game level
    /// </summary>
    /// <param name="system"></param>
    /// <returns></returns>
    public static bool IsLevel(this ComponentSystemBase system)
    {
        return system.EntityManager.World.GetExistingSystem<GameStateSystem>().isLevel;
    }

    /// <summary>
    /// formats time in seconds to {0.0}s
    /// </summary>
    /// <param name="cooldown">value in seconds</param>
    /// <returns></returns>
    public static string ToCooldownString(this float cooldown)
    {
        if (cooldown <= 0)
            return string.Empty;
        else
            return $"{cooldown:F1}s";
    }
}