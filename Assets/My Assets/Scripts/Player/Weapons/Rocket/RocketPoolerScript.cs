using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RocketPoolerScript
{

    public static RocketPoolerScript Instance;
    private List<GameObject> objectPool;
    private GameObject prefab;
    private int size;
    public RocketPoolerScript instance
    {
        get
        {
            return Instance;
        }
    }

    public RocketPoolerScript(GameObject _prefab, int _size)
    {
        if (Instance != null)
            return;

        Instance = this;
        objectPool = new List<GameObject>();
        prefab = _prefab;
        size = _size;

        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < size; i++)
        {
            GameObject temp = GameObject.Instantiate(prefab);
            temp.SetActive(false);
            objectPool.Add(temp);
        }
    }

    public GameObject GetObjectFromPool()
    {
        for (int i = 0; i < size; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                objectPool[i].SetActive(true);
                return objectPool[i];
            }
        }
        
        return null;
    }

    public void ReturnObjectToPool(GameObject _activeObj)
    {
        _activeObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _activeObj.GetComponent<RocketMovementScript>().target = null;
        _activeObj.SetActive(false);

    }




}
