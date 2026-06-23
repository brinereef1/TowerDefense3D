using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector3 _targetPosition;
    private int _currentIndexPath = 0;
    private float reachedDestiny;

    [SerializeField] private Vector3 offset;

    [SerializeField] private Path currentPath;

    private void Awake()
    {
        //Find the Path gameobject
        currentPath = GameObject.Find("Path").GetComponent<Path>();
    }

    private void OnEnable()
    {
        //Set target position to the first element of the waypoints list
        _targetPosition = currentPath.GetWayPoints(_currentIndexPath);
    }

    private void Update()
    {
        // Move the enemy forward at moveSpeed units per second
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition + offset, moveSpeed * Time.deltaTime);

        //When enemy reaches it's target position change the target position to the next waypoint
        reachedDestiny = (_targetPosition - transform.position).magnitude;
        if (reachedDestiny <= 1.1f)
        {
            if (_currentIndexPath < currentPath.WayPoints.Length - 1)
            {
                _currentIndexPath++;
                _targetPosition = currentPath.GetWayPoints(_currentIndexPath);
            } else
            {
                Destroy(gameObject);
            }
        }
    }
}
