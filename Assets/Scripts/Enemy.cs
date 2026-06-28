using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Current waypoint this enemy is moving toward.
    private Vector3 _targetPosition;

    // Current waypoint index in the path.
    private int _currentIndexPath = 0;

    // Distance between the enemy and its current waypoint.
    private float _reachedDestiny;

    // Reference to the path this enemy follows.
    private Path _currentPath;

    // ScriptableObject containing this enemy's stats.
    [SerializeField] EnemyData data;

    // Fired whenever an enemy leaves the game (death or reaching the goal).
    public static event Action<Enemy> EnemyRemoved;

    // Fired only when an enemy reaches the end of the path.
    public static event Action<EnemyData> EnemyReachedGoal;

    // Current health of this enemy.
    private float lives;

    private void Awake()
    {
        // Cache the path reference instead of searching for it every frame.
        _currentPath = GameObject.Find("Path").GetComponent<Path>();
    }

    private void OnEnable()
    {
        // Reset the enemy when reused from the object pool.
        _targetPosition = _currentPath.GetWayPoints(_currentIndexPath);

        // Restore health for the next spawn.
        lives = data.Lives;
    }

    private void Update()
    {
        // Move towards the current waypoint.
        transform.position = Vector3.MoveTowards(
            transform.position,
            _targetPosition + data.Offset,
            data.Speed * Time.deltaTime);

        // Measure how close the enemy is to its target waypoint.
        _reachedDestiny =
            (_targetPosition - transform.position).magnitude;

        if (_reachedDestiny <= 1.1f)
        {
            // Move to the next waypoint if one exists.
            if (_currentIndexPath < _currentPath.WayPoints.Length - 1)
            {
                _currentIndexPath++;

                _targetPosition =
                    _currentPath.GetWayPoints(_currentIndexPath);
            }
            else
            {
                // Remove the enemy after it reaches the end of the path.
                DisableEnemy(true);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        // Reduce health without allowing it to go below zero.
        lives -= damage;
        lives = Math.Max(lives, 0);

        // Remove the enemy once its health reaches zero.
        if (lives <= 0)
        {
            DisableEnemy(false);
        }
    }

    private void DisableEnemy(bool reachedGoal)
    {
        // Notify every subscribed system that this enemy has left the game.
        EnemyRemoved?.Invoke(this);

        // Notify GameManager only if the enemy escaped.
        if (reachedGoal)
        {
            EnemyReachedGoal?.Invoke(data);
        }

        // Return this enemy to the object pool.
        gameObject.SetActive(false);

        // Reset the path index for the next reuse.
        _currentIndexPath = 0;
    }
}