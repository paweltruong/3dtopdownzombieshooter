using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public abstract class SpellCollisionSystemBase : SystemBase
{
    protected BuildPhysicsWorld physicsWorld;
    protected StepPhysicsWorld stepWorld;

    protected override void OnCreate()
    {
        physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected void ExecuteSpellTriggerJob<T>(SpellIds spell)
        where T : struct, IComponentData
    {
        if (this.IsLevel())
        {


            var jobHandle = new SpellTriggerJob<T>
            {
                SpellDataGroup = GetComponentDataFromEntity<T>(),
                EnemyDataGroup = GetComponentDataFromEntity<EnemyData>(),
                EnemyBySpellIndexBufferElementGroup = GetBufferFromEntity<EnemyBySpellIndexBufferElement>(),
                SpellDamage = GetSpellDamage(spell),
                DebuffDuration = GetDebuffDuration(spell),
                SpellId = (int)spell,
            }
            .Schedule(stepWorld.Simulation, ref physicsWorld.PhysicsWorld, Dependency);
            jobHandle.Complete();
        }

    }

    int GetSpellDamage(SpellIds spell)
    {
        if (spell == SpellIds.FireStrike)
            return GameDataManager.instance.FireStrikeDamage;
        return 0;
    }

    float GetDebuffDuration(SpellIds spell)
    {
        if (spell == SpellIds.IceBlast)
            return GameDataManager.instance.IceBlastDuration;
        return 0;
    }
}
