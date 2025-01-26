using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGenerationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        if (GUILayout.Button("GenerateMap")) 
        {
            ((MapGenerator)target).GenerateMap();
        }

        if (GUILayout.Button("CleanMap"))
        {
            ((MapGenerator)target).CleanMap();
        }
    }
}
