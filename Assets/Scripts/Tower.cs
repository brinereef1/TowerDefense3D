using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData data;
    private SphereCollider sphereCollider;
    public List<Enemy> enemiesRange;
    [SerializeField] ObjectPooler projectilePooler;
    private float shootTimer;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = data.Range;
        enemiesRange = new List<Enemy>();
        projectilePooler = GetComponent<ObjectPooler>();
        shootTimer = data.ShootInterval;
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= data.ShootInterval)
        {
            shootTimer = 0;
            Shoot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemiesRange.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemiesRange.Contains(enemy))
            {
                enemiesRange.Remove(enemy);
            }
        }
    }

    private void Shoot()
    {
        if (enemiesRange.Count > 0)
        {
            GameObject projectile = projectilePooler.GetPooledObjects();
            projectile.transform.position = transform.position;
            projectile.SetActive(true);
            Collider enemyCollider = enemiesRange[0].GetComponent<Collider>();

            Vector3 target = enemyCollider.bounds.center;

            Vector3 _shootDirection =
                (target - transform.position).normalized;
            // Draw a red line showing where the tower thinks it's aiming
            Debug.Log($"Tower : {transform.position}");
            Debug.Log($"Enemy : {enemiesRange[0].transform.position}");
            Debug.DrawRay(transform.position, _shootDirection * 5f, Color.red, 1f);
            projectile.GetComponent<Projectile>().Shoot(data, _shootDirection);
        }
    }
}
