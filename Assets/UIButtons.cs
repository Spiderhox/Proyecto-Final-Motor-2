using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void RestartGame() //esto esta en el game manager
    {
        GameManager.Instance.ResetGameState();
        SceneManager.LoadScene("Basement scene 1");
    }

}
