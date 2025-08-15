using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOutcomeHandler : MonoBehaviour
{
    [SerializeField] private GameObject welcomePanel;


    [SerializeField] public AudioClip musicaVictoriaClip;
    [SerializeField] public AudioClip musicaGameOverClip;

    private AudioSource musicaSource;

    void Start()
    {
        musicaSource = gameObject.AddComponent<AudioSource>();
        musicaSource.playOnAwake = false;


        if (GameManager.Instance == null) return;

        if (GameManager.Instance.gameFailed)
        {
            GameManager.Instance.GameOverPanel?.SetActive(true);
            Debug.Log("fin del juego");
            HideWelcomePanel();

            if (musicaGameOverClip != null)
            {
                musicaSource.clip = musicaGameOverClip;
                musicaSource.Play();
            }


        }

        else if (GameManager.Instance.gameWon && GameManager.Instance.bookPickedUp)
        {
            GameManager.Instance.VictoryPanel?.SetActive(true);
            DisableEnemies();
            HideWelcomePanel();

            if (musicaVictoriaClip != null)
            {
                musicaSource.clip = musicaVictoriaClip;
                musicaSource.Play();
            }

        }
    }

    void HideWelcomePanel()
    {
        if (welcomePanel != null)
        {
            welcomePanel.SetActive(false);
            Debug.Log("inicio mensaje oculto");
        }
    }

    void DisableEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                enemy.SetActive(false);
        }
    }

}
