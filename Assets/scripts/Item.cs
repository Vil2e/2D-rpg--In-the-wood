using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    string target = "Player";

    //道具數值
    public float damageIncreaseValue = 2;
    public float healthRecoverValue = 4;
    public float speedIncreaseValue = 0.5f;

    public float effectTime = 5f;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Collider2D col;

    [SerializeField] ItemType itemType;

    //指定道具種類
    private enum ItemType
    {
        attack,
        health,
        speed,

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
        {
            switch (itemType)
            {
                case ItemType.attack:

                    SwordAttack sword = collision.GetComponentInChildren<SwordAttack>();

                    if (sword != null)
                    {
                        sword.damage += damageIncreaseValue;
                        //加入效果持續時間
                        sprite.enabled = false;
                        col.enabled = false;
                        StartCoroutine(StopEffect(ItemType.attack));
                    }
                    break;

                case ItemType.speed:

                    PlayerController player = collision.GetComponent<PlayerController>();

                    if (player != null)
                    {
                        player.moveSpeed += speedIncreaseValue;
                        //加入效果持續時間
                        sprite.enabled = false;
                        col.enabled = false;
                        StartCoroutine(StopEffect(ItemType.speed));
                    }
                    break;

                case ItemType.health:

                    PlayerHealth playerHealth = collision.GetComponentInChildren<PlayerHealth>();

                    if (playerHealth != null)
                    {
                        playerHealth.Recover(healthRecoverValue);
                    }

                    Destroy(gameObject);
                    break;
            }

        }

        //停止buff效果
        void EffectStop(ItemType item)
        {
            switch (item)
            {
                case ItemType.attack:
                    SwordAttack sword = GameObject.FindObjectOfType<SwordAttack>();
                    sword.damage = 1;
                    break;

                case ItemType.speed:
                    PlayerController player = GameObject.FindObjectOfType<PlayerController>();
                    player.moveSpeed = 1;
                    break;
            }

        }

        IEnumerator StopEffect(ItemType item)
        {
            yield return new WaitForSeconds(effectTime);
            EffectStop(item);
            Destroy(gameObject);
        }

    }

}


