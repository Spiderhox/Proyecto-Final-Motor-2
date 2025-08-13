using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HojasCuaderno : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) // go to game manager girl
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddToCount(1);
            Destroy(gameObject);
        }
    }
}
