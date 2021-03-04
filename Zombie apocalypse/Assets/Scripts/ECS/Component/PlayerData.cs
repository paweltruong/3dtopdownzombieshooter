using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerData : IComponentData
{
    public Entity bulletPrefab;
    public Entity fireStrikePrefab;
    public Entity iceBlastPrefab;
    public int currentHealth;
}
