
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

public class HazardDamageBehaviour : MonoBehaviour
{
    public static event Action OnJugadorMuerto;
    public AudioClip muerteJugadorClip;
    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //collision.gameObject.SetActive(false);
            //OnJugadorMuerto?.Invoke();
            if (muerteJugadorClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(muerteJugadorClip);
            }

            collision.gameObject.GetComponent<TopdownPlayer>().OnJugadorMuerto();
        }

        if (collision.gameObject.CompareTag("Destructable"))
        {
            //Destroy(collision.gameObject, 0.2f);
            collision.gameObject.SetActive(false);
        }
    }
}

