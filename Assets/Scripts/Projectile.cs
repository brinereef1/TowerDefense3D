using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Tower stats used by this projectile.
    [SerializeField] TowerData _data;

    // Enemy this projectile is currently tracking.
    [SerializeField] Enemy targetEnemy;

    // Time before this projectile automatically returns to the pool.
    [SerializeField] float projectileDuration;

    private void Update()
    {
        // Return the projectile to the pool once its lifetime expires.
        if (projectileDuration <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        // Count down the projectile's remaining lifetime.
        projectileDuration -= Time.deltaTime;

        // Return the projectile if its target no longer exists.
        if (targetEnemy == null)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            // Continuously follow the target enemy.
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetEnemy.transform.position,
                _data.ProjectileSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Damage the enemy on contact, then return the projectile to the pool.
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(_data.Damage);

            gameObject.SetActive(false);
        }
    }

    public void Shoot(TowerData data, Enemy enemy)
    {
        // Initialize the projectile before firing.
        _data = data;
        targetEnemy = enemy;
        projectileDuration = data.ProjectileDuration;
    }
}