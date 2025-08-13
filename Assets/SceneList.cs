
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Portals/Scene List")]
public class SceneList : ScriptableObject
{
    public List<SceneReference> scenes;
}

[System.Serializable]
public class SceneReference
{
    public string displayName;
    public Object sceneAsset;
    public int buildIndex;
}
