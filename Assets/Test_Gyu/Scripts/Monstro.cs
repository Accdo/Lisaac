using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Monstro : MonoBehaviour
{
    public GameObject player;
    public GameObject bullet;
    public Transform body;

    private SpriteRenderer sr;
    private Animator ani;
    private Rigidbody2D rb;

    // 콜라이더 가져오기
    Collider2D bossCollider;
    Collider2D bodyCollider;
    GameObject[] bullets;

    private Color originalColor;
    private bool isFlashing = false;

    private int health;

    void Start()
    {
        sr = body.GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        ani = body.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(BossRoutine());
    }

    void Update()
    {
        health = body.GetComponent<EnemyHp>().currentHp;

        bossCollider = GetComponent<Collider2D>();
        bodyCollider = body.GetComponent<Collider2D>();
        bullets = GameObject.FindGameObjectsWithTag("Bullet");
    }

    // 플레이어와 충돌 시 고정
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    // 피격 상태 (단일)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !isFlashing  && bossCollider.bounds.Intersects(bodyCollider.bounds))
        {
            StartCoroutine(FlashColor(0.1f));
        }
    }

    // 피격 상태(지속)
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !isFlashing  && bossCollider.bounds.Intersects(bodyCollider.bounds))
        {
            StartCoroutine(FlashColor(1f));
        }
    }

    // 색상 변경 코루틴
    IEnumerator FlashColor(float duration)
    {
            // 피격 상태에서 색상 변경
            isFlashing = true;
            sr.color = new Color(1f, 0.5f, 0.5f); // 피격 색상
            yield return new WaitForSeconds(duration);
            sr.color = originalColor; // 원래 색상
            isFlashing = false;
    }

    // 랜덤 모션 코루틴
    IEnumerator BossRoutine()
    {
        while (true)
        {
            // 플레이어 찾기
            player = GameObject.FindGameObjectsWithTag("Player")
                .FirstOrDefault(p => p.GetComponent<Collider2D>() != null);

            yield return new WaitForSeconds(1f);

            int rand = Random.Range(0, 8 + (health < 50 ? 2 : 0)); // 행동 선택 확률
            rand = 10;

            if (rand < 4)
            {
                ani.SetTrigger("Move");
                StartCoroutine(Move(2));
            }
            else if (rand < 8)
            {
                ani.SetTrigger("Attack");
                StartCoroutine(Attack());
            }
            else
            {
                ani.SetTrigger("Jump");
                StartCoroutine(Jump());
            }

            yield return new WaitForSeconds(2f);
        }
    }

    // 공격 코루틴
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(.5f);
        ani.speed = 0f; // 애니메이션 정지

        Vector2 dirToPlayer = (player.transform.position - transform.position).normalized; // 방향 찾기
        float centerAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg; // 각도로 변환

        int bulletCount = Random.Range(6, 9); // 발사체 개수
        float coneAngle = 90f; // 발사 각도 범위

        for (int i = 0; i < bulletCount; i++)
        {
            // 발사각 설정
            float randomOffset = Random.Range(-coneAngle / 2f, coneAngle / 2f);
            float shootAngle = centerAngle + randomOffset;
            float rad = shootAngle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)); // 발사 방향

            float randomSpeed = Random.Range(0.2f, 1.5f); // 속도
            float randomScale = Random.Range(1f, 2f); // 크기

            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity); // 총알 생성
            go.transform.localScale = Vector3.one * randomScale; // 총알 크기 조절

            // 총알 속도 적용
            Rigidbody2D bulletRb = go.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = direction.normalized * randomSpeed;
            }

            yield return new WaitForSeconds(0.1f);
        }

        ani.speed = 1f; // 애니메이션 시작
    }

    // 이동 함수
    IEnumerator Move(float DISTANCE)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (player != null)
        {
            float time = 0f;
            float duration = ani.GetCurrentAnimatorStateInfo(0).length;

            Vector3 start = transform.position;
            Vector3 dir = (player.transform.position - start).normalized;
            Vector3 end = start + dir * DISTANCE;

            sr.flipX = dir.x >= 0f;

            while (time < duration)
            {
                // 본체 이동
                //Vector3 nextPos = Vector3.Lerp(start, end, time / duration);
                //rb.MovePosition(nextPos);

                // 점프 유무
                if (!bossCollider.bounds.Intersects(bodyCollider.bounds))
                {
                    // 겹치지 않으면 충돌 무시
                    Physics2D.IgnoreCollision(bossCollider, player.GetComponent<Collider2D>(), true);
                    Physics2D.IgnoreCollision(bodyCollider, player.GetComponent<Collider2D>(), true);
                    foreach (GameObject bullet in bullets)
                    {
                        if (bullet != null && bullet.GetComponent<Collider2D>() != null)
                        {
                            Physics2D.IgnoreCollision(bossCollider, bullet.GetComponent<Collider2D>(), true);
                            Physics2D.IgnoreCollision(bodyCollider, bullet.GetComponent<Collider2D>(), true);
                        }
                    }
                }

                time += Time.deltaTime;
                yield return null;
            }

            //rb.MovePosition(end);

            // 이동 후 충돌 무시 해제
            Physics2D.IgnoreCollision(bossCollider, player.GetComponent<Collider2D>(), false);
            Physics2D.IgnoreCollision(bodyCollider, player.GetComponent<Collider2D>(), false);
            foreach (GameObject bullet in bullets)
            {
                if (bullet != null && bullet.GetComponent<Collider2D>() != null)
                {
                    Physics2D.IgnoreCollision(bossCollider, bullet.GetComponent<Collider2D>(), false);
                    Physics2D.IgnoreCollision(bodyCollider, bullet.GetComponent<Collider2D>(), false);
                }
            }
        }
    }

    // 점프 코루틴
    IEnumerator Jump()
    {
        Vector3 start = transform.position;
        Vector3 end = player.transform.position;
        Vector3 dir = (end - start);

        StartCoroutine(Move(dir.magnitude)); // 이동

        yield return new WaitForSeconds(1.6f); // 점프 후 대기

        int bulletCount = 15;
        float intervalAngle = 360 / bulletCount; // 발사체 사이각
        float angle = 0;


        for (int i = 0; i < bulletCount * 2; i++)
        {
            angle += intervalAngle * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)); // 발사 방향

            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity); // 총알 생성
            go.transform.localScale = Vector3.one * 2; // 총알 크기 2배

            // 총알 속도 적용
            Rigidbody2D bulletRb = go.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                player = GameObject.FindGameObjectsWithTag("Player")
                    .FirstOrDefault(player => player.GetComponent<Collider2D>() != null);

                start = transform.position;
                end = player.transform.position;
                dir = (end - start);

                bulletRb.linearVelocity = direction - 3 * dir.normalized;
            }
        }

        yield return null;
    }
}