using System.Collections;
using UnityEngine;

public class Artemis : MonoBehaviour
{
    // 보스 정보
    public float laserShotDelay = 1f;
    public float artemisMoveDelay = 2f;
    public float artemisMoveSpeed = 3f;
    public GameObject artemisLaser;
    public Transform firePoint;

    // 필요 정보
    private Transform playerLocation;
    private SpriteRenderer spriteRenderer;
    private Animator artemisAnimator;

    // 스테이터스
    private bool isShot;
    private bool isPhase2;
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

    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        exoMechManager = GameObject.FindWithTag("ExoMech");

        spriteRenderer = GetComponent<SpriteRenderer>();
        artemisAnimator = GetComponent<Animator>();
        exoMechComponet = exoMechManager.GetComponent<ExoMech>();
        exoMechHp = exoMechManager.GetComponent<EnemyHp>();

        shootTimer = laserShotDelay;
        moveTimer = artemisMoveDelay;

        isShot = false;
        isPhase2 = false;
        attType = 1;
        normalAttCount = 0;
        startPos = transform.position;
    }

    void Update()
    {
        Vector2 direction = (playerLocation.position - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        // 시선이 위쪽이여서 -90도를 하여 플레이어를 바라보게함
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            // 이동할 새로운 위치 설정 (현재 위치 기준 -3.5 ~ +3.5 랜덤 이동)
            float randomY = Random.Range(-3.5f, 3.5f);
            movePos = new Vector2(startPos.x, startPos.y + randomY);

            // 이동 시작
            StartCoroutine(MoveToPosition(movePos));

            // 타이머 초기화
            moveTimer = artemisMoveDelay;
        }
    }

    private IEnumerator MoveToPosition(Vector2 targetPos)
    {
        while ((Vector2)transform.position != targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, artemisMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void PhaseChange()
    {
        artemisAnimator.SetBool("IsPhase2", true);
        isPhase2 = true;
        isShot = false;
        laserShotDelay = 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            PlayerBullet bullet = collision.GetComponent<PlayerBullet>();
            exoMechComponet.ExoMechsOnHit(bullet.damage);

            // Debug.Log(exoMechHp.currentHp);

            /*if (exoMechHp.currentHp <= exoMechHp.maxHp / 2 && !isPhase2)
            {
                artemisAnimator.SetBool("IsPhase2", true);
                isPhase2 = true;
                isShot = false;
                laserShotDelay = 0.5f;
            }*/
        }
    }
}
