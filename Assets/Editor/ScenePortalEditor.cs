#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(ScenePortal))]
public class ScenePortalEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ScenePortal portal = (ScenePortal)target;

        DrawDefaultInspector();

        if (portal.sceneList != null && portal.sceneList.scenes != null && portal.sceneList.scenes.Count > 0)
        {
            string[] options = portal.sceneList.scenes.Select(s => s.displayName + " (" + (s.sceneAsset != null ? s.sceneAsset.name : "NULL") + ")").ToArray();
            portal.targetSceneIndex = EditorGUILayout.Popup("Target Scene", portal.targetSceneIndex, options);
        }
        else
        {
            EditorGUILayout.HelpBox("Asigne una SceneList", MessageType.Warning);
        }
    }
}
#endif

