using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // Prefab that will be reused by this object pool.
    [SerializeField] GameObject prefab;

    // Number of objects to create when the game starts.
    [SerializeField] int poolSize;

    // Stores every object created by this pool.
    [SerializeField] List<GameObject> pool;

    private void Start()
    {
        // Create the pool and pre-instantiate the initial objects.
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObjects();
        }
    }

    private GameObject CreateNewObjects()
    {
        // Create a new object as a child of this pool for hierarchy organization.
        GameObject newPrefab = Instantiate(prefab, transform);

        // Keep new objects inactive until they are needed.
        newPrefab.SetActive(false);

        // Add the object to the pool.
        pool.Add(newPrefab);

        return newPrefab;
    }

    public GameObject GetPooledObjects()
    {
        // Return the first inactive object available in the pool.
        foreach (GameObject obj in pool)
        {
            if (!obj.activeSelf)
            {
                return obj;
            }
        }

        // Expand the pool if every object is currently in use.
        return CreateNewObjects();
    }
}