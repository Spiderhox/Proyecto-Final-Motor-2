using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortalDirection { Left, Right, Up, Down }

public class ScenePortal : MonoBehaviour
{
    public SceneList sceneList;
    public int targetSceneIndex = 0;
    public PortalDirection entryDirection;
    public PortalDirection exitDirection;
    public int portalIndex;
   
    public Transform spawnPoint;

    public string GetTargetSceneName()
    {
        if (sceneList == null || sceneList.scenes == null || sceneList.scenes.Count <= targetSceneIndex)
            return null;
        return sceneList.scenes[targetSceneIndex].sceneName; ;
    }
}

