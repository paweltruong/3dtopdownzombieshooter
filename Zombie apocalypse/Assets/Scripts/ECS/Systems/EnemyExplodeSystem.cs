using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class EnemyExplodeSystem : SystemBase
{
    BuildPhysicsWorld physicsWorld;
    StepPhysicsWorld stepWorld;

    protected override void OnCreate()
    {
        physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    [BurstCompile]
    struct TriggerJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<PlayerData> PlayerDataGroup;
        public ComponentDataFromEntity<EnemyData> EnemyDataGroup;

        public int damageToPlayer;

        public void Execute(TriggerEvent triggerEvent)
        {
            var entityA = triggerEvent.EntityA;
            var entityB = triggerEvent.EntityB;

            bool isBodyAPlayer = PlayerDataGroup.HasComponent(entityA);
            bool isBodyBPlayer = PlayerDataGroup.HasComponent(entityB);

            bool isBodyAEnemy = EnemyDataGroup.HasComponent(entityA);
            bool isBodyBEnemy = EnemyDataGroup.HasComponent(entityB);

            if ((isBodyAPlayer && !isBodyBEnemy)
                || (isBodyBPlayer && !isBodyAEnemy))
            {
                //Player with not enemy
                return;
            }

            if (!isBodyAPlayer && !isBodyBPlayer)
            {
                //No player involved
                return;
            }
            if (!isBodyAEnemy && !isBodyBEnemy)
            {
                //No enemies involved
                return;
            }


            var playerEntity = isBodyAPlayer ? entityA : entityB;
            var enemyEntity = isBodyAEnemy ? entityA : entityB;

            var playerDataComponent = PlayerDataGroup[playerEntity];
            var enemyDataComponent = EnemyDataGroup[enemyEntity];

            if (enemyDataComponent.currentHealth > 0 && !enemyDataComponent.alreadyExploded)
            {
                enemyDataComponent.currentHealth = 0;
                enemyDataComponent.alreadyExploded = true;
                EnemyDataGroup[enemyEntity] = enemyDataComponent;
                //Enemy {enemyEntity.Index} exploded on player dealing {damageToPlayer} damage

                if (playerDataComponent.currentHealth > 0)
                {
                    playerDataComponent.currentHealth -= damageToPlayer;
                    PlayerDataGroup[playerEntity] = playerDataComponent;
                }
                else
                {
                    //Player is already dead
                }
            }
            else
            {
                //Enemy {enemyEntity.Index} is already
            }

        }
    }


    protected override void OnUpdate()
    {
        if (this.IsLevel())
        {
            //TODO:parallel?
            var jobHandle = new TriggerJob
            {
                PlayerDataGroup = GetComponentDataFromEntity<PlayerData>(),
                EnemyDataGroup = GetComponentDataFromEntity<EnemyData>(),
                damageToPlayer = GameDataManager.instance.EnemyDamage,
            }
                .Schedule(stepWorld.Simulation, ref physicsWorld.PhysicsWorld, Dependency);

            jobHandle.Complete();

        }
    }
}
