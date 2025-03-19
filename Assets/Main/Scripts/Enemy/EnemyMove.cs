using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float chaseSpeed = 2f;
    public float wanderTime = 2f;
    public float chaseRange = 5f;
    public float wallCheckDistance = 0.5f;
    public LayerMask wallLayer;
    public LayerMask playerLayer;

    private Rigidbody2D rb2D;
    private Vector2 moveDirection;
    private Transform player;
    private Animator animator;
    private bool isChasing = false;
    private float wanderTimer;
    private EnemyAttack enemyAttack;



    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemyAttack = GetComponent<EnemyAttack>();
        moveDirection = Vector2.left;
        wanderTimer = wanderTime;
    }


    void Update()
    {
        if (player != null && !enemyAttack.isAttacking)
        {
            float distaceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distaceToPlayer < chaseRange)
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }
        }

        if (!isChasing)
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0f)
            {
                SetRandomDirection();
                wanderTimer = wanderTime;
            }
        }
        UpdateAnimation();
    }
    void FixedUpdate()
    {
        if (!enemyAttack.isAttacking)
            if (isChasing && player != null)
            {
                Vector2 chaseDirection = (player.position - transform.position).normalized;
                rb2D.linearVelocity = chaseDirection * chaseSpeed;
            }
            else
            {
                rb2D.linearVelocity = moveDirection * moveSpeed;

                if (WallDetected())
                {
                    SetRandomDirection();
                }
            }

    }

    private void SetRandomDirection()
    {
        moveDirection = Random.insideUnitCircle.normalized;
    }

    private bool WallDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, wallCheckDistance, wallLayer);
        return hit.collider != null;
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("MoveX", rb2D.linearVelocity.x);
        animator.SetFloat("MoveY", rb2D.linearVelocity.y);

    }


}
