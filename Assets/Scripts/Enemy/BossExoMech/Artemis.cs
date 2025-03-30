using System.Collections;
using UnityEngine;

public class Artemis : MonoBehaviour
{
    // 보스 정보
    public float laserShotDelay = 1f;    
    public float artemisMoveSpeed = 7f;
    public float laserShotDelayPhase2 = 0.5f;
    public float artemisMoveSpeedPhase2 = 10f;
    public GameObject artemisLaser;
    public Transform firePoint;

    // 필요 정보
    private Transform playerLocation;
    private SpriteRenderer spriteRenderer;
    private Animator artemisAnimator;
    private LineRenderer[] laserTrajectories;
    private int trajectoryCount = 5;

    // 스테이터스
    private bool isShot;
    private bool isPhase2;
    private bool isFreeze;
    private int attType;
    private int normalAttCount;
    private float shootTimer;
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

        isShot = false;
        isPhase2 = false;
        isFreeze = false;
        attType = 1;
        normalAttCount = 0;
        startPos = transform.position;

        // 레이저 발사 궤적 5개 생성
        laserTrajectories = new LineRenderer[trajectoryCount];
        for (int i = 0; i < trajectoryCount; i++)
        {
            GameObject laserObject = new GameObject("LaserTrajectory" + i);
            laserObject.transform.parent = transform;
            laserObject.transform.position = firePoint.position;

            LineRenderer newLine = laserObject.AddComponent<LineRenderer>();
            newLine.positionCount = 2;
            newLine.startWidth = 0.05f;
            newLine.endWidth = 0.05f;
            newLine.material = new Material(Shader.Find("Sprites/Default"));
            newLine.startColor = new Color(237 / 255f, 145 / 255f, 33 / 255f, 0.5f);
            newLine.endColor = new Color(237 / 255f, 145 / 255f, 33 / 255f, 0.5f);
            newLine.enabled = false;

            laserTrajectories[i] = newLine;
        }
    }

    void Update()
    {
        if(!isFreeze)
        {
            PlayerGaze();

            if (!isShot)
            {
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0)
                {
                    if(attType == 1)
                    {
                        // 레이저 발사
                        LaserShotReady();

                        // 딜레이주기
                        shootTimer = laserShotDelay;
                        if (isPhase2)
                        {
                            if (normalAttCount >= 4)
                            {
                                // 일반공격 4번 시전시 특수공격으로 전환
                                attType = 2;
                                shootTimer = laserShotDelay * 2;
                            }
                        }

                    }
                    else if(attType == 2)
                    {
                        LaserContinuumShotReady();
                    }
                }
            }

            // 레이저 궤적 관리
            if (isShot)
            {
                UpdateLaserTrajectories();
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

    void LaserShotReady()
    {
        isShot = true;
        artemisAnimator.SetTrigger("IsShot");

        float laserLength = 20f;

        // 방향 보정을위해 -90도 회전
        Vector2 fireDirection = (playerLocation.position - firePoint.position).normalized;
        Vector2 endPosition = (Vector2)firePoint.position + fireDirection * laserLength;

        // 궤적 표시
        SetLaserTrajectory(1, true);

        // 이동할 새로운 위치 설정 (현재 위치 기준 -3.5 ~ +3.5 랜덤 이동)
        float randomY = Random.Range(-3.5f, 3.5f);
        movePos = new Vector2(startPos.x, startPos.y + randomY);

        // 이동 시작
        StartCoroutine(MoveToPosition(movePos));
    }

    void LaserShot()
    {
        // 위치 조정
        Vector2 fireDirection = (playerLocation.position - firePoint.position).normalized;

        // 레이저 발사
        float laserAngle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        GameObject missile = Instantiate(artemisLaser, firePoint.position, Quaternion.Euler(0, 0, laserAngle)); // 보정각도 추가
        missile.GetComponent<ArtemisLaser>().SetDirection(fireDirection);

        // 궤적 숨기기
        SetLaserTrajectory(1, false);

        if (isPhase2)
        {
            normalAttCount++;
        }
    }

    void LaserShotEnd()
    {
        isShot = false;
    }

    void LaserContinuumShotReady()
    {
        isShot = true;
        Debug.Log("HEAR");
        artemisAnimator.SetTrigger("IsContShot");

        // 궤적 표시
        SetLaserTrajectory(2, true);

        // 이동할 새로운 위치 설정 (현재 위치 기준 -3.5 ~ +3.5 랜덤 이동)
        float randomY = Random.Range(-3.5f, 3.5f);
        movePos = new Vector2(startPos.x, startPos.y + randomY);

        // 이동 시작
        StartCoroutine(MoveToPosition(movePos));
    }

    void LaserContinuumShot()
    {
        foreach(LineRenderer traj in laserTrajectories)
        {
            Vector3 startPoint = traj.GetPosition(0);
            Vector3 endPoint = traj.GetPosition(1);
            Vector3 fireDirection = (endPoint - startPoint).normalized;
            float laserAngle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            GameObject missile = Instantiate(artemisLaser, startPoint, Quaternion.Euler(0, 0, laserAngle));
            missile.GetComponent<ArtemisLaser>().SetDirection(fireDirection);
        }

        // 궤적 숨기기
        SetLaserTrajectory(2, false);
    }

    void LaserContinuumShotEnd()
    {
        isShot = false;
        attType = 1;
        normalAttCount = 0;
        shootTimer = laserShotDelay * 2;
    }

    void SetLaserTrajectory(int attType, bool onOff)
    {
        switch (attType)
        {
            case 1:
                laserTrajectories[0].enabled = onOff;
                break;
            case 2:
                laserTrajectories[0].enabled = onOff;
                laserTrajectories[1].enabled = onOff;
                laserTrajectories[2].enabled = onOff;
                laserTrajectories[3].enabled = onOff;
                laserTrajectories[4].enabled = onOff;
                break;
        }
    }

    void UpdateLaserTrajectories()
    {
        Vector2 fireDirection = (playerLocation.position - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;

        // 중앙 레이저 갱신
        UpdateSingleTrajectory(0, fireDirection, baseAngle);

        // 왼쪽 -15도, -30도 갱신
        UpdateSingleTrajectory(1, fireDirection, baseAngle - 15);
        UpdateSingleTrajectory(2, fireDirection, baseAngle - 30);

        // 오른쪽 +15도, +30도 갱신
        UpdateSingleTrajectory(3, fireDirection, baseAngle + 15);
        UpdateSingleTrajectory(4, fireDirection, baseAngle + 30);
    }

    void UpdateSingleTrajectory(int index, Vector2 direction, float angleOffset)
    {
        if (index < 0 || index >= trajectoryCount) return;

        float laserAngle = angleOffset;
        Vector2 newDirection = Quaternion.Euler(0, 0, laserAngle) * Vector2.right;
        Vector2 endPosition = (Vector2)firePoint.position + newDirection * 20f;

        laserTrajectories[index].SetPosition(0, firePoint.position);
        laserTrajectories[index].SetPosition(1, endPosition);
    }

    private IEnumerator MoveToPosition(Vector2 targetPos)
    {
        while ((Vector2)transform.position != targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, artemisMoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator PhaseChange()
    {
        artemisAnimator.SetBool("IsPhase2", true);
        isPhase2 = true;
        isShot = false;
        isFreeze = true;
        laserShotDelay = laserShotDelayPhase2;
        artemisMoveSpeed = artemisMoveSpeedPhase2;
        SetLaserTrajectory(1, false);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (!isFreeze)
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
