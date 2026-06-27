using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] TowerData _data;
    [SerializeField] Vector3 projectileDirection;
    [SerializeField] float projectileDuration;

    private void Update()
    {
        if (projectileDuration <= 0)
        {
            gameObject.SetActive(false);
        } else
        {
            projectileDuration -= Time.deltaTime;
            transform.position += projectileDirection *
                      _data.ProjectileSpeed *
                      Time.deltaTime;
        }
    }

    public void Shoot(TowerData data, Vector3 direction) 
    {
        _data = data;
        projectileDirection = direction;
        projectileDuration = data.ProjectileDuration;

    }
}
