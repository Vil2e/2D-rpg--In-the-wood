using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeartBar : MonoBehaviour
{
	[SerializeField] GameObject heartPrefab;
	PlayerHealth playerHealth;
	List<HealthHeart> hearts = new List<HealthHeart>();

	[SerializeField] float playerMaxHealth = 12f;


	private void OnEnable()
	{

		GameManager.OnPlayerSpawned += OnPlayerSpawned;
		PlayerHealth.OnhealthUpdate += DrawHearts;
	}

	private void OnDisable()
	{
		GameManager.OnPlayerSpawned -= OnPlayerSpawned;
		PlayerHealth.OnhealthUpdate -= DrawHearts;

	}

	private void OnPlayerSpawned(GameObject player)
	{   // playerHealth在hurtbox上
		playerHealth = player.GetComponentInChildren<PlayerHealth>();
		DrawHearts();
	}

	public void CreateEmptyHeart()
	{
		GameObject newHeart = Instantiate(heartPrefab, transform);

		//生成heart實體(game obj跟image處理)
		HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
		heartComponent.SetHeartImage(HealthHeart.HeartStatus.Empty);
		//將生成的heart放入list管理
		hearts.Add(heartComponent);
	}

	public void DrawHearts()
	{
		//如果有health = 12 -> 需要畫出 12/4 = 3顆心
		ClearHearts();

		//決定要畫幾顆空白心
		//由max health決定
		float maxHealthRemainder = playerMaxHealth % 4;
		int heartsToMake = (int)(playerMaxHealth / 4 + maxHealthRemainder);

		for (int i = 0; i < heartsToMake; i++)
		{
			CreateEmptyHeart();
		}

		//更新每一個心的sprite
		for (int i = 0; i < heartsToMake; i++)
		{
			//這裡的運算是把當下體力 - 換算一顆心的體力點,然後加入clamp的閥值
			int heartStatusRemainder = (int)Mathf.Clamp(playerHealth.Health - (i * 4), 0, 4);
			hearts[i].SetHeartImage((HealthHeart.HeartStatus)heartStatusRemainder);
		}

	}

	public void ClearHearts()//清除health bar下的所有子物件
	{
		foreach (Transform t in transform)
		{
			Destroy(t.gameObject);
		}

		hearts = new List<HealthHeart>();
	}
}
