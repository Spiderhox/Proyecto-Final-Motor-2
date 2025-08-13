
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

public class HazardDamageBehaviour : MonoBehaviour
{
    public static event Action OnJugadorMuerto;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //collision.gameObject.SetActive(false);
            //OnJugadorMuerto?.Invoke();
            collision.gameObject.GetComponent<TopdownPlayer>().OnJugadorMuerto();
        }

        if (collision.gameObject.CompareTag("Destructable"))
        {
            //Destroy(collision.gameObject, 0.2f);
            collision.gameObject.SetActive(false);
        }
    }
}

