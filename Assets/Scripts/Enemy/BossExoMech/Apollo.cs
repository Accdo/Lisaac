using System;
using UnityEngine;

public class Apollo : MonoBehaviour
{

    public float fireBallshotDelay = 2f;
    public GameObject fireBall;

    public Transform firePoint;
    private Transform playerLocation;
    private float shootTimer;
    private SpriteRenderer spriteRenderer;

    //아폴로
    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        shootTimer = fireBallshotDelay;
    }

    void Update()
    {
        Vector2 direction = (playerLocation.position - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        // 시선이 위쪽이여서 -90도를 하여 플레이어를 바라보게함
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            FireBallShot();
            shootTimer = fireBallshotDelay;
        }
    }

    void FireBallShot()
    {
        GameObject missile = Instantiate(fireBall, firePoint.position, firePoint.rotation);

        missile.GetComponent<ApolloFireBall>().SetDirection(firePoint.position);
    }
}
