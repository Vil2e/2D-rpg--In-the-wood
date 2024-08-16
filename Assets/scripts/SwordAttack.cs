using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float damage = 3;

    [SerializeField]BoxCollider2D SwordCollider;

    Vector3 horiColSize;
    Vector3 verColSize;

    //調整上下攻擊的hitbox offset
    Vector3 upAttackOffset;
    [SerializeField] Vector3 verOffset;
    [SerializeField] Vector3 horiOffset;
    [SerializeField] float verHitboxOffset = -0.13f;

    Vector3 rightAttackOffset;

    [SerializeField] float knockbackForce = 500f;

    Color damageColor;
    Color originCoor;
    SpriteRenderer enemyRender;





    // Start is called before the first frame update
    void Start()
    {
        ColorUtility.TryParseHtmlString("#FF814F", out damageColor);
        originCoor = Color.white;

        rightAttackOffset = horiOffset;
        upAttackOffset = verOffset;

        horiColSize = new Vector3(0.2f, 0.25f);
        verColSize = new Vector3(0.25f, 0.2f);

        SwordCollider = GetComponent<BoxCollider2D>();

    }

    
    public void AttackRight()
    {
        SwordCollider.size = horiColSize;
        transform.localPosition = rightAttackOffset;
        SwordCollider.enabled = true;
    }

    public void AttackLeft()
    {
        SwordCollider.size = horiColSize;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
        SwordCollider.enabled = true;
    }

    public void AttackUp()
    {
        SwordCollider.size = verColSize;
        transform.localPosition = upAttackOffset;
        SwordCollider.enabled = true;
    }

    public void AttackDown()
    {
        SwordCollider.size = verColSize;
        transform.localPosition = new Vector3(upAttackOffset.x, upAttackOffset.y + verHitboxOffset);
        SwordCollider.enabled = true;
    }

    public void StopAttack()
    {

        SwordCollider.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Monster monster = collision.GetComponent<Monster>();


        if (monster != null)
        {
            enemyRender = collision.gameObject.GetComponent<SpriteRenderer>();

            if (enemyRender != null)
            {
                SFXManager.instance.HitSound();
                DamageColor();
                Invoke("BackToOriginColor", 0.2f);

            }

            //計算敵人跟player間的方向
            Vector3 parentPos = transform.parent.position;

            Vector2 direction = (Vector2)( collision.transform.position - parentPos).normalized;

            Vector2 knockback = direction * knockbackForce;

            //collision.SendMessage("OnHit", damage, knockback);
            monster.OnHit(damage, knockback);

           

        }
     

    }

    void DamageColor()
    {
        enemyRender.color = damageColor;
    }

    void BackToOriginColor()
    {
        enemyRender.color = originCoor;

    }

}
