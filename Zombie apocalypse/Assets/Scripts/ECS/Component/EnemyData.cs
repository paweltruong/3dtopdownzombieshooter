using Unity.Entities;

[GenerateAuthoringComponent]
public struct EnemyData : IComponentData
{
    public float speed;
    public int currentHealth;
    public bool alreadyExploded;
    public float iceBlastDebuffTimer;
}
