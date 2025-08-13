using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    private bool triggered = false;
    public GameObject glowEffect; // assign inspector

    void Update()
    {
        if (glowEffect != null)
        {
            glowEffect.SetActive(GameManager.Instance.CanPickUpBook());
        }

        if (GameManager.Instance.CanPickUpBook())
        {
            if (!glowEffect.activeSelf)
                glowEffect.SetActive(true);
        }
        else
        {
            if (glowEffect.activeSelf)
                glowEffect.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Player")) return;

        triggered = true;

        if (GameManager.Instance.CanPickUpBook())
        {
            GameManager.Instance.bookPickedUp = true;
            GameManager.Instance.gameWon = true;


            GameManager.Instance.ShowBookPopup();
            Destroy(gameObject); 
        }
        else
        {
            GameManager.Instance.gameFailed = true;

            GameManager.Instance.ShowNotEnoughPages();
            triggered = false; 
        }
    }


}
