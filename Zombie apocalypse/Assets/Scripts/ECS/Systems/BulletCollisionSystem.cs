using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class BulletCollisionSystem : SystemBase
{
    BuildPhysicsWorld physicsWorld;
    StepPhysicsWorld stepWorld;

    EndSimulationEntityCommandBufferSystem commandBuffer;

    protected override void OnCreate()
    {
        physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepWorld = World.GetOrCreateSystem<StepPhysicsWorld>();

        commandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    struct TriggerJob : ITriggerEventsJob
    {
        public ComponentDataFromEntity<BulletData> BulletDataGroup;
        public ComponentDataFromEntity<EnemyData> EnemyDataGroup;
        public EntityCommandBuffer commandBuffer;

        public int projectileDamage;

        public void Execute(TriggerEvent triggerEvent)
        {
            var entityA = triggerEvent.EntityA;
            var entityB = triggerEvent.EntityB;

            bool isBodyABullet = BulletDataGroup.HasComponent(entityA);
            bool isBodyBBullet = BulletDataGroup.HasComponent(entityB);

            if (isBodyABullet && isBodyBBullet)
            {
                //Bullet with bullet
                return;
            }

            bool isBodyAEnemy = EnemyDataGroup.HasComponent(entityA);
            bool isBodyBEnemy = EnemyDataGroup.HasComponent(entityB);

            if ((isBodyABullet && !isBodyBEnemy)
                || (isBodyBBullet && !isBodyAEnemy))
            {
                //Bullet with not enemy
                return;
            }

            if (!isBodyABullet && !isBodyBBullet)
            {
                //No bullet involved
                return;
            }
            if (!isBodyAEnemy && !isBodyBEnemy)
            {
                //No enemies involved
                return;
            }


            var bulletEntity = isBodyABullet ? entityA : entityB;
            var enemyEntity = isBodyAEnemy ? entityA : entityB;

            var bulletDataComponent = BulletDataGroup[bulletEntity];
            var enemyDataComponent = EnemyDataGroup[enemyEntity];
            if (!bulletDataComponent.alreadyUsedOnEnemy)
            {
                if (enemyDataComponent.currentHealth > 0)
                {
                    enemyDataComponent.currentHealth -= projectileDamage;
                    EnemyDataGroup[enemyEntity] = enemyDataComponent;

                    bulletDataComponent.alreadyUsedOnEnemy = true;
                    BulletDataGroup[bulletEntity] = bulletDataComponent;

                    //remove collider for bullet to disable triggering events
                    commandBuffer.RemoveComponent<PhysicsCollider>(bulletEntity);
                    //commandBuffer.DestroyEntity(bulletEntity);//TODO:test pooling

                    //Applied {projectileDamage} to {enemyEntity.Index} current hp is {enemyDataComponent.currentHealth}
                }
                else
                {
                    //Enemy {enemyEntity.Index} is dead
                }
            }
            else
            {
                ///Bullet {bulletEntity.Index} already used
            }
        }
    }


    protected override void OnUpdate()
    {
        if (this.IsLevel())
        {
            var localCommandBuffer = commandBuffer.CreateCommandBuffer();

            var jobHandle = new TriggerJob
            {
                BulletDataGroup = GetComponentDataFromEntity<BulletData>(),
                EnemyDataGroup = GetComponentDataFromEntity<EnemyData>(),
                projectileDamage = GameDataManager.instance.BulletDamage,
                commandBuffer = localCommandBuffer
            }
            .Schedule(stepWorld.Simulation, ref physicsWorld.PhysicsWorld, Dependency);

            commandBuffer.AddJobHandleForProducer(jobHandle);

            jobHandle.Complete();
        }
    }
}
