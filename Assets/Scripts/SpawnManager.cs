using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] WaveData[] waves;
    [SerializeField] private int _currentWaveIndex = 0;
    [SerializeField] float spawnTime;

    [SerializeField] ObjectPooler spherePool;
    [SerializeField] ObjectPooler capsulePool;
    [SerializeField] ObjectPooler cylinderPool;

    private Dictionary<EnemyType, ObjectPooler> _pooledObjects;

    private WaveData currentWave => waves[_currentWaveIndex];

    private void Awake()
    {
        _pooledObjects = new Dictionary<EnemyType, ObjectPooler>()
        {
            {EnemyType.sphere, spherePool},
            {EnemyType.capsule, capsulePool},
            {EnemyType.cylinder, cylinderPool},
        };    
    }

    private void Update()
    {
        // Adding spawn time to delta time and reseting it after completion of 1 secs, spawning object every 1 secs
        spawnTime += Time.deltaTime;
        if (spawnTime > currentWave.SpawnTime)
        {
            spawnTime = 0;
            SpawnObjects();
        }
    }

   void SpawnObjects()
    {
        //Spawning objects to this gameobject initial position
        if (_pooledObjects.TryGetValue(currentWave.Type, out var pool))
        {
            GameObject spawnedObjects = pool.GetPooledObjects();
            spawnedObjects.transform.position = transform.position;
            spawnedObjects.SetActive(true);
        }
    }
}
