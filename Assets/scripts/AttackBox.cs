using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{

    Monster monster;


    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponentInParent<Monster>();

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            Vector3 parentPos = transform.position;

            Vector2 direction = (Vector2)(collision.transform.position - parentPos).normalized;

            Vector2 knockback = direction * monster.KnockbackForce;

            //collision.SendMessage("OnHit", damage, knockback);
            playerHealth.OnHit(monster.Damage, knockback);

        }
    }

}
