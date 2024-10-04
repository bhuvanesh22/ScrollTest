using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject itemPrefab;  
    public int poolSize = 50;      

    Queue<GameObject> pool = new();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject item = Instantiate(itemPrefab);
            item.SetActive(false);
            pool.Enqueue(item);
        }
    }

    public GameObject GetItem()
    {
        if (pool.Count > 0)
        {
            GameObject item = pool.Dequeue();
            item.SetActive(true);
            return item;
        }
        else
        {
            GameObject item = Instantiate(itemPrefab);
            return item;
        }
    }

    public void ReturnItem(GameObject item)
    {
        item.SetActive(false);
        pool.Enqueue(item);
    }
}
