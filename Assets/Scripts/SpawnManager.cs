using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // List of all waves that will be played in this level
    [SerializeField] WaveData[] waves;

    // Tracks which wave is currently active
    [SerializeField] private int _currentWaveIndex = 0;

    // Timer used to control spawn intervals
    [SerializeField] float spawnTime;

    // Number of enemies spawned in the current wave
    [SerializeField] int spawnCounter;

    // Number of enemies that have completed the path
    [SerializeField] int enemiesRemoved;

    // True while waiting between waves
    [SerializeField] bool inBetween;

    // Timer for the wave break
    [SerializeField] float wavePauseTimer;

    // Separate object pools for each enemy type
    [SerializeField] ObjectPooler spherePool;
    [SerializeField] ObjectPooler capsulePool;
    [SerializeField] ObjectPooler cylinderPool;

    // Maps an enemy type to its corresponding object pool
    private Dictionary<EnemyType, ObjectPooler> _pooledObjects;

    // Shortcut for accessing the current wave data
    private WaveData currentWave => waves[_currentWaveIndex];

    private void Awake()
    {
        // Create a lookup table so we can easily find
        // the correct pool from an EnemyType
        _pooledObjects = new Dictionary<EnemyType, ObjectPooler>()
        {
            { EnemyType.sphere, spherePool },
            { EnemyType.capsule, capsulePool },
            { EnemyType.cylinder, cylinderPool }
        };
    }

    private void OnEnable()
    {
        // Listen for enemies reaching the end of the path
        Enemy.EnemiesRemovedData += HandleEnemiesRemoved;
    }

    private void OnDisable()
    {
        // Always unsubscribe when disabled to avoid duplicate subscriptions
        Enemy.EnemiesRemovedData -= HandleEnemiesRemoved;
    }

    private void Update()
    {
        // Handle the delay between waves
        if (inBetween)
        {
            wavePauseTimer += Time.deltaTime;

            // Move to the next wave after a short break
            if (wavePauseTimer >= 2)
            {
                wavePauseTimer = 0;

                // Move to the next wave and loop back to the first
                // wave when reaching the end of the array
                _currentWaveIndex = (_currentWaveIndex + 1) % waves.Length;

                // Reset counters for the new wave
                spawnCounter = 0;
                enemiesRemoved = 0;

                // Allow spawning again
                inBetween = false;
            }
        }
        else
        {
            // Continuously count time for spawning enemies
            spawnTime += Time.deltaTime;

            // Spawn enemies until this wave reaches its limit
            if (spawnTime > currentWave.SpawnTime &&
                spawnCounter < currentWave.NumberOfEnemies)
            {
                spawnTime = 0;

                SpawnObjects();

                spawnCounter++;
            }

            // Once all enemies have been spawned AND
            // all of them have finished the path,
            // start the wave transition period
            else if (spawnCounter >= currentWave.NumberOfEnemies &&
                     enemiesRemoved >= currentWave.NumberOfEnemies)
            {
                inBetween = true;
            }
        }
    }

    void SpawnObjects()
    {
        // Find the correct pool based on the enemy type
        if (_pooledObjects.TryGetValue(currentWave.Type, out var pool))
        {
            // Get an available enemy from the pool
            GameObject spawnedObject = pool.GetPooledObjects();

            // Spawn enemy at this object's position
            spawnedObject.transform.position = transform.position;

            // Activate enemy and start its behaviour
            spawnedObject.SetActive(true);
        }
    }

    void HandleEnemiesRemoved(EnemyData data)
    {
        // Keep track of how many enemies have completed the path
        enemiesRemoved++;
    }
}