using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
	public static SFXManager instance;

	public AudioSource src;
	public AudioClip slashSound, hitSound, deathSound, playerDeathSound, playerHitSound, clickSound, doorAppearSound, stairsDown;


	private void Awake()
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

	public void SlashSound()
	{
		if (GameManager.instance.GetGamePauseStatus() == false)
		{
			src.PlayOneShot(slashSound);

		}
	}

	public void HitSound()
	{
		src.PlayOneShot(hitSound);
	}

	public void DeathSound()
	{
		src.PlayOneShot(deathSound);
	}

	public void PlayerDeath()
	{
		src.PlayOneShot(playerDeathSound);
	}

	public void PlayerHit()
	{
		src.PlayOneShot(playerHitSound);
	}

	public void DoorAppear()
	{
		src.PlayOneShot(doorAppearSound);
	}

	public void ClickSound()
	{
		src.PlayOneShot(clickSound);
	}

	public void StairsDown()
	{
		src.PlayOneShot(stairsDown);
	}

}
