using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Monstro : MonoBehaviour
{
    public GameObject player;
    public GameObject bullet;

    private SpriteRenderer sr;
    private Animator ani;

    private Color originalColor;
    private bool isFlashing = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        originalColor = sr.color;

        StartCoroutine(BossRoutine());
    }

    // 피격 상태
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !isFlashing)
        {
            StartCoroutine(FlashColor(new Color(1f, 0.5f, 0.5f), 0.1f));
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !isFlashing)
        {
            StartCoroutine(FlashColor(new Color(1f, 0.5f, 0.5f), 1f));
        }
    }

    IEnumerator FlashColor(Color flashColor, float duration)
    {
        // 피격 상태에서 색상 변경
        isFlashing = true;
        sr.color = flashColor;
        yield return new WaitForSeconds(duration);
        sr.color = originalColor;
        isFlashing = false;
    }

    // 랜덤 공격
    IEnumerator BossRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            int rand = Random.Range(1, 2);
            switch (rand)
            {
                case 1:
                    Debug.Log("보스 공격");
                    ani.SetTrigger("Attack"); // 트리거 설정
                    break;
            }

            yield return new WaitForSeconds(1); // 대기
        }
    }

    void Attack()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 찾기

        if (player != null)
        {
            StartCoroutine(FireBulletsWithDelay());
        }
    }

    IEnumerator FireBulletsWithDelay()
    {
        ani.speed = 0; // 애니메이션 정지

        Vector2 dirToPlayer = (player.transform.position - transform.position).normalized; // 방향 찾기

        float centerAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg; // 각도로 변환

        int bulletCount = Random.Range(6, 9); // 6~8발
        float coneAngle = 90f; // 각도 범위

        for (int i = 0; i < bulletCount; i++)
        {
            // 발사각 설정
            float randomOffset = Random.Range(-coneAngle / 2f, coneAngle / 2f);
            float shootAngle = centerAngle + randomOffset;
            float rad = shootAngle * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)); // 발사 방향

            float randomSpeed = Random.Range(0.2f, 1.5f); // 총알 속도 랜덤
            float randomScale = Random.Range(1f, 2f); // 총알 크기 랜덤

            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity); // 총알 생성
            go.transform.localScale = Vector3.one * randomScale; // 총알 크기 조절

            // 총알 속도 적용
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction.normalized * randomSpeed;
            }

            yield return new WaitForSeconds(0.1f); // 대기
        }

        ani.speed = 1; // 애니메이션 시작
    }
}