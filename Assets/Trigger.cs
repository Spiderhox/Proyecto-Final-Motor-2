
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public ScenePortal portalData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger activado por: " + other.name);

        if (other.CompareTag("Player") && PortalManager.IsBusy == false)
        {
            Debug.Log("Jugador detectado, intentando cambiar de escena...");

            Debug.Log("PortalManager is null: " + (PortalManager.Instance == null));
            PortalManager.Instance.RequestSceneChange(portalData, TopdownPlayer.instance.gameObject);
        }
    }
}
