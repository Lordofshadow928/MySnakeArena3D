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

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                PrepareObject(obj, position, rotation);
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
            pool.Add(newObj);

            PrepareObject(newObj, position, rotation);
            return newObj;
        }

        Debug.LogWarning("Pool empty and cannot expand!");
        return null;
    }

    private void PrepareObject(GameObject obj, Vector3 position, Quaternion rotation)
    {
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        // Reset physics BEFORE enabling
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        obj.SetActive(true);
    }

    public void ReturnObject(GameObject obj)
    {
        Destroy(obj);
        return;
        // Reset physics immediately
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

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