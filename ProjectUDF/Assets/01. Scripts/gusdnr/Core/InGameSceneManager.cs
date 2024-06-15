using GameManageDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSceneManager : MonoSingleton<InGameSceneManager>
{

	#region Actions

	public static event Action OnStartLoadScene;
	public static event Action OnEndLoadScene;

	#endregion

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
		SceneManager.sceneLoaded += OnSceneLoaded;
		GameManager.OnEnd += () => SetSceneIndex((int)SceneList.Result);
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
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

		OnStartLoadScene?.Invoke();

		while (!asyncLoad.isDone)
		{
			if (asyncLoad.progress >= 0.9f)
				asyncLoad.allowSceneActivation = true;

			Debug.Log($"Loading : [{asyncLoad.progress * 100}]%");
			yield return null;
		}

		SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

		OnEndLoadScene?.Invoke();
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

		OnStartLoadScene?.Invoke();

		while (!asyncLoad.isDone)
		{
			if (asyncLoad.progress >= 0.9f)
				asyncLoad.allowSceneActivation = true;

			Debug.Log($"Loading : [{asyncLoad.progress * 100}]%");
			yield return null;
		}

		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));

		OnEndLoadScene?.Invoke();
	}

	#endregion

	#region Load Scene SceneList

	public void SetSceneSceneList(SceneList afterScene)
	{
		int SceneListToInt = (int)afterScene;
		if (ActiveScene().buildIndex == SceneListToInt) return;
		StartCoroutine(TransitionScene(SceneListToInt));
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
	public void QuitTheGame()
	{
		Application.Quit();
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("SceneLoad Complete : " + scene.name);
	}

	#endregion
}
