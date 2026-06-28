using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    // ScriptableObject containing this tower's stats.
    [SerializeField] private TowerData data;

    // Trigger collider used to detect enemies within range.
    private SphereCollider sphereCollider;

    // List of enemies currently inside this tower's range.
    public List<Enemy> enemiesInRange;

    // Object pool used to reuse projectiles.
    [SerializeField] ObjectPooler projectilePooler;

    // Timer controlling the shooting interval.
    private float shootTimer;

    private void Start()
    {
        // Set the detection radius based on the tower's range.
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = data.Range;

        // Initialize the enemy list.
        enemiesInRange = new List<Enemy>();

        // Cache the projectile object pool attached to this tower.
        projectilePooler = GetComponent<ObjectPooler>();

        // Start ready to fire after the configured interval.
        shootTimer = data.ShootInterval;
    }

    private void OnEnable()
    {
        // Remove enemies from the target list when they leave the game.
        Enemy.EnemyRemoved += RemoveEnemy;
    }

    private void OnDisable()
    {
        // Stop listening when this tower is disabled.
        Enemy.EnemyRemoved -= RemoveEnemy;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        // Remove the enemy if it exists in this tower's range list.
        enemiesInRange.Remove(enemy);
    }

    private void Update()
    {
        // Count time until the next shot.
        shootTimer += Time.deltaTime;

        if (shootTimer >= data.ShootInterval)
        {
            shootTimer = 0;
            Shoot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Add enemies that enter the tower's attack range.
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove enemies that leave the tower's attack range.
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

    private void Shoot()
    {
        // Remove any invalid enemy references as a safety check.
        enemiesInRange.RemoveAll(enemy =>
            enemy == null || !enemy.gameObject.activeInHierarchy);

        // Shoot at the first enemy currently in range.
        if (enemiesInRange.Count > 0)
        {
            // Reuse a projectile from the object pool.
            GameObject projectile = projectilePooler.GetPooledObjects();

            // Spawn the projectile at the tower's position.
            projectile.transform.position = transform.position;

            // Activate the projectile.
            projectile.SetActive(true);

            // Pass the tower data and target enemy to the projectile.
            projectile.GetComponent<Projectile>().Shoot(data, enemiesInRange[0]);
        }
    }
}