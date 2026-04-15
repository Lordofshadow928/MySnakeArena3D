using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 15;
    public bool expandPool = true;

    
    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        CreatePool();
    }
    void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        if (expandPool)
        {
            if (prefab == null)
            {
                Debug.LogError("Prefab is NULL in ObjectPool!");
                return null;
            }

            GameObject newObj = Instantiate(prefab, transform);
            newObj.SetActive(false);
            pool.Add(newObj);
            return newObj;
        }
        Debug.LogWarning("Pool empty and cannot expand!");
        return null;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public int ActiveCount()
    {
        int count = 0;
        foreach (var obj in pool)
        {
            if (obj.activeInHierarchy)
                count++;
        }
        return count;
    }
}
