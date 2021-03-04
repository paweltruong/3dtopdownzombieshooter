using Unity.Entities;

[InternalBufferCapacity(30)]
public struct EnemyBySpellIndexBufferElement : IBufferElementData
{
    public int EnemyEntityIndex;
    public int SpellEntityIndex;

    public static bool operator ==(EnemyBySpellIndexBufferElement elem1, EnemyBySpellIndexBufferElement elem2)
    {
        return elem1.EnemyEntityIndex == elem2.EnemyEntityIndex && elem1.SpellEntityIndex == elem2.SpellEntityIndex;
    }
    public static bool operator !=(EnemyBySpellIndexBufferElement elem1, EnemyBySpellIndexBufferElement elem2)
    {
        return elem1.EnemyEntityIndex != elem2.EnemyEntityIndex || elem1.SpellEntityIndex != elem2.SpellEntityIndex;
    }
    //public override bool Equals(object obj)
    //{
    //    return base.Equals(obj);
    //}
    //public override int GetHashCode()
    //{
    //    return base.GetHashCode();
    //}
}