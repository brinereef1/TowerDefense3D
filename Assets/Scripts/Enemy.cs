using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector3 _targetPosition;
    private int _currentIndexPath = 0;
    private float _reachedDestiny;

    private Path _currentPath;

    [SerializeField] EnemyData data;

    private void Awake()
    {
        //Find the Path gameobject
        _currentPath = GameObject.Find("Path").GetComponent<Path>();
    }

    private void OnEnable()
    {
        //Set target position to the first element of the waypoints list
        _targetPosition = _currentPath.GetWayPoints(_currentIndexPath);
    }

    private void Update()
    {
        // Move the enemy forward at moveSpeed units per second
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition + data.Offset, data.Speed * Time.deltaTime);

        //When enemy reaches it's target position change the target position to the next waypoint
        _reachedDestiny = (_targetPosition - transform.position).magnitude;
        if (_reachedDestiny <= 1.1f)
        {
            if (_currentIndexPath < _currentPath.WayPoints.Length - 1)
            {
                _currentIndexPath++;
                _targetPosition = _currentPath.GetWayPoints(_currentIndexPath);
            } else
            {
                gameObject.SetActive(false);
                _currentIndexPath = 0;
            }
        }
    }
}
