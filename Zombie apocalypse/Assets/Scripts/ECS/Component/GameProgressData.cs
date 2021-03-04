using Unity.Entities;

[GenerateAuthoringComponent]
public struct GameProgressData : IComponentData
{
    public int enemiesKilled;
    public int enemiesAlive;
}
