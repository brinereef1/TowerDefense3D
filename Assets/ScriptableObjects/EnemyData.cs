using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int Lives;
    public int Damage;
    public int Speed;
    public Vector3 Offset;
}
