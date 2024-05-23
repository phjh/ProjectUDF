using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapDefine
{
	#region Structs

	[Serializable]
	public struct MonsterInfo
	{
		public GameObject monsterObj;
		public Vector2 monsterPos;
	}

	#endregion

	#region Enums

	public enum Exit
	{
		Down = 0,
		Right,
		Up,
		Left
	}

	public enum MapEvents
	{
		FloorStart = 1,
		FloorClear = 2,
		MapStart = 3,
		MapClear = 4,
		WaveClear = 5,
		MonsterKill = 6
	}

	#endregion

	#region Classes

	public class PlacedTileData
	{
		public List<TileBase> PlacedTiles = new List<TileBase>();
		public List<Vector3Int> PlacedPoses = new List<Vector3Int>();
	}

	#endregion
}