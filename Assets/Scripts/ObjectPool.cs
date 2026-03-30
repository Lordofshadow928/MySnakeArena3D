using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 15;
    public bool expandPool = true;


    private List<GameObject> pool = new List<GameObject>();

    private void Start()
    {
        CreatePool();
    }
    void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            return obj;
        }

        if (expandPool)
        {
            GameObject newObj = Instantiate(prefab);
            newObj.SetActive(false);
            pool.Add(newObj);
            return newObj;
        }

        return null;
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
