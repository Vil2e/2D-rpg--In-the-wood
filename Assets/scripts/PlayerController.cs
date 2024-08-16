using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;

    public float collisionOffset = 0.02f;

    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    Vector2 movementInput;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

  
   
    void FixedUpdate()
    {
        //canMove是用來讓揮劍時 使玩家不能移動
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                //先做一般移動判定
                //如果不能通過的情況就改判定單一方向
                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0f));

                }

                if (!success)
                {
                    success = TryMove(new Vector2(0f, movementInput.y));

                }

                // set direction of sprite to movement direction


                animator.SetBool("isWalking", success);

            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }

        }
    }

    // trymove bool == true才會進行移動
    private bool TryMove(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            //確定是否有潛在的collision, 0表示沒有collision
            int count = rb.Cast(
                direction,// 介於-1與1的x、y值 用來判定是否有collsion的方向
                movementFilter,//決定哪裡會發生collision 或者會跟哪些layers collide
                castCollisions,//當cast結束後 用來儲存發生的collisions list
                moveSpeed * Time.fixedDeltaTime + collisionOffset);//

            if (count == 0)
            {
                //rb.AddForce(movementInput * moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
 
    }

    //處理move input
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();

        //這個if條件是讓input value維持在最後輸入的值
        //避免按左方向時面向左 放手後因為value是0跑回預設值而面向右
        //因為input沒有輸入值就會是0 當他value是0就會自動set float
        if(movementInput != Vector2.zero)
        {
            animator.SetFloat("XInput", movementInput.x);
            animator.SetFloat("YInput", movementInput.y);
        }

    }

    void OnFire()
    {
        SFXManager.instance.SlashSound();
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack()
    {
        LockMovement();

        if(animator.GetFloat("XInput") != 0)
        {
            if (spriteRenderer.flipX == true)
            {
                swordAttack.AttackLeft();
            }
            else
            {
                swordAttack.AttackRight();
            }

        }

        else if(animator.GetFloat("YInput") == 1)
        {
            swordAttack.AttackUp();
        }

        else if (animator.GetFloat("YInput") == -1)
        {
            swordAttack.AttackDown();

        }


    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

   


}
