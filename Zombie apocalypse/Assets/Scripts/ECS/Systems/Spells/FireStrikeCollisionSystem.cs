using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class FireStrikeCollisionSystem : SpellCollisionSystemBase
{    
    protected override void OnUpdate()
    {
        ExecuteSpellTriggerJob<FireStrikeData>(SpellIds.FireStrike);
    }
}
