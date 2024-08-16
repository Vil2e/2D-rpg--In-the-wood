using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public string target = "Player";
    Rigidbody2D rb;
    Vector2 direction;
    bool playerDetected;

    Vector3 targetPos;
    Animator animator;
    SpriteRenderer render;
    float moveSpeed = 1f;

    [SerializeField] bool attackType;//有勾選的才需要check attack range

    Monster monster;

  


    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }


    public bool canApproach = true;//用來處理knockback時的cool down

    Vector3 lTemp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        render = GetComponentInParent<SpriteRenderer>();
        monster = GetComponentInParent<Monster>();
        lTemp = monster.gameObject.GetComponent<Transform>().localScale;

    }


    // Update is called once per frame
    void FixedUpdate()
    {   //處理怪物的移動
        if (canApproach)//這個變數是處理怪被打到的情況 有冷卻時間才能再移動
        {
            if (playerDetected && targetPos != Vector3.zero)
            {
                direction = (targetPos - transform.position);
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

                animator.SetFloat("XInput", direction.x);
             
                animator.SetFloat("YInput", direction.y);
                animator.SetBool("isMoving", true);

                //往左往右 sprite的翻轉
                if (direction.x > 0)
                {
                    lTemp.x = 1;
                    //render.flipX = false;
                    monster.gameObject.GetComponent<Transform>().localScale = lTemp;

                }
                else if (direction.x < 0)
                {
                    lTemp.x = -1;
                    monster.gameObject.GetComponent<Transform>().localScale = lTemp;
                    //render.flipX = true;
                }

           
            }

        }


    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag(target))
        {
            float distance = Vector3.Distance(collision.transform.position, transform.position);

            playerDetected = true;
            targetPos = collision.transform.position;

            if(distance <= monster.AttackRange)
            {
                animator.SetBool("isAttack", true);
            }

            else if(distance > monster.AttackRange)
            {
                animator.SetBool("isAttack", false);
            }

        }
     
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
        {
            
            targetPos = Vector3.zero;
            playerDetected = false;
            direction = Vector2.zero;
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttack", false);
        }
    }

   
    
}
