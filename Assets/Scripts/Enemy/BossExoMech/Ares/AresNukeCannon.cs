using UnityEngine;

public class AresNukeCannon : MonoBehaviour
{

    // 무기 정보
    public float shotDelay = 7f;
    public GameObject nuke;
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
    private float shootTimer;

    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        exoMechManager = GameObject.FindWithTag("ExoMech");

        weponeAnimator = GetComponent<Animator>();
        exoMechComponet = exoMechManager.GetComponent<ExoMech>();

        isShot = false;
        isFreeze = false;
        shootTimer = shotDelay;
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
                    NukeCannonFireReady();
                    shootTimer = shotDelay;
                }
            }
        }
    }
    void NukeCannonFireReady()
    {
        isShot = true;
        weponeAnimator.SetTrigger("IsShot");
    }

    void NukeCannonFire()
    {
        GameObject missile = Instantiate(nuke, firePoint.position, firePoint.rotation);
        // SoundManager.Instance.ExoPlasmaShoot();
    }

    void NukeCannonReGenEnd()
    {
        isShot = false;
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

    void PlayerGaze()
    {
        Vector2 direction = (playerLocation.position - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        // 시선이 위쪽이여서 -90도를 하여 플레이어를 바라보게함
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
