using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using System;


public class GameManager : MonoBehaviour
{
	[SerializeField] bool isGameStart;//避免在menu畫面生成player
	[SerializeField] bool isGamePause;//如果game pause,設定playerControl無反應
	public bool IsGamePause { get { return isGamePause; } }

	GameObject player;
	[SerializeField] GameObject door;

	[SerializeField] Animator transitionAnim;
	[SerializeField] GameObject stopMenu;

	[SerializeField] AssetReferenceGameObject playerReference;
	public static event Action<GameObject> OnPlayerSpawned;


	string path = Application.dataPath + "/streamingAssets";



	void Awake()
	{
		AsyncOperationHandle<GameObject> asyncOperationHandle = playerReference.LoadAssetAsync<GameObject>();
		asyncOperationHandle.Completed += AsyncOperationHandle_Completed;


	}

	private void Update()
	{
		if (isGameStart && Input.GetKeyDown(KeyCode.M))
		{
			isGamePause = true;
			stopMenu.SetActive(true);
			Time.timeScale = 0;

		}

	}

	void AsyncOperationHandle_Completed(AsyncOperationHandle<GameObject> asyncOperationHandle)
	{   //確認載入成功 -> 生成物件, 否則印出錯誤訊息
		if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded && isGameStart)
		{
			player = Instantiate(asyncOperationHandle.Result);
			OnPlayerSpawned?.Invoke(player);
		}
		else
		{
			Debug.Log("Loading Asset failed!");
		}
	}
	public void LoadNextScene()
	{
		int totalSceneAmount = SceneManager.sceneCountInBuildSettings;
		int currentScene = SceneManager.GetActiveScene().buildIndex;
		int nextScene = currentScene + 1;

		if (nextScene > totalSceneAmount - 1) { return; }

		transitionAnim.SetTrigger("End");
		SceneManager.LoadScene(nextScene);
		transitionAnim.SetTrigger("Start");

	}

	public void ClickStartGame()
	{
		SFXManager.instance.ClickSound();
		SceneManager.LoadScene(1);
	}

	public void BackToMenu()//回到遊戲
	{
		SFXManager.instance.ClickSound();
		SceneManager.LoadScene(0);
		Time.timeScale = 1;

	}

	public void QuitGame()
	{
		SFXManager.instance.ClickSound();
		Application.Quit();

	}

	//解除原本隱藏在關卡的門
	public void LevelFinished()
	{
		if (door != null)
		{
			SFXManager.instance.DoorAppear();
			door.SetActive(true);
		}

	}

	public void Continue()
	{
		SFXManager.instance.ClickSound();
		isGamePause = false;
		Time.timeScale = 1;
		stopMenu.SetActive(false);

	}

	public void Save()//存擋
	{
		SFXManager.instance.ClickSound();
		SaveData saveData = new SaveData();
		int currentScene = SceneManager.GetActiveScene().buildIndex;

		//寫入目前第幾關
		saveData.levelNumber = currentScene;

		//轉成json、存擋
		string jsonFile = JsonUtility.ToJson(saveData);
		File.WriteAllText(path + "/save.json", jsonFile);


	}

	public void Load()//讀檔功能
	{
		SFXManager.instance.ClickSound();

		int level = ReadJson.Instance.GetSavedLevel();

		if (level != 0)
		{
			SceneManager.LoadScene(level);
		}
		else
		{
			print("You dont have any saved file");
		}
	}



}
