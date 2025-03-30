using System.Collections;
using TMPro;
using UnityEngine;

public class Apollo : MonoBehaviour
{

    // 보스 정보
    public float fireBallshotDelay = 2f;
    public float apolloMoveDelay = 5f;
    public float apolloMoveSpeed = 3f;
    public GameObject fireBall;
    public GameObject apolloMissile;
    public Transform firePoint;

    // 필요 정보
    private Transform playerLocation;
    private SpriteRenderer spriteRenderer;
    private Animator apolloAnimator;

    // 스테이터스
    private bool isShot;
    private bool isPhase2;
    private bool isFreeze;
    private int attType;
    private int normalAttCount;
    private float shootTimer;
    private float moveTimer;
    private Vector2 startPos;
    private Vector2 movePos;

    // 상위 관리자
    private GameObject exoMechManager;
    private ExoMech exoMechComponet;
    private EnemyHp exoMechHp;



    //아폴로
    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        exoMechManager = GameObject.FindWithTag("ExoMech");

        spriteRenderer = GetComponent<SpriteRenderer>();
        apolloAnimator = GetComponent<Animator>();
        exoMechComponet = exoMechManager.GetComponent<ExoMech>();
        exoMechHp = exoMechManager.GetComponent<EnemyHp>();

        shootTimer = fireBallshotDelay;
        moveTimer = apolloMoveDelay;

        isShot = false;
        isPhase2 = false;
        attType = 1;
        normalAttCount = 0;
        startPos = transform.position;
    }

    void Update()
    {
        if(!isFreeze)
        {
            PlayerGaze();

            moveTimer -= Time.deltaTime;

            if (moveTimer <= 0)
            {
                // 이동할 새로운 위치 설정 (현재 위치 기준 -3.5 ~ +3.5 랜덤 이동)
                float randomY = Random.Range(-3.5f, 3.5f);
                movePos = new Vector2(startPos.x, startPos.y + randomY);

                // 이동 시작
                StartCoroutine(MoveToPosition(movePos));

                // 타이머 초기화
                moveTimer = apolloMoveDelay;
            }

            if (!isShot)
            {
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0)
                {
                    if (attType == 1)
                    {
                        apolloAnimator.SetTrigger("FireballFire");
                        shootTimer = fireBallshotDelay;
                        isShot = true;
                        if (isPhase2)
                        {
                            if (normalAttCount >= 4)
                            {
                                attType = 2;
                                shootTimer = fireBallshotDelay * 2;
                            }
                        }
                    }
                    else if (attType == 2)
                    {
                        attType = 1;
                        apolloAnimator.SetTrigger("MissileFire");
                        shootTimer = fireBallshotDelay * 2;
                        isShot = true;
                    }
                }
            }
        }
    }

    void PlayerGaze()
    {
        Vector2 direction = (playerLocation.position - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        // 시선이 위쪽이여서 -90도를 하여 플레이어를 바라보게함
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void FireBallShot()
    {
        GameObject missile = Instantiate(fireBall, firePoint.position, firePoint.rotation);

        if (isPhase2)
        {
            normalAttCount++;
        }

        missile.GetComponent<ApolloFireBall>().SetDirection((playerLocation.position - firePoint.position).normalized);
    }

    void FireBallShotEnd()
    {
        isShot = false;
    }

    void FireMissile()
    {
        GameObject missile = Instantiate(apolloMissile, firePoint.position, firePoint.rotation);
    }

    void FireMissileEnd()
    {
        isShot = false;
        normalAttCount = 0;
    }

    private IEnumerator MoveToPosition(Vector2 targetPos)
    {
        while ((Vector2)transform.position != targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, apolloMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator PhaseChange()
    {
        isPhase2 = true;
        isShot = false;
        isFreeze = true;
        fireBallshotDelay = 0.5f;
        apolloAnimator.SetBool("IsPhase2", true);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if(!isFreeze)
            {
                PlayerBullet bullet = collision.GetComponent<PlayerBullet>();
                exoMechComponet.ExoMechsOnHit(bullet.damage);
            }
        }
    }

    private void PhaseChangeEnd()
    {
        isFreeze = false;
    }
}
