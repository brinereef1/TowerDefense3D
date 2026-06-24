using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // All waves available in the level
    [SerializeField] WaveData[] waves;

    // Which wave is currently active
    [SerializeField] private int _currentWaveIndex = 0;

    // Timer used to determine when the next enemy should spawn
    [SerializeField] float spawnTime;

    // Counter to check if the current wave is finished
    [SerializeField] int spawnCounter;

    [SerializeField] int enemiesRemoved;

    // Object pools for each enemy type
    [SerializeField] ObjectPooler spherePool;
    [SerializeField] ObjectPooler capsulePool;
    [SerializeField] ObjectPooler cylinderPool;

    // Allows us to quickly get the correct pool from an EnemyType
    private Dictionary<EnemyType, ObjectPooler> _pooledObjects;

    // Shortcut for the currently selected wave
    private WaveData currentWave => waves[_currentWaveIndex];

    private void Awake()
    {
        // Connect each enemy type to its corresponding object pool
        _pooledObjects = new Dictionary<EnemyType, ObjectPooler>()
        {
            { EnemyType.sphere, spherePool },
            { EnemyType.capsule, capsulePool },
            { EnemyType.cylinder, cylinderPool }
        };
    }

    private void OnEnable()
    {
        Enemy.EnemiesRemovedData += HandleEnemiesRemoved;
    }

    private void OnDisable()
    {
        Enemy.EnemiesRemovedData -= HandleEnemiesRemoved;
    }

    private void Update()
    {
        // Keep counting time every frame
        spawnTime += Time.deltaTime;

        // Spawn certain number of enemies according to the current wave enemy numbers in time interval
        if (spawnTime > currentWave.SpawnTime && spawnCounter < currentWave.NumberOfEnemies)
        {
            spawnTime = 0;
            SpawnObjects();
            spawnCounter++;
        } 
        // when current wave enemies are spawned move on to the next wave 
        else if (spawnCounter >= currentWave.NumberOfEnemies && enemiesRemoved >= currentWave.NumberOfEnemies)
        {
            _currentWaveIndex = (_currentWaveIndex + 1) % waves.Length;
            spawnCounter = 0;
            enemiesRemoved = 0;
        }
    }

    void SpawnObjects()
    {
        // Find the correct pool for the current wave's enemy type
        if (_pooledObjects.TryGetValue(currentWave.Type, out var pool))
        {
            // Get an inactive enemy from the pool
            GameObject spawnedObject = pool.GetPooledObjects();

            // Spawn it at the SpawnManager's position
            spawnedObject.transform.position = transform.position;

            // Activate it so it starts moving
            spawnedObject.SetActive(true);
        }
    }

    void HandleEnemiesRemoved(EnemyData data)
    {
        // Increament enemies removed count to track if all the enemies of the current wave reached the end
        enemiesRemoved++;
    }
}