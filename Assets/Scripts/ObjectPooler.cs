using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int poolSize;
    [SerializeField] List<GameObject> pool;

    private void Start()
    {
        //Filling up pool list with poolsize amount of prefab  
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObjects();
        }
    }

    private GameObject CreateNewObjects()
    {
        //Spawning prefab, disabling those prefab, adding those prefab inside pool list and returning that spawned prefab
        GameObject newPrefab = Instantiate(prefab, transform);
        newPrefab.SetActive(false);
        pool.Add(newPrefab);
        return newPrefab;
    }

    public GameObject GetPooledObjects()
    {
        //Looking for gameobject which are disable inside pool list and returning that gameobject and if there isn't any, spawn a new prefab
        foreach (GameObject obj in pool)
        {
            if (!obj.activeSelf)
            {
                return obj;
            }
        }
        return CreateNewObjects();
    }
}
