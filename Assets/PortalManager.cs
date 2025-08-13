using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public static PortalManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void RequestSceneChange(ScenePortal exitPortal, GameObject player)
    {
        string targetScene = exitPortal.GetTargetSceneName();
        int exitIndex = exitPortal.portalIndex;
        PortalDirection exitDir = exitPortal.exitDirection;

        StartCoroutine(ChangeSceneRoutine(targetScene, exitIndex, exitDir, player));
    }

    IEnumerator ChangeSceneRoutine(string nextScene, int exitIndex, PortalDirection exitDir, GameObject player)
    {
        string lastScene = SceneManager.GetActiveScene().name;

        var load = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        yield return load;

        // Wait until the scene is fully loaded
        Scene newLoadedScene = SceneManager.GetSceneByName(nextScene);
        Debug.Log("Loaded scene: " + newLoadedScene.name);
        Debug.Log("Is loaded: " + newLoadedScene.isLoaded);
        Debug.Log("Is valid: " + newLoadedScene.IsValid());

        bool success = SceneManager.SetActiveScene(newLoadedScene);
        Debug.Log("SetActiveScene success: " + success);
        while (!newLoadedScene.isLoaded)
            yield return null;

        // Now it's safe to set it active
        SceneManager.SetActiveScene(newLoadedScene);

        // Find the matching portal
        var portals = GameObject.FindObjectsOfType<ScenePortal>();
        var targetPortal = portals.FirstOrDefault(p =>
            p.entryDirection == OppositeDirection(exitDir) &&
            p.portalIndex == exitIndex &&
            p.gameObject.scene == newLoadedScene
        );

        if (targetPortal != null)
        {
            player.transform.position = targetPortal.spawnPoint.position;

            // Inform GameManager about the new spawn point
            GameManager.Instance.UpdateSpawnPoint(targetPortal.spawnPoint);
        }


        if (targetPortal != null)
            player.transform.position = targetPortal.spawnPoint.position;

        var unload = SceneManager.UnloadSceneAsync(lastScene);
        yield return unload;

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


