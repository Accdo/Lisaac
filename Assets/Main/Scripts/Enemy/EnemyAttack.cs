using UnityEngine;
using UnityEngine.Timeline;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float attackSpeed = 4f;
    public float attackDuration = 0.5f;
    public float attackCooldown = 2f;
    public LayerMask playerLayer;
    public LayerMask WallLayer;

    private Transform player;
    private Rigidbody2D rb2D;
    private Animator animator;
    public bool isAttacking = false;
    private Vector2 attackDirection;

    private float cooldownTimer;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }


    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (player != null && cooldownTimer <= 0f)
        {
            float distaceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distaceToPlayer <= attackRange && !isAttacking)
            {
                Attack();
            }
        }
        else if (!isAttacking) // 플레이어를 잃었을 때 공격 애니메이션 원상 복구
        {
            animator.SetBool("Attack", false);
        }
    }

    void FixedUpdate()
    {
        if (isAttacking)
        {
            rb2D.linearVelocity = attackDirection * attackSpeed;

            if (WallDetected())
            {
                StopAttack();
            }
        }
    }



    void Attack()
    {
        isAttacking = true;
        cooldownTimer = attackCooldown;
        if (player != null) // 플레이어가 있을 경우만 방향 설정
        {
            attackDirection = (player.position - transform.position).normalized;
        }
        else
        {
            attackDirection = Vector2.zero; // 플레이어가 없으면 공격 중단
            StopAttack();
            return;
        }

        animator.SetBool("Attack", true);
        UpdateAnimationDirection();
        Invoke("StopAttack", attackDuration);
    }
    void StopAttack()
    {
        isAttacking = false;
        rb2D.linearVelocity = Vector2.zero;
        animator.SetBool("Attack", false);
    }

    bool WallDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, 0.5f, WallLayer);
        return hit.collider != null;
    }
    void UpdateAnimationDirection()
    {
        animator.SetFloat("AttackX", attackDirection.x);
        animator.SetFloat("AttackY", attackDirection.y);
    }
}
