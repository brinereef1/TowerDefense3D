using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] float spawnInterval = 1f;
    [SerializeField] float spawnTime;

    [SerializeField] GameObject prefab;

    private void Update()
    {
        // Adding spawn time to delta time and reseting it after completion of 1 secs, spawning object every 1 secs
        spawnTime += Time.deltaTime;
        if (spawnTime > spawnInterval)
        {
            spawnTime = 0;
            SpawnObjects();
        }
    }

    void SpawnObjects()
    {
        //Spawning objects in this gameobject initial position and rotation
        GameObject spawnedObjects = Instantiate(prefab, transform.position, transform.rotation);
    }
}
