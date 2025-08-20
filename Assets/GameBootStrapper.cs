using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBootStrapper : MonoBehaviour
{
    public GameObject portalManagerPrefab;

    void Awake()
    {
        if (PortalManager.Instance == null)
        {
            Instantiate(portalManagerPrefab);
        }
    }

}
