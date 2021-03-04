using Unity.Entities;

[GenerateAuthoringComponent]
public struct SpawnerData : IComponentData
{
    public Entity enemyPrefab;
}
