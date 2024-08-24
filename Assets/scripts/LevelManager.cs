using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
	public static LevelManager instance;
	public static int current_Level = 0;
	public static int maxLevel = 3;

	void Awake()
	{
		if (instance = null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

	}



}
