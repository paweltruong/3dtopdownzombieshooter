using System.Collections;
using System.Collections.Generic;
using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;


public class PlayerRotationSystem : ComponentSystem
{
    BuildPhysicsWorld buildPhysicsWorld;
    CollisionFilter filter;


    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<BuildPhysicsWorld>();

        filter = CollisionFilter.Default;
        filter.CollidesWith = (uint)PhysicsCategories.Ground;
    }

    protected override void OnUpdate()
    {
        if (this.IsLevel() && !GameController.instance.isPaused)
        {
            //get ray from camera to mouse position
            var ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

            Entities.WithAll<PlayerData>().ForEach((Entity player, ref Translation position, ref Rotation rotation) =>
            {
                if (CastRay(ray.origin, ray.origin + ray.direction * 100, out Unity.Physics.RaycastHit result, buildPhysicsWorld.PhysicsWorld.CollisionWorld, filter))
                {
                    var newForward = result.Position - position.Value;
                    newForward.y = 0;
                    rotation.Value = Quaternion.LookRotation(newForward, Vector3.up);
                }
            });
        }
    }

    public static bool CastRay(float3 start, float3 end, out Unity.Physics.RaycastHit result, CollisionWorld world, CollisionFilter filter)
    {
        var input = new RaycastInput
        {
            Start = start,
            End = end,
            Filter = filter,
        };
        return world.CastRay(input, out result);
    }
}
