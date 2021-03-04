using System.Linq;
using Unity.Entities;
using Unity.Physics;

struct SpellTriggerJob<T> : ITriggerEventsJob
     where T : struct, IComponentData
{
    public ComponentDataFromEntity<T> SpellDataGroup;
    public ComponentDataFromEntity<EnemyData> EnemyDataGroup;
    public BufferFromEntity<EnemyBySpellIndexBufferElement> EnemyBySpellIndexBufferElementGroup;
    
    //Those below can be put into struct Spell Effect and put into array so different spells could have many effects
    public int SpellDamage;
    public float DebuffDuration;
    public int SpellId;

    public void Execute(TriggerEvent triggerEvent)
    {
        var entityA = triggerEvent.EntityA;
        var entityB = triggerEvent.EntityB;

        bool isBodyASpell = SpellDataGroup.HasComponent(entityA);
        bool isBodyBSpell = SpellDataGroup.HasComponent(entityB);

        if (isBodyASpell && isBodyBSpell)
        {
            //Spell with Spell
            return;
        }

        bool isBodyAEnemy = EnemyDataGroup.HasComponent(entityA);
        bool isBodyBEnemy = EnemyDataGroup.HasComponent(entityB);

        if ((isBodyASpell && !isBodyBEnemy)
            || (isBodyBSpell && !isBodyAEnemy))
        {
            //Spell with not enemy
            return;
        }

        if (!isBodyASpell && !isBodyBSpell)
        {
            //No spell involved
            return;
        }
        if (!isBodyAEnemy && !isBodyBEnemy)
        {
            //No enemies involved
            return;
        }


        var spell = isBodyASpell ? entityA : entityB;
        var enemyEntity = isBodyAEnemy ? entityA : entityB;

        var buffer = EnemyBySpellIndexBufferElementGroup[spell];
        var newBufferElement = new EnemyBySpellIndexBufferElement { EnemyEntityIndex = enemyEntity.Index, SpellEntityIndex = spell.Index };

        if (!buffer.AsNativeArray().Any(be => be == newBufferElement))
        {
            //Apply effects
            var enemyDataComponent = EnemyDataGroup[enemyEntity];
            if (enemyDataComponent.currentHealth > 0 && !enemyDataComponent.alreadyExploded)
            {
                if (SpellDamage > 0)
                {
                    enemyDataComponent.currentHealth -= SpellDamage;
                    EnemyDataGroup[enemyEntity] = enemyDataComponent;
                    //Applied {spellDamage} to {enemyEntity.Index} current hp is {enemyDataComponent.currentHealth}
                }
                if (DebuffDuration > 0 && (SpellIds)SpellId != SpellIds.Unknown)
                {
                    switch (SpellId)
                    {
                        //only one spell with debuf supported atm
                        default:
                        enemyDataComponent.iceBlastDebuffTimer = DebuffDuration;
                            break;
                    }
                    EnemyDataGroup[enemyEntity] = enemyDataComponent;
                }

                buffer.Add(newBufferElement);
            }
            else
            {
                //Enemy {enemyEntity.Index} is dead
            }
        }
    }
}