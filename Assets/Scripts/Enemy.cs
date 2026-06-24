using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Current waypoint the enemy is moving toward
    private Vector3 _targetPosition;

    // Which waypoint we are currently trying to reach
    private int _currentIndexPath = 0;

    // Distance between enemy and current waypoint
    private float _reachedDestiny;

    // Reference to the Path script
    private Path _currentPath;

    // Contains this enemy's stats
    [SerializeField] EnemyData data;

    public static event Action<EnemyData> EnemiesRemovedData;

    private void Awake()
    {
        // Find the path in the scene
        _currentPath = GameObject.Find("Path").GetComponent<Path>();
    }

    private void OnEnable()
    {
        // Whenever this enemy is reused from the pool,
        // start from the first waypoint
        _targetPosition = _currentPath.GetWayPoints(_currentIndexPath);
    }

    private void Update()
    {
        // Move toward the current waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            _targetPosition + data.Offset,
            data.Speed * Time.deltaTime);

        // Check how far away we are from the waypoint
        _reachedDestiny = (_targetPosition - transform.position).magnitude;

        // If we've reached the waypoint...
        if (_reachedDestiny <= 1.1f)
        {
            // ...and there are more waypoints ahead
            if (_currentIndexPath < _currentPath.WayPoints.Length - 1)
            {
                // Move to the next waypoint
                _currentIndexPath++;
                _targetPosition = _currentPath.GetWayPoints(_currentIndexPath);
            }
            else
            {
                // Reached the end of the path

                // Return enemy to the pool
                gameObject.SetActive(false);

                // Reset path index so next spawn starts from waypoint 0
                _currentIndexPath = 0;

                // Sending the enemies data which has reached the last waypoint
                EnemiesRemovedData?.Invoke(data);
            }
        }
    }
}