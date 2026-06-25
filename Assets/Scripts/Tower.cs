using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData data;
    private SphereCollider sphereCollider;

    public List<Enemy> enemiesRange;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = data.Range;

        enemiesRange = new List<Enemy>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, data.Range);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = GetComponent<Enemy>();
            enemiesRange.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = GetComponent<Enemy>();
            if(enemiesRange.Contains(enemy))
            {
                enemiesRange.Remove(enemy);
            }
        }
    }
}
