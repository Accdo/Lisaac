using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Video;

public class Monstro : MonoBehaviour
{
    [Header("외부 객체")]
    public GameObject player;
    public GameObject bullet;
    public GameObject[] pBullets;
    public GameObject nextBoss;

    [Header("하위 객체")]
    [SerializeField] private Transform body;
    [SerializeField] private RawImage intro;
    [SerializeField] private Image healthFill;

    [Header("컴포넌트")]
    private SpriteRenderer sprite;
    private Animator ani;
    private Rigidbody2D rigid;
    private Collider2D bodyCollider;
    private Collider2D myCollider;
    private AudioSource mainCamAudio;
    [SerializeField] private VideoPlayer video;

    [Header("내부 변수")]
    private Color originColor;
    private bool hitting = false;
    private int maxHP;
    private int currentHP;
    [SerializeField] private float speed = 3f;
    private Vector3 direction;

    void Start()
    {
        // 컴포넌트
        sprite = body.GetComponent<SpriteRenderer>();
        ani = body.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        mainCamAudio = Camera.main?.GetComponent<AudioSource>();

        originColor = sprite.color;

        // 배경음 비활성화
        mainCamAudio.enabled = false;

        // 플레이어 찾기
        player = GameObject.FindGameObjectsWithTag("Player")
            .FirstOrDefault(p => p.GetComponent<Collider2D>() != null);

        transform.position -= (player.transform.position - transform.position) * .8f;

        StartCoroutine(BossRoutine());
    }

    void Update()
    {
        if (body == null)
        {
            Destroy(gameObject);
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("EnemyBullet"))
                Destroy(go);

            foreach (GameObject door in FindObjectsByType<GameObject>(FindObjectsSortMode.None).Where(d => d.name.Contains("Door")))
            {
                if (door != null && door.GetComponent<Collider2D>() != null)
                {
                    Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), door.GetComponent<Collider2D>(), true);
                }
            }

            Instantiate(nextBoss, transform.position, Quaternion.identity);
            return;
        }

        // 체력
        maxHP = body.GetComponent<EnemyHp>().maxHp;
        currentHP = body.GetComponent<EnemyHp>().currentHp;
        healthFill.fillAmount = (float)currentHP / maxHP;

        // 콜라이더
        myCollider = GetComponent<Collider2D>();
        bodyCollider = body.GetComponent<Collider2D>();
        pBullets = GameObject.FindGameObjectsWithTag("Bullet");

        // 플레이어 찾기
        player = GameObject.FindGameObjectsWithTag("Player")
            .FirstOrDefault(p => p.GetComponent<Collider2D>() != null);

        if (player != null)
        {
            // 방향 설정 및 시각 반전
            direction = player.transform.position - transform.position;
            sprite.flipX = direction.x >= 0f;
        }
    }

    // 플레이어와 충돌 시 밀림 방지
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;
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
            sprite.color = new Color(1f, 0.5f, 0.5f);
            healthFill.color = new Color(1f, 0.5f, 0.5f);

            yield return new WaitForSeconds(0.1f);

            // 원래 색상
            sprite.color = originColor;
            healthFill.color = new Color(1f, 0f, 0f);

            hitting = false;
        }
    }

    // 랜덤 모션 코루틴
    IEnumerator BossRoutine()
    {
        // 시간 정지 후 인트로 재생
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(Mathf.Max((float)video.length, 0.1f));
        intro.gameObject.SetActive(false);
        Time.timeScale = 1;
        yield return new WaitForSeconds(.2f);
        mainCamAudio.enabled = true;

        while (body != null)
        {
            // 행동 확률적 선택
            int rand = Random.Range(0, 100);

            if (currentHP < maxHP / 4)
            {
                Debug.Log("점프");
                ani.SetTrigger("Jump");
                StartCoroutine(Jump());
                yield return new WaitForSeconds(2f);
            }
            else if (rand < 30)
            {
                Debug.Log("이동");
                ani.SetTrigger("Move");
                StartCoroutine(Move(Mathf.Max(speed, direction.magnitude / 2)));
                yield return new WaitForSeconds(.5f);
            }
            else if (rand < 80)
            {
                Debug.Log("공격");
                ani.SetTrigger("Attack");
                StartCoroutine(Attack());
                yield return new WaitForSeconds(1f);
            }
            else if (rand < 100)
            {
                Debug.Log("스킬");
                ani.SetTrigger("Attack");
                StartCoroutine(Shock());
                yield return new WaitForSeconds(1.5f);
            }

            if (currentHP <= 0) break;

            yield return new WaitForSeconds(.5f);
        }
    }

    // 이동 코루틴
    IEnumerator Move(float DISTANCE)
    {
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (player == null) yield break;

        SoundManager.Instance.BossMove();

        yield return new WaitForEndOfFrame();
        float duration = ani.GetCurrentAnimatorStateInfo(0).length / Mathf.Max(ani.speed, 0.01f);
        float time = 0f;
        bool jumping = false;

        Vector3 start = transform.position;
        Vector3 end = start + direction.normalized * DISTANCE;

        while (time < duration)
        {
            // 본체 이동
            Vector3 nextPos = Vector3.Lerp(start, end, time / duration);
            rigid.MovePosition(nextPos);

            // 점프 유무
            jumping = !bodyCollider.bounds.Intersects(myCollider.bounds);

            // 충돌 활성화/비활성화
            Physics2D.IgnoreCollision(myCollider, player.GetComponent<Collider2D>(), jumping);
            Physics2D.IgnoreCollision(bodyCollider, player.GetComponent<Collider2D>(), jumping);
            foreach (GameObject bullet in pBullets)
            {
                if (bullet != null && bullet.GetComponent<Collider2D>() != null)
                {
                    Physics2D.IgnoreCollision(myCollider, bullet.GetComponent<Collider2D>(), jumping);
                    Physics2D.IgnoreCollision(bodyCollider, bullet.GetComponent<Collider2D>(), jumping);
                }
            }

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        rigid.MovePosition(end);

        yield return new WaitForEndOfFrame();
    }

    // 점프 코루틴
    IEnumerator Jump()
    {
        StartCoroutine(Move(direction.magnitude)); // 이동

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length / ani.speed - .7f); // 점프 후 대기

        SoundManager.Instance.BossJump();

        StartCoroutine(Shock()); // 전방위 공격
    }

    // 전방(부채꼴) 공격 코루틴
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.6f);

        SoundManager.Instance.BossAttack();

        try
        {
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
            }
        }
        finally
        {
            ani.speed = 1f;  // 애니메이션 시작
        }

        yield return new WaitForEndOfFrame();
    }

    // 전방위(원형) 공격 코루틴
    IEnumerator Shock()
    {
        yield return new WaitForSeconds(0.6f);

        SoundManager.Instance.BossAttack();

        int count = Random.Range(15, 21); // 발사체 개수
        float intervalAngle = 360 / count; // 발사체 사이각
        float angle = 0;

        for (int i = 0; i < count; i++)
        {
            angle += intervalAngle * Mathf.Deg2Rad;

            Vector3 shoot = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)); // 발사 방향

            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity); // 총알 생성
            go.transform.localScale = Vector3.one * 2; // 총알 크기 2배

            // 총알 데미지 2배
            go.GetComponent<EnemyBullet>().damage = 2;

            // 총알 속도 적용
            Rigidbody2D bulletRb = go.GetComponent<Rigidbody2D>();
            if (bulletRb != null) bulletRb.linearVelocity = shoot - 3 * direction.normalized;
        }

        yield return new WaitForSeconds(3f);
    }
}