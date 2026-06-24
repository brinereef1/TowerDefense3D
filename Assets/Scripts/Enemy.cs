using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Current waypoint position this enemy is moving toward
    private Vector3 _targetPosition;

    // Index of the current waypoint in the path
    private int _currentIndexPath = 0;

    // Distance from the current waypoint
    private float _reachedDestiny;

    // Reference to the path the enemy follows
    private Path _currentPath;

    // Stats for this enemy type
    [SerializeField] EnemyData data;

    // Event fired when an enemy reaches the end of the path
    public static event Action<EnemyData> EnemiesRemovedData;

    private void Awake()
    {
        // Find the path once and cache the reference
        _currentPath = GameObject.Find("Path").GetComponent<Path>();
    }

    private void OnEnable()
    {
        // Whenever this enemy is reused from the pool,
        // start again from the first waypoint
        _targetPosition = _currentPath.GetWayPoints(_currentIndexPath);
    }

    private void Update()
    {
        // Move toward the current target waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            _targetPosition + data.Offset,
            data.Speed * Time.deltaTime);

        // Calculate distance to the target waypoint
        _reachedDestiny =
            (_targetPosition - transform.position).magnitude;

        // If close enough, consider the waypoint reached
        if (_reachedDestiny <= 1.1f)
        {
            // Continue to the next waypoint if one exists
            if (_currentIndexPath < _currentPath.WayPoints.Length - 1)
            {
                _currentIndexPath++;

                _targetPosition =
                    _currentPath.GetWayPoints(_currentIndexPath);
            }
            else
            {
                // Enemy has reached the end of the path

                // Return enemy to the object pool
                gameObject.SetActive(false);

                // Reset path progress for future reuse
                _currentIndexPath = 0;

                // Notify systems that this enemy has completed the path
                EnemiesRemovedData?.Invoke(data);
            }
        }
    }
}