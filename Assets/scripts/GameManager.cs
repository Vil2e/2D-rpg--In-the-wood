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
using UnityEngine.ResourceManagement.ResourceProviders;


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

	private void Start()
	{
		print(LevelManager.current_Level);
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
	}
	public void LoadNextScene()
	{
		transitionAnim.SetTrigger("End");
		StartCoroutine(LoadNextLevel("Level_0" + (LevelManager.current_Level + 1)));
		transitionAnim.SetTrigger("Start");

	}

	public void ClickStartGame()
	{
		SFXManager.instance.ClickSound();
		StartCoroutine(LoadNextLevel("Level_0" + (LevelManager.current_Level + 1)));

	}

	public void BackToMenu()//回到遊戲menu
	{
		SFXManager.instance.ClickSound();
		SceneManager.LoadScene("Level_00");
		LevelManager.current_Level = 0;
		Time.timeScale = 1;


	}

	IEnumerator LoadNextLevel(string level)
	{
		LevelManager.current_Level++;
		if (LevelManager.current_Level > LevelManager.maxLevel)
		{
			LevelManager.current_Level = LevelManager.maxLevel;
		}
		AsyncOperationHandle<SceneInstance> sceneInstance = Addressables.LoadSceneAsync(level);
		while (sceneInstance.Status != AsyncOperationStatus.Succeeded)
		{
			yield return null;
		}



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

	// 從pause menu回到遊戲
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
		int currentScene = LevelManager.current_Level;

		//寫入目前第幾關
		saveData.levelNumber = currentScene;

		//轉成json、存擋
		string jsonFile = JsonUtility.ToJson(saveData);
		File.WriteAllText(path + "/save.json", jsonFile);


	}

	public void Load()//讀檔功能
	{
		SFXManager.instance.ClickSound();

		int levelNum = ReadJson.Instance.GetSavedLevel();

		if (levelNum != 0)
		{
			StartCoroutine(LoadNextLevel("Level_0" + (LevelManager.current_Level + 1)));
		}
		else
		{
			print("You dont have any saved file");
		}
	}



}
