using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManageDefine
{
	#region Enums
	[System.Serializable]
	public enum SceneList
	{
		Start = 0,
		Lobby = 1,
		InGame = 2,
		Result = 3,
	}

	public enum GameStates
	{
		Lobby = 0,
		Start = 1,
		Playing = 2,
		NonPauseUIOn = 3,
		PauseUIOn = 4,
		End = 5,
	}

	public enum GameResults
	{
		None = -1,
		Playing = 0,
		TimeOut = 1,
		DiePlayer = 2,
		ClearGame = 3
	}

	#endregion

	[Serializable]
	public class GameResultData
	{
		public int ClearRoomCount { get; set; }
		public GameResults ResultState { get; set; }

		public List<int> CollectOres { get; set; }
		public List<int> CollectGems { get; set; }
	}

}
