using System;
using UnityEngine;

public class Apollo : MonoBehaviour
{

    public float fireBallshotDelay = 2f;
    public GameObject fireBall;
    public GameObject apolloMissile;

    public Transform firePoint;
    private Transform playerLocation;
    private float shootTimer;
    private SpriteRenderer spriteRenderer;
    private Animator apolloAnimator;

    private bool isShot;
    private bool isPhase2;

    private int attType;
    private int normalAttCount;
    private GameObject exoMechManager;
    private ExoMech exoMechComponet;
    private EnemyHp exoMechHp;



    //아폴로
    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        shootTimer = fireBallshotDelay;
        apolloAnimator = GetComponent<Animator>();
        isShot = false;
        isPhase2 = false;

        attType = 1;
        normalAttCount = 0;

        exoMechManager = GameObject.FindWithTag("ExoMech");
        exoMechComponet = exoMechManager.GetComponent<ExoMech>();
        exoMechHp = exoMechManager.GetComponent<EnemyHp>();
}

    void Update()
    {
        Vector2 direction = (playerLocation.position - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        // 시선이 위쪽이여서 -90도를 하여 플레이어를 바라보게함
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        if(!isShot)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                Debug.Log("ShootDelay : " + fireBallshotDelay);
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

    void FireBallShot()
    {
        GameObject missile = Instantiate(fireBall, firePoint.position, firePoint.rotation);

        if (isPhase2)
        {
            normalAttCount++;
            Debug.Log(normalAttCount);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            PlayerBullet bullet = collision.GetComponent<PlayerBullet>();
            exoMechComponet.ExoMechsOnHit(bullet.damage);

            // Debug.Log(exoMechHp.currentHp);

            if (exoMechHp.currentHp <= exoMechHp.maxHp / 2 && !isPhase2)
            {
                apolloAnimator.SetBool("IsPhase2", true);
                isPhase2 = true;
                isShot = false;
                fireBallshotDelay = 0.5f;
            }
        }
    }
}
