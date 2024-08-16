using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Monster : MonoBehaviour
{
    public float Health
    {
        set
        {
            health = value;

        }

        get
        {
            return health;
        }
    }


    public RootRole rootRole = new RootRole();

    //選取對應的怪物index
    [SerializeField] int monsterIndex;

    protected bool isAlive = true;

    protected Animator animator;

    protected Rigidbody2D rb;

    protected Collider2D physicsCollider;
    protected DetectionZone detectionZone;
    protected float detectRange;


    float damage = 1f;

    float bodyRemainTime;
    float knockbackForce;
    float knockbackCooldown;
    float health;
    float attackRange;

    public float Damage { get { return damage; } protected set { damage = value; } }
    public float AttackRange { get { return attackRange; } }
    public float KnockbackForce { get { return knockbackForce; } }


    [SerializeField] BoxCollider2D attackBox;

    [SerializeField] bool isBoss;

    private void Awake()
    {
        Role monster = ReadJson.Instance.GetMonsterValue(monsterIndex);

        //賦值
        detectionZone = GetComponentInChildren<DetectionZone>();
        detectRange = GetComponentInChildren<CircleCollider2D>().radius;
        
        Health = monster.hp;
        Damage = monster.damage;
        knockbackForce = monster.knockback;
        knockbackCooldown = monster.knockbackCooldown;
        attackRange = monster.attackRange;
        detectRange = monster.detectionZone;
        detectionZone.MoveSpeed = monster.speed;
        bodyRemainTime = monster.bodyRemainTime;
    }


    private void Start()
    {
        //mystate = new RoleState();
        
        animator = GetComponent<Animator>();
        animator.SetBool("isAlive", isAlive);
        rb = GetComponentInChildren<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
        //detectionZone = GetComponentInChildren<DetectionZone>();

        
    }


    private void Defeated()
    {
      

        animator.SetBool("isAttack", false);
        SFXManager.instance.DeathSound();
        physicsCollider.enabled = false;
        rb.simulated = false;
        detectionZone.canApproach = false;

        if (isBoss)
        {
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.LevelFinished();
        }

        CancelInvoke("ReApproach");

        

        Invoke("DestroyEnemy", bodyRemainTime);

    }


    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        animator.SetTrigger("hit");
        detectionZone.canApproach = false;
        animator.SetBool("isMoving", false);
        rb.AddForce(knockback, ForceMode2D.Impulse);

        Invoke("ReApproach", knockbackCooldown);

        if (health <= 0)
        {
            health = 0;
            animator.SetBool("isAlive", false);
            Defeated();
        }

    }

    public void OnHit(float damage)
    {
        Health -= damage;
        animator.SetBool("isMoving", false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            Vector3 parentPos = transform.position;

            Vector2 direction = (Vector2)(collision.transform.position - parentPos).normalized;

            Vector2 knockback = direction * knockbackForce;

            //collision.SendMessage("OnHit", damage, knockback);
            playerHealth.OnHit(damage, knockback);

        }
    }


    //knockback冷卻過後 再次追擊玩家
    private void ReApproach()
    {
        detectionZone.canApproach = true;

    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void EnableAttack()
    {
        attackBox.enabled = true;
    }

    void DisableAttack()
    {
        attackBox.enabled = false;
    }

}
