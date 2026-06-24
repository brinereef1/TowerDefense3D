using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // Enemy prefab that this pool manages
    [SerializeField] GameObject prefab;

    // Initial number of enemies to create
    [SerializeField] int poolSize;

    // Stores all created enemies
    [SerializeField] List<GameObject> pool;

    private void Start()
    {
        // Create the pool at game start
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObjects();
        }
    }

    private GameObject CreateNewObjects()
    {
        // Create a new enemy
        GameObject newPrefab = Instantiate(prefab, transform);

        // Keep it disabled until needed
        newPrefab.SetActive(false);

        // Store it in the pool
        pool.Add(newPrefab);

        return newPrefab;
    }

    public GameObject GetPooledObjects()
    {
        // Look for an inactive object that can be reused
        foreach (GameObject obj in pool)
        {
            if (!obj.activeSelf)
            {
                return obj;
            }
        }

        // If every object is busy, create a new one
        return CreateNewObjects();
    }
}