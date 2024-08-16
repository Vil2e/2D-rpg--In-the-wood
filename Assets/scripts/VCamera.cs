using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamera : MonoBehaviour
{
	CinemachineVirtualCamera vcam;
	Transform targetToFollow;

	private void Awake()
	{
		vcam = GetComponent<CinemachineVirtualCamera>();
	}

	private void OnEnable()
	{
		GameManager.OnPlayerSpawned += OnPlayerSpawned;

	}

	private void OnDisable()
	{
		GameManager.OnPlayerSpawned -= OnPlayerSpawned;
	}

	private void OnPlayerSpawned(GameObject player)
	{
		targetToFollow = player.GetComponent<PlayerController>().transform;

		if (targetToFollow != null)
		{
			vcam.Follow = targetToFollow;
		}
		else
		{
			Debug.Log("Player controller not found on the spawned player");
		}

	}


}
