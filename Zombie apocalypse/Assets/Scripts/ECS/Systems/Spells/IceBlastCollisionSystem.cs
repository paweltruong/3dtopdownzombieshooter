using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class IceBlastCollisionSystem : SpellCollisionSystemBase
{
    protected override void OnUpdate()
    {
        ExecuteSpellTriggerJob<IceBlastData>(SpellIds.IceBlast);
    }
}
