using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General game settings
/// </summary>
public class GameDataManager : MonoBehaviour
{
    public GameDifficultySettings Difficulty;

    public bool debugSpawn;
    public float debugEnemySpeedMultiplier = 1f;

    public float SpawnRadius = 25f;
    public float EnemyBaseSpeed = 0.3f;
    public float EnemyHeight = 0.5f;
    public float EnemyLevelY = .25f;
    public int EnemyBaseHealth = 100;
    public int EnemyDamage = 50;
    public int BulletDamage = 75;

    public float FireStrikeSpeed = 15f;
    public float FireStrikeCooldown = .4f;
    public int FireStrikeDamage = 60;

    public float IceBlastSpeed = 10f;
    public float IceBlastCooldown = .8f;
    public float IceBlastDuration = 5f;
    public float IceBlastSlow = .5f;

    public float MaxPlayerDistanceForObject = 30f;

    public static GameDataManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
