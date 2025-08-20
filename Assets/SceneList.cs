
using System.Collections.Generic;
using UnityEngine;


/*public class SceneList : ScriptableObject
{
    public List<SceneReference> scenes;
}

[System.Serializable]
public class SceneReference
{
    public string displayName;
    public Object sceneAsset;
    public int buildIndex;
}*/

[CreateAssetMenu(menuName = "Portals/Scene List")]
public class SceneList : ScriptableObject
{
    public List<SceneReference> scenes;

#if UNITY_EDITOR
    void OnValidate()
    {
        foreach (var scene in scenes)
            scene.Sync();
    }
#endif

}

[System.Serializable]
public class SceneReference
{

    public string displayName;

#if UNITY_EDITOR
    public UnityEditor.SceneAsset sceneAsset;
#endif

    public string sceneName; // Este se usa en el build
    public int buildIndex;

#if UNITY_EDITOR
    public void Sync()
    {
        if (sceneAsset != null)
            sceneName = sceneAsset.name;
    }
#endif
}
