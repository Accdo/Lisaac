using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("구성")]
    [SerializeField] private float speed;
    [SerializeField] private float stepInterval = 0.5f;
    private float stepTimer;
    public Vector2 MoveDirection => moveDirection;

    private PlayerAnimation playerAnimation;
    private PlayerActions actions;
    private Rigidbody2D rb2D;

    private Vector2 moveDirection;

    void Awake()
    {
        actions = new PlayerActions();
        rb2D = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }



    void Update()
    {
        ReadMovement();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rb2D.MovePosition(rb2D.position + moveDirection * (speed * Time.fixedDeltaTime));
        if (moveDirection != Vector2.zero)
        {
            stepTimer -= Time.fixedDeltaTime;
            if (stepTimer <= 0f)
            {
                SoundManager.Instance.Step();
                stepTimer = stepInterval;
            }

        }
        else
        {
            stepTimer = 0f;
        }

    }

    void ReadMovement()
    {
        moveDirection = actions.Movement.Move.ReadValue<Vector2>().normalized; //moveDirection에 방향값을 가져옴
        if (moveDirection == Vector2.zero)
        {
            playerAnimation.SetMoveBoolTransition(false);
            return;
        }
        playerAnimation.SetMoveBoolTransition(true);
        playerAnimation.SetMoveAnimation(moveDirection);
    }

    void OnEnable()
    {
        actions.Enable();
    }

    void OnDisable()
    {
        actions.Disable();
    }
}
