using System.Collections;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public static PortalManager Instance;

    /*private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("PortalManager marcado como DontDestroyOnLoad");

        }

        
        else Destroy(gameObject);
         Debug.Log("PortalManager duplicado, se destruye");

    }*/

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("PortalManager duplicado, se destruye");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private GameObject cachedPlayer;
    private ScenePortal currentPortal;

    public void RequestSceneChange(ScenePortal portal, GameObject player)
    {
        if (portal == null)
        {
            Debug.LogError("El portal es null.");
            return;
        }

        if (player == null || player.Equals(null))
        {
            Debug.LogError("El jugador recibido es null o ha sido destruido.");
            return;
        }

        cachedPlayer = player;
        currentPortal = portal;
        StartCoroutine(ChangeSceneRoutine(
        portal.GetTargetSceneName(),
        portal.portalIndex,
        portal.exitDirection,
        TopdownPlayer.instance?.gameObject
        ));

    }

    IEnumerator ChangeSceneRoutine(string nextScene, int exitIndex, PortalDirection exitDir, GameObject player)
    {

        string sceneName = currentPortal.GetTargetSceneName();

        string lastScene = SceneManager.GetActiveScene().name;

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Nombre de escena inválido.");
            yield break;
        }

        // Validar que el spawnPoint aún existe
        if (currentPortal.spawnPoint == null || currentPortal.spawnPoint.Equals(null))
        {
            Debug.LogError("El spawnPoint del portal actual ha sido destruido o es null.");
            yield break;
        }

        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return null;

        Scene newLoadedScene = SceneManager.GetSceneByName(sceneName);
        if (!newLoadedScene.IsValid() || !newLoadedScene.isLoaded)
        {
            Debug.LogError($"La escena '{sceneName}' no se cargó correctamente.");
            yield break;
        } 
        
        TopdownPlayer.instance.transform.position = currentPortal.spawnPoint.position;

        SceneManager.SetActiveScene(newLoadedScene);

        
        if (cachedPlayer == null || player.Equals(null))

        {
            Debug.LogError("El jugador es null justo antes de moverlo a la nueva escena.");
            Debug.Log("Jugador cacheado: " + cachedPlayer.name);
            yield break;
        }

        if (currentPortal == null)
        {
            Debug.Log("el portal actual no existe");
        }


        SceneManager.MoveGameObjectToScene(cachedPlayer, newLoadedScene);
        Debug.Log("Jugador movido correctamente a: " + sceneName);

        // Esperar
        int waitFrames = 0;
        while (GameObject.FindObjectsOfType<ScenePortal>().Length == 0 && waitFrames < 300)
        {
            yield return null;
            waitFrames++;
        }

        if (waitFrames >= 300)
        {
            Debug.LogError("Timeout esperando portales en la nueva escena.");
            yield break;
        }

        // Buscar el portal destino
        var portals = GameObject.FindObjectsOfType<ScenePortal>();
        var targetPortal = portals.FirstOrDefault(p =>
            p.entryDirection == OppositeDirection(currentPortal.exitDirection) &&
            p.portalIndex == currentPortal.portalIndex &&
            p.gameObject.scene == newLoadedScene
        );

        if (targetPortal == null)
        {
            Debug.LogError("No se encontró el portal destino en la nueva escena.");
            yield break;
        }

        if (targetPortal.spawnPoint == null)
        {
            Debug.LogError("El portal destino no tiene asignado un spawnPoint.");
            yield break;
        }

        if (cachedPlayer == null)
            Debug.LogError("cachedPlayer es null");

        else if (cachedPlayer.scene.name == null)
            Debug.LogError("cachedPlayer apunta a un objeto destruido");

        else
            Debug.Log("cachedPlayer está vivo en escena: " + cachedPlayer.scene.name);

        // Teletransportar jugador
        cachedPlayer.transform.position = targetPortal.spawnPoint.position;

        // Decirle al GM
        GameManager.Instance.UpdateSpawnPoint(targetPortal.spawnPoint);
        GameManager.Instance.MarkPlayerPositioned();

        Debug.Log($"Cambio de escena completado. Escena activa: {SceneManager.GetActiveScene().name}");
        Debug.Log($"La escena anterior '{lastScene}' fue descargada automáticamente al cargar '{sceneName}'.");
    }

    PortalDirection OppositeDirection(PortalDirection dir)
    {
        switch (dir)
        {
            case PortalDirection.Left: return PortalDirection.Right;
            case PortalDirection.Right: return PortalDirection.Left;
            case PortalDirection.Up: return PortalDirection.Down;
            case PortalDirection.Down: return PortalDirection.Up;
            default: return dir;
        }
    }
}


