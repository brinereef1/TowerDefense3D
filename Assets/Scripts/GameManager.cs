using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Notifies the UI whenever the player's lives change.
    public static event Action<int> OnLivesChanged;

    // Player's starting lives.
    private int _lives = 20;

    private void Start()
    {
        // Display the initial lives when the game starts.
        OnLivesChanged?.Invoke(_lives);
    }

    private void OnEnable()
    {
        // Listen for enemies reaching the end of the path.
        Enemy.EnemyReachedGoal += HandlePlayerLives;
    }

    private void OnDisable()
    {
        // Stop listening when this object is disabled.
        Enemy.EnemyReachedGoal -= HandlePlayerLives;
    }

    void HandlePlayerLives(EnemyData data)
    {
        // Reduce player lives based on the enemy's damage value.
        _lives = Mathf.Max(0, _lives - data.Damage);

        // Notify the UI with the updated lives count.
        OnLivesChanged?.Invoke(_lives);
    }
}