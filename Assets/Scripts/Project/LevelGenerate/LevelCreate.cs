using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class LevelCreate : MonoBehaviour
{
   
}
#if UNITY_EDITOR
[CustomEditor(typeof(LevelGenerate))]
[System.Serializable]
class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGenerate level = (LevelGenerate)target;
        if (GUILayout.Button("Create Level"))
        {
            level.LevelCreate();
        }
        if (GUILayout.Button("Save Level"))
        {
            level.levelSave();
        }
        if (GUILayout.Button("Delete Level"))
        {
            level.LevelDelete();
        }
        if (GUILayout.Button("Level Remove"))
        {
            level.LevelRemove();
        }
        DrawDefaultInspector();
    }
}
#endif
