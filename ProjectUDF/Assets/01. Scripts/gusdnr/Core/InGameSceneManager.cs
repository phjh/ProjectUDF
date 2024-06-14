using GameManageDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSceneManager : MonoSingleton<InGameSceneManager>
{

	#region Public Values

	public Scene ActiveScene() { return SceneManager.GetActiveScene(); }
	public int ActiveSceneIndex() { return ActiveScene().buildIndex; }
	public SceneList CurScene() { return (SceneList)ActiveSceneIndex(); }

	public bool CheckLoadCurrentScene(int index)
	{
		return ActiveScene() == SceneManager.GetSceneByBuildIndex(index);
	}

	public bool CheckLoadCurrentScene(string name)
	{
		return ActiveScene() == SceneManager.GetSceneByName(name);
	}
	#endregion

	private void OnEnable()
	{
		//SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		//SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void Awake()
	{
		
	}

	#region Load Scene

	#region Load Scene String

	public void SetSceneName(string sceneName)
	{
		if (ActiveScene().name == sceneName) return;
		StartCoroutine(TransitionScene(sceneName));
	}

	private IEnumerator TransitionScene(string sceneName)
	{
		Scene beforeScene = ActiveScene();
		yield return LoadSceneAsync(sceneName);
		yield return UnloadSceneAsync(beforeScene.buildIndex);
	}

	private IEnumerator LoadSceneAsync(string sceneName)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		asyncLoad.allowSceneActivation = false;

		while (!asyncLoad.isDone)
		{
			if (asyncLoad.progress >= 0.9f)
				asyncLoad.allowSceneActivation = true;

			Debug.Log($"Loading : [{asyncLoad.progress * 100}]%");
			yield return null;
		}

		SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
	}

	#endregion

	#region Load Scene Int

	public void SetSceneIndex(int index)
	{
		if (ActiveScene().buildIndex == index) return;
		StartCoroutine(TransitionScene(index));
	}

	private IEnumerator TransitionScene(int index)
	{
		Scene beforeScene = ActiveScene();
		yield return LoadSceneAsync(index);
		yield return UnloadSceneAsync(beforeScene.buildIndex);
	}

	private IEnumerator LoadSceneAsync(int index)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
		asyncLoad.allowSceneActivation = false;

		while (!asyncLoad.isDone)
		{
			if (asyncLoad.progress >= 0.9f)
				asyncLoad.allowSceneActivation = true;

			Debug.Log($"Loading : [{asyncLoad.progress * 100}]%");
			yield return null;
		}

		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
	}

	#endregion

	private IEnumerator UnloadSceneAsync(int index)
	{
		AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(index);
		while (!asyncUnload.isDone)
		{
			Debug.Log($"UnLoading : [{asyncUnload.progress * 100}]%");
			yield return null;
		}
		Debug.Log($"UnLoad Complete : {SceneManager.GetSceneByBuildIndex(index).name}");
	}

	#endregion

	#region Methods

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
	}

	#endregion
}
