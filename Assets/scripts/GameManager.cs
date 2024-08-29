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
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
	[SerializeField] bool isGameStart;
	public bool IsGameStart
	{
		get { return isGameStart; }
	}
	GameObject player;

	[SerializeField] Animator transitionAnim;
	string current_role;

	public static event Action<GameObject> OnPlayerSpawned;
	AsyncOperationHandle<GameObject> asyncOperationHandle;
	AsyncOperationHandle<SceneInstance> sceneInstance;


	string path = Application.dataPath + "/streamingAssets";

	public static GameManager instance;
	public UIManager uiMgr;

	void Awake()
	{
		// 保持 GameManager 在場景切換後不被銷毀
		DontDestroyOnLoad(this.gameObject);


		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		// 訂閱場景卸載事件
		SceneManager.sceneUnloaded += OnSceneUnloaded;

	}

	// 釋放內存
	private void OnSceneUnloaded(Scene current)
	{
		if (asyncOperationHandle.IsValid())
		{
			// 釋放player
			Addressables.Release(asyncOperationHandle);
			// 釋放場景
			Addressables.Release(sceneInstance);

			// Debug.Log("資源已釋放");
		}
	}

	// 解除訂閱
	private void OnDisable()
	{
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}

	// 用來觀測異步下載進度
	private IEnumerator ShowLoadingProgress(AsyncOperationHandle<GameObject> handle)
	{
		while (!handle.IsDone)
		{
			// 印出目前進度
			// Debug.Log($"Loading Progress: {handle.PercentComplete * 100}%");

			// 等待下一幀再檢查進度
			yield return null;
		}

		// 載入完成後印出100%
		// Debug.Log("Loading Progress: 100%");
	}

	// 換關(index num會在換關時遞增)
	public void LoadNextScene()
	{
		transitionAnim.SetTrigger("End");
		// Debug.Log("LoadNextLevel is being called.");
		StartCoroutine(LoadLevelAsset("Level_0" + (LevelManager.instance.current_Level + 1)));
		transitionAnim.SetTrigger("Start");

	}

	// 異步載入要切換的level asset
	IEnumerator LoadLevelAsset(string level)
	{
		LevelManager.instance.current_Level++;
		if (LevelManager.instance.current_Level > LevelManager.instance.MaxLevel)
		{
			LevelManager.instance.current_Level = 0;
			level = "Level_0" + LevelManager.instance.current_Level;
		}

		sceneInstance = Addressables.LoadSceneAsync(level, LoadSceneMode.Single, false);
		yield return sceneInstance; // 等待sceneInstance載入完成 再繼續下一步

		sceneInstance.Result.ActivateAsync().completed += (operation) =>
		{
			// Debug.Log("場景載入完成：" + level);

			// 在這裡載入玩家角色
			current_role = LevelManager.instance.current_role;
			LoadRole(current_role);
			uiMgr = FindObjectOfType<UIManager>();

		};
	}

	// 異步載入player Asset並生成
	void LoadRole(string role)
	{
		// print("current role: " + current_role);
		//確認載入成功 -> 生成物件, 否則印出錯誤訊息
		asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(role);
		StartCoroutine(ShowLoadingProgress(asyncOperationHandle));
		asyncOperationHandle.Completed += (handle) =>
		{
			if (handle.Status == AsyncOperationStatus.Succeeded && isGameStart)
			{
				// print("instantiate player");
				player = Instantiate(handle.Result);
				// 檢查OnPlayerSpawned是否有訂閱, 有則丟入player作為參數 null則return
				OnPlayerSpawned?.Invoke(player);

			}
			else
			{
				Debug.Log("角色載入失敗");
			}
		};

	}

	// 給sart game button用的func
	public void ClickStartGame()
	{
		SFXManager.instance.ClickSound();
		isGameStart = true;
		LoadNextScene();

	}

	//回到遊戲menu
	public void BackToMenu()
	{
		SFXManager.instance.ClickSound();
		SceneManager.LoadScene("Level_00");
		LevelManager.instance.current_Level = 0;
		Time.timeScale = 1;
		isGameStart = false;

	}

	public void QuitGame()
	{
		SFXManager.instance.ClickSound();
		Application.Quit();

	}

	// 解除原本隱藏在關卡的門
	public void LevelFinished()
	{
		SFXManager.instance.DoorAppear();
		uiMgr.UnhideDoor();
	}

	// 存擋
	public void Save()
	{
		SFXManager.instance.ClickSound();
		SaveData saveData = new SaveData();
		int currentScene = LevelManager.instance.current_Level;
		string selectedRole = LevelManager.instance.current_role;

		//寫入目前第幾關
		saveData.levelNumber = currentScene;

		//寫入選擇角色
		saveData.selectedRole = selectedRole;

		//轉成json、存擋
		string jsonFile = JsonUtility.ToJson(saveData);
		File.WriteAllText(path + "/save.json", jsonFile);

	}

	// 讀檔功能
	public void Load()
	{
		SFXManager.instance.ClickSound();

		var data = ReadJson.Instance.GetSavedData();
		int levelNum = data.Item1;
		string role = data.Item2;
		LevelManager.instance.current_role = role;
		isGameStart = true;


		if (levelNum != 0)
		{
			StartCoroutine(LoadLevelAsset("Level_0" + levelNum));
			LevelManager.instance.current_Level = levelNum;
		}
		else
		{
			print("You dont have any saved file");
		}
	}

	// 確認isGamePause status
	public bool GetGamePauseStatus()
	{
		return uiMgr.IsGamePause;
	}


}