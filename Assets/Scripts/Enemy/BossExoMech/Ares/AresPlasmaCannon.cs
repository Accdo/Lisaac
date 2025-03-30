using UnityEngine;

public class AresPlasmaCannon : MonoBehaviour
{
    // 무기 정보
    public float shotDelay = 1f;
    public Transform firePoint;
    public GameObject plasmaBall;

    // 필요 정보
    private Transform playerLocation;
    private SpriteRenderer spriteRenderer;
    private Animator weponeAnimator;

    // 상위 관리자
    private GameObject exoMechManager;
    private ExoMech exoMechComponet;
    private EnemyHp exoMechHp;

    // 스테이터스
    private bool isShot;
    private bool isFreeze;
    private float shootTimer;

    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        exoMechManager = GameObject.FindWithTag("ExoMech");

        spriteRenderer = GetComponent<SpriteRenderer>();
        weponeAnimator = GetComponent<Animator>();
        /*exoMechComponet = exoMechManager.GetComponent<ExoMech>();
        exoMechHp = exoMechManager.GetComponent<EnemyHp>();*/
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
                    PlasmaCannonFireReady();
                    shootTimer = shotDelay;
                }
            }
        }
    }

    void PlasmaCannonFireReady()
    {
        isShot = true;
        weponeAnimator.SetTrigger("IsShot");
    }

    void PlasmaCannonFire()
    {
        // 위치 조정
        Vector2 fireDirection = (playerLocation.position - firePoint.position).normalized;

        // 레이저 발사
        float laserAngle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        GameObject missile = Instantiate(plasmaBall, firePoint.position, Quaternion.Euler(0, 0, laserAngle - 90f)); // 보정각도 추가
        SoundManager.Instance.ExoPlasmaShoot();

        /*GameObject missile = Instantiate(plasmaBall, firePoint.position, firePoint.rotation);
        missile.GetComponent<ArtemisLaser>().SetDirection((playerLocation.position - firePoint.position).normalized);*/
        // missile.transform.Rotate(0, 90, 0);
        // missile.GetComponent<ApolloFireBall>().SetDirection((playerLocation.position - firePoint.position).normalized);
    }

    void PlasmaCannonEnd()
    {
        isShot = false;
    }

    void PlayerGaze()
    {
        Vector2 direction = (playerLocation.position - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        // 시선이 위쪽이여서 -90도를 하여 플레이어를 바라보게함
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
