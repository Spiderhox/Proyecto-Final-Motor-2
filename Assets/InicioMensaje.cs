using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicioMensaje : MonoBehaviour
{
    [SerializeField] private GameObject panelToHide;

    public void HidePanel()
    {
        panelToHide.SetActive(false);

    }

}
