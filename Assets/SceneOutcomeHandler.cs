using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneOutcomeHandler : MonoBehaviour
{
    [SerializeField] private GameObject welcomePanel;

    void Start()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.gameFailed)
        {
            GameManager.Instance.GameOverPanel?.SetActive(true);
            Debug.Log("fin del juego");
            HideWelcomePanel(); 
        }
        else if (GameManager.Instance.gameWon && GameManager.Instance.bookPickedUp)
        {
            GameManager.Instance.VictoryPanel?.SetActive(true);
            DisableEnemies();
            HideWelcomePanel(); 
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
