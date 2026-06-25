using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    public EnemyType Type;
    public float SpawnTime;
    public int NumberOfEnemies;
}
