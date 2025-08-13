using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnmanager : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPosition;
    public List<GameObject> pool = new List<GameObject>();
    public int spawnElementsAmount;

    public float spawnInterval = 2f;

    private float timer;

    void Start()
    {
        for (int i = 0; i < spawnElementsAmount; i++)
        {
            GameObject obj = Instantiate(ballPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            GameObject newObject = GetObjectFromPool();
            if (newObject != null)
            {
                newObject.transform.position = spawnPosition.position;
                newObject.transform.rotation = Quaternion.identity;
            }
            timer = 0;
        }
    }

    public GameObject GetObjectFromPool()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;

            }
        }
        return null;
    }
}
