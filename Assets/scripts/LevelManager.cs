using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
	public static LevelManager instance;
	public int current_Level = 0;
	int maxLevel = 2;
	public int MaxLevel { get { return maxLevel; } }

	public string current_role = "Role01";

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

	}

	private void Start()
	{
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			current_Level = 0;
		}
	}



}
