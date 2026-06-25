using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<int> OnLivesChanged;

    private int _lives = 20;

    private void Start()
    {
        OnLivesChanged?.Invoke(_lives);
    }

    private void OnEnable()
    {
        Enemy.EnemiesRemovedData += HandlePlayerLives;
    }

    private void OnDisable()
    {
        Enemy.EnemiesRemovedData -= HandlePlayerLives;
    }

    void HandlePlayerLives(EnemyData data)
    {
        _lives = Mathf.Max(0, _lives - data.Damage);
        OnLivesChanged?.Invoke(_lives);
    }
}
