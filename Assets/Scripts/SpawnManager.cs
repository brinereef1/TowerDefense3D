using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // All waves available in this level.
    [SerializeField] WaveData[] waves;

    // Index of the currently active wave.
    private int _currentWaveIndex = 0;

    // Timer used to control enemy spawn intervals.
    private float spawnTime;

    // Number of enemies spawned in the current wave.
    private int spawnCounter;

    // Number of enemies removed (killed or reached the goal) in the current wave.
    private int enemiesRemoved;

    // Indicates whether the game is waiting before starting the next wave.
    private bool inBetween;

    // Countdown timer between waves.
    private float wavePauseTimer;

    // Delay before the next wave starts.
    [SerializeField] float timeBetweenWaves;

    // Current wave number shown to the player.
    private int waveCounter = 1;

    // Object pools for each enemy type.
    [SerializeField] ObjectPooler spherePool;
    [SerializeField] ObjectPooler capsulePool;
    [SerializeField] ObjectPooler cylinderPool;

    // Maps each enemy type to its corresponding object pool.
    private Dictionary<EnemyType, ObjectPooler> _pooledObjects;

    // Shortcut to access the current wave.
    private WaveData currentWave => waves[_currentWaveIndex];

    // Notifies other systems whenever the wave changes.
    public static event Action<int> onWaveChanged;

    private void Awake()
    {
        wavePauseTimer = timeBetweenWaves;

        // Create a lookup table so enemies can be spawned by type.
        _pooledObjects = new Dictionary<EnemyType, ObjectPooler>()
        {
            { EnemyType.sphere, spherePool },
            { EnemyType.capsule, capsulePool },
            { EnemyType.cylinder, cylinderPool }
        };
    }

    private void Start()
    {
        // Display the first wave when the game starts.
        onWaveChanged?.Invoke(waveCounter);
    }

    private void OnEnable()
    {
        // Listen for enemies leaving the game (death or reaching the goal).
        Enemy.EnemyRemoved += HandleEnemyRemoved;
    }

    private void OnDisable()
    {
        // Stop listening when this object is disabled.
        Enemy.EnemyRemoved -= HandleEnemyRemoved;
    }

    private void Update()
    {
        if (inBetween)
        {
            // Count down the delay before the next wave.
            wavePauseTimer -= Time.deltaTime;

            if (wavePauseTimer <= 0f)
            {
                // Reset the wave timer.
                wavePauseTimer = timeBetweenWaves;

                // Move to the next wave and loop if we've reached the last one.
                _currentWaveIndex = (_currentWaveIndex + 1) % waves.Length;

                // Increase and broadcast the current wave number.
                waveCounter++;
                onWaveChanged?.Invoke(waveCounter);

                // Prepare counters for the new wave.
                spawnCounter = 0;
                enemiesRemoved = 0;

                // Resume spawning.
                inBetween = false;
            }
        }
        else
        {
            // Count elapsed time between enemy spawns.
            spawnTime += Time.deltaTime;

            // Spawn enemies until the wave limit is reached.
            if (spawnTime > currentWave.SpawnTime &&
                spawnCounter < currentWave.NumberOfEnemies)
            {
                spawnTime = 0;

                SpawnObjects();

                spawnCounter++;
            }

            // Wait for the next wave only after every spawned enemy has been removed.
            else if (spawnCounter >= currentWave.NumberOfEnemies &&
                     enemiesRemoved >= currentWave.NumberOfEnemies)
            {
                inBetween = true;
            }
        }
    }

    void SpawnObjects()
    {
        // Get the correct object pool for the current enemy type.
        if (_pooledObjects.TryGetValue(currentWave.Type, out var pool))
        {
            // Reuse an inactive enemy from the pool.
            GameObject spawnedObject = pool.GetPooledObjects();

            // Spawn the enemy at the spawn point.
            spawnedObject.transform.position = transform.position;

            // Return the enemy to the game.
            spawnedObject.SetActive(true);
        }
    }

    private void HandleEnemyRemoved(Enemy enemy)
    {
        // Count every enemy that leaves the game.
        enemiesRemoved++;
    }
}