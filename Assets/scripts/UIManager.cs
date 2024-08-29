using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] GameObject stopMenu;
	[SerializeField] GameObject door;

	bool isGamePause = false;
	public bool IsGamePause
	{
		get { return isGamePause; }
	}

	private void Update()
	{
		if (GameManager.instance.IsGameStart && Input.GetKeyDown(KeyCode.M))
		{
			OpenStopMenu();
		}

	}

	public void OpenStopMenu()
	{
		stopMenu.SetActive(true);
		Time.timeScale = 0;
		isGamePause = true;

	}

	public void CloseStopMenu()
	{
		stopMenu.SetActive(false);
	}

	public void UnhideDoor()
	{
		door.SetActive(true);
	}

	public void Continue()
	{
		SFXManager.instance.ClickSound();
		isGamePause = false;
		Time.timeScale = 1;
		CloseStopMenu();
	}

	public void SaveButton()//存擋
	{
		GameManager.instance.Save();
	}

	public void LoadButton()//讀檔功能
	{
		GameManager.instance.Load();
	}

	public void BackToMenuButton()
	{
		GameManager.instance.BackToMenu();
	}

	public void ClickStartGameButton()
	{
		GameManager.instance.ClickStartGame();
	}

	public void QuitGameButton()
	{
		GameManager.instance.QuitGame();
	}
}
