using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapEditor))]
public class MapEditorUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

		MapEditor mapEditor = (MapEditor)target;
        if (GUILayout.Button("Create Map"))
        {
            mapEditor.CreateDefaultMap();
        }
		if (GUILayout.Button("Save Map"))
		{
			mapEditor.SaveMap();
		}
		if (GUILayout.Button("Load Map"))
		{
			mapEditor.LoadMap();
		}
		if (GUILayout.Button("Reset Map"))
		{
			mapEditor.ResetMap();
		}
	}
}