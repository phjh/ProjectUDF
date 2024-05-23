#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;



#if UNITY_EDITOR

[CustomEditor(typeof(MapEditor))]
public class MapEditor : MonoBehaviour
{
	[Serializable]
	public struct MapNumber
	{
		public int FloorNumber;
		public int RooomNumber;
	}
	[Header("Base Prefab")]
	public GameObject MapBase;
	[Header("Info")]
	public MapNumber number;

	private GameObject EditingMap;
	private string MapObjectName;

	[ContextMenu("Create_Map")]
	public void CreateDefaultMap()
	{
		if (EditingMap != null)
		{

			DestroyImmediate(EditingMap);

		}

		#region ����Ʈ �� ����

		MapObjectName = $"{number.FloorNumber}F{number.RooomNumber}R";
		EditingMap = Instantiate(MapBase);
		EditingMap.transform.position = Vector3.zero;
		EditingMap.transform.rotation = Quaternion.Euler(Vector3.zero);
		EditingMap.name = MapObjectName;
		#endregion
	}

	[ContextMenu("Save_Map")]
	public void SaveMap()
	{
		if (EditingMap == null) return;

		PrefabUtility.SaveAsPrefabAsset(EditingMap,
			Application.dataPath + $"/Resources/Map/{number.FloorNumber}/{MapObjectName}.prefab");
	}

	[ContextMenu("Load_Map")]
	public void LoadMap()
	{
		if (EditingMap != null)
		{
			DestroyImmediate(EditingMap);
		}

		MapObjectName = $"{number.FloorNumber}F{number.RooomNumber}R";
		EditingMap = Resources.Load<GameObject>($"MapMap/{number.FloorNumber}/{MapObjectName}");
		if(EditingMap != null)
		{
			Debug.Log("Complete to Load");
			EditingMap = Instantiate(EditingMap);
			EditingMap.transform.name = MapObjectName;
		}
		else if(EditingMap == null)
		{
			Debug.Log("Can't find Object / Craete New Map");
			CreateDefaultMap();
		}
	}

	[ContextMenu("Reset_Map")]
	public void ResetMap()
	{
		Tilemap[] tilemap = EditingMap.GetComponentsInChildren<Tilemap>();
		foreach(var tm in tilemap)
		{
			tm.ClearAllTiles();
		}
	}
}

#endif