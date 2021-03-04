using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDifficultySettings", menuName = "Zombie Apocalypse/Game Difficulty Settings")]
public class GameDifficultySettings : ScriptableObject
{
    [Tooltip("Number of enemies that spawn at once")]
    public int NumberOfEnemiesToSpawn = 3;
    [Tooltip("In seconds")]
    public float SpawnInterval = 0.2f;
    public int PlayerMaxHp = 50000;
    public int EnemyMaxHp = 100;
}
