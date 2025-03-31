using UnityEngine;

public class AresLaserCannon : MonoBehaviour
{

    // 무기 정보
    public float shotDelay = 1f;
    public GameObject laserBeam;
    public Transform firePoint;

    // 필요 정보
    private Transform playerLocation;
    private Animator weponeAnimator;

    // 상위 관리자
    private GameObject exoMechManager;
    private ExoMech exoMechComponet;

    // 스테이터스
    private bool isShot;
    private bool isFreeze;
    private bool isFiring;
    private float shootTimer;
    private GameObject laserBeamObj;
    private LineRenderer lineRenderer;

    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        exoMechManager = GameObject.FindWithTag("ExoMech");

        weponeAnimator = GetComponent<Animator>();
        exoMechComponet = exoMechManager.GetComponent<ExoMech>();

        isShot = false;
        isFreeze = false;
        isFiring = false;
        shootTimer = shotDelay;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1, 0, 0, 0.5f);  // 빨간색 + 50% 투명도
        lineRenderer.endColor = new Color(1, 0, 0, 0.5f);    // 빨간색 + 50% 투명도
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!isFreeze)
        {
            PlayerGaze();
            if (!isShot)
            {
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0)
                {
                    LaserCannonFireReady();
                    shootTimer = shotDelay;
                }
            }
            else
            {
                UpdateSingleTrajectory();
            }
        }
    }

    void LaserCannonFireReady()
    {
        isShot = true;
        weponeAnimator.SetTrigger("IsShot");

        // 궤적 표시
        lineRenderer.enabled = true;

        // SoundManager.Instance.AresLaserCharge();
    }

    void LaserCannonFireImminent()
    {
        isFiring = true;
    }

    void LaserCannonFire()
    {
        // 궤적 시작점 끝점
        Vector3 startPos = lineRenderer.GetPosition(0);
        Vector3 endPos = lineRenderer.GetPosition(1);

        // 레이저 방향 계산
        Vector2 fireDirection = (endPos - startPos).normalized;

        // 레이저 발사 각도 계산
        float laserAngle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;

        // 레이저 발사
        laserBeamObj = Instantiate(laserBeam, firePoint.position, Quaternion.Euler(0, 0, laserAngle + 90f)); // 보정각도 추가


        SoundManager.Instance.AresLaserShot();

        // 궤적 숨기기
        lineRenderer.enabled = false;
    }

    void LaserCannonFireEnd()
    {
        Destroy(laserBeamObj);
    }

    void LaserCannonEnd()
    {
        isShot = false;
        isFiring = false;
    }

    void PlayerGaze()
    {
        if (isFiring) return;
        Vector2 direction = (playerLocation.position - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        // 시선이 위쪽이여서 -90도를 하여 플레이어를 바라보게함
        transform.rotation = Quaternion.Euler(0, 0, angle);
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

    void UpdateSingleTrajectory()
    {
        if (isFiring) return;
        Vector2 fireDirection = (playerLocation.position - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;

        Vector2 newDirection = Quaternion.Euler(0, 0, baseAngle) * Vector2.right;
        Vector2 endPosition = (Vector2)firePoint.position + newDirection * 20f;

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, endPosition);
    }
}
