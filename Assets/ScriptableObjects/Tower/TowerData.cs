using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    public float Range;
    public float ShootInterval;
    public float ProjectileSpeed;
    public float ProjectileDuration;
    public float Damage;
}
