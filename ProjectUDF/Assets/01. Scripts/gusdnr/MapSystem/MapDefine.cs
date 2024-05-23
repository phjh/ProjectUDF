using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapDefine
{
	public struct MonsterInfo
	{
		public GameObject monsterObj;
		public Vector2 monsterPos;
	}

	public enum Exit
	{
		Down = 0,
		Right,
		Up,
		Left
	}

	public class PlacedTileData
	{
		public List<TileBase> PlacedTiles = new List<TileBase>();
		public List<Vector3Int> PlacedPoses = new List<Vector3Int>();
	}
}