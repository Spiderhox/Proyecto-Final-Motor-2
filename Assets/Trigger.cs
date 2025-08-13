
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public ScenePortal portalData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PortalManager.Instance.RequestSceneChange(
                portalData,
                other.gameObject
            );
        }
    }
}
