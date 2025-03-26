using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class Monstro : MonoBehaviour
{
    // 외부 객체
    public GameObject player;
    public GameObject bullet;

    // 하위 객체
    public Transform body;
    public Image healthFill;

    // 컴포넌트
    private SpriteRenderer sr;
    private Animator ani;
    private Rigidbody2D rb;
    private Collider2D bodyCollider;
    private Collider2D myCollider;

    // 내부 상태
    private Color originColor;
    private bool hitting = false;
    private int maxHP;
    private int currentHP;
    private Vector3 direction;

    void Start()
    {
        // 컴포넌트
        sr = body.GetComponent<SpriteRenderer>();
        ani = body.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        originColor = sr.color;

        StartCoroutine(BossRoutine());
    }

    void Update()
    {
        if (body == null) { Destroy(gameObject); return; }

        // 체력
        maxHP = body.GetComponent<EnemyHp>().maxHp;
        currentHP = body.GetComponent<EnemyHp>().currentHp;
        healthFill.fillAmount = (float)currentHP / maxHP;

        // 콜라이더
        myCollider = GetComponent<Collider2D>();
        bodyCollider = body.GetComponent<Collider2D>();

        // 플레이어 찾기
        player = GameObject.FindGameObjectsWithTag("Player")
            .FirstOrDefault(p => p.GetComponent<Collider2D>() != null);

        // 방향 설정 및 시각 반전
        if (player != null)
        {
            direction = player.transform.position - transform.position;
            sr.flipX = direction.x >= 0f;
        }
    }

    // 플레이어와 충돌 시 밀림 방지
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
        if (collision.CompareTag("Bullet"))
        {
            StartCoroutine(FlashColor());
        }
    }

    // 피격 상태 (지속)
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            StartCoroutine(FlashColor());
        }
    }

    // 색상 변경 코루틴
    IEnumerator FlashColor()
    {
        if (!hitting && bodyCollider.bounds.Intersects(myCollider.bounds))
        {
            hitting = true;

            // 피격 색상
            sr.color = new Color(1f, 0.5f, 0.5f);
            healthFill.color = new Color(1f, 0.5f, 0.5f);

            yield return new WaitForSeconds(0.1f);

            // 원래 색상
            sr.color = originColor;
            healthFill.color = new Color(1f, 0f, 0f);

            hitting = false;
        }
    }

    // 랜덤 모션 코루틴
    IEnumerator BossRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            // 행동 확률적 선택
            int rand = Random.Range(0, 101);

            if (currentHP < maxHP / 5) rand = 101;

            if (rand < 30)
            {
                ani.SetTrigger("Move");
                StartCoroutine(Move(2));
            }
            else if (rand < 70)
            {
                ani.SetTrigger("Attack");
                StartCoroutine(Attack());
            }
            else if (rand < 101)
            {
                ani.SetTrigger("Attack");
                StartCoroutine(Shock());
            }
            else
            {
                ani.SetTrigger("Jump");
                StartCoroutine(Jump());
            }

            yield return new WaitForSeconds(3f);
        }
    }

    // 이동 코루틴
    IEnumerator Move(float DISTANCE)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (player == null) yield break;

        float time = 0f;
        float duration = ani.GetCurrentAnimatorStateInfo(0).length;

        Vector3 start = transform.position;
        Vector3 end = start + direction.normalized * DISTANCE;

        while (time < duration)
        {
            // 본체 이동
            Vector3 nextPos = Vector3.Lerp(start, end, time / duration);
            rb.MovePosition(nextPos);

            // 점프 유무
            if (!myCollider.bounds.Intersects(bodyCollider.bounds))
            {
                // 겹치지 않으면 충돌 무시
                myCollider.enabled = false;
                bodyCollider.enabled = false;
            }

            time += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(end);

        // 이동 후 충돌 무시 해제
        myCollider.enabled = true;
        bodyCollider.enabled = true;
    }

    // 점프 코루틴
    IEnumerator Jump()
    {
        StartCoroutine(Move(direction.magnitude)); // 이동

        yield return new WaitForSeconds(1f); // 점프 후 대기

        StartCoroutine(Shock()); // 전방위 공격
    }

    // 전방(부채꼴) 공격 코루틴
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.6f);

        ani.speed = 0f; // 애니메이션 정지

        float centerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        int count = Random.Range(6, 9); // 발사체 개수
        float angleRange = 90f; // 발사 각도 범위

        for (int i = 0; i < count; i++)
        {
            // 발사각 설정
            float offsetAngle = Random.Range(-angleRange / 2f, angleRange / 2f);
            float shootAngle = centerAngle + offsetAngle;
            float rad = shootAngle * Mathf.Deg2Rad;
            Vector2 shoot = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)); // 발사 방향

            float speed = Random.Range(0.2f, 1.5f); // 총알 속도
            float scale = Random.Range(1f, 2f); // 총알 크기

            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity); // 총알 생성
            go.transform.localScale = Vector3.one * scale; // 총알 크기 적용

            // 총알 속도 적용
            Rigidbody2D bulletRb = go.GetComponent<Rigidbody2D>();
            if (bulletRb != null) bulletRb.linearVelocity = shoot.normalized * speed;

            yield return new WaitForSeconds(0.1f);
        }

        ani.speed = 1f;  // 애니메이션 시작
    }

    // 전방위(원형) 공격 코루틴
    IEnumerator Shock()
    {
        yield return new WaitForSeconds(0.6f);

        int count = Random.Range(15, 21); // 발사체 개수
        float intervalAngle = 360 / count; // 발사체 사이각
        float angle = 0;

        for (int i = 0; i < count * 2; i++)
        {
            angle += intervalAngle * Mathf.Deg2Rad;

            Vector3 shoot = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)); // 발사 방향

            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity); // 총알 생성
            go.transform.localScale = Vector3.one * 2; // 총알 크기 2배

            // 총알 속도 적용
            Rigidbody2D bulletRb = go.GetComponent<Rigidbody2D>();
            if (bulletRb != null) bulletRb.linearVelocity = shoot - 3 * direction.normalized;
        }

        yield return null;
    }
}
