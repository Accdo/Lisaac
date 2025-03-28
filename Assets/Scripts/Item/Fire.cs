using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Fire : Item
{
    [Header("컴포넌트")]
    private SpriteRenderer sr;
    private Animator ani;

    [Header("내부 변수")]
    private int color = 0;
    public float coolTime = 10f;
    public float lifeTime = 60f;
    private bool isBullet = false;
    Vector3 pos;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        pos = transform.position + new Vector3(0, -.8f, 0);
    }

    public override void PickUpItem(GameObject player)
    {
        if (!isBullet)
        {
            // 아이템 획득
            SoundManager.Instance.PickupItem();
            transform.SetParent(player.transform);
            transform.localPosition = new Vector3(0f, 1f, 0f);
            transform.localScale *= .8f;

            color = Random.Range(0, 4) + 1;
            StartCoroutine(ChangeFire());
            StartCoroutine(FireAttack());
        }
    }

    IEnumerator ChangeFire()
    {
        while (color != 5)
        {
            color = (color % 4) + 1;
            ani.SetInteger("Color", color);

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator FireAttack()
    {
        Vector3 pos = transform.position + new Vector3(0, -.8f, 0);

        while (true)
        {
            if (Input.GetKey(KeyCode.Q) && ani.GetInteger("Color") != 5)
            {
                switch (ani.GetInteger("Color"))
                {
                    case 1:
                        Debug.Log("빨강");
                        RedFire();
                        break;
                    case 2:
                        Debug.Log("파랑");
                        StartCoroutine(BlueFire());
                        break;
                    case 3:
                        Debug.Log("남색");
                        StartCoroutine(NavyFire());
                        break;
                    case 4:
                        Debug.Log("보라");
                        StartCoroutine(PurpleFire());
                        break;
                    case 5:
                        Debug.Log("하양");
                        break;
                }

                // 일시 정지
                int originColor = color;
                color = 5;
                ani.SetInteger("Color", color);
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

                yield return new WaitForSeconds(coolTime);

                // 다시 시작
                color = originColor;
                ani.SetInteger("Color", color);
                StartCoroutine(ChangeFire());
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }

            yield return null;
        }
    }

    private void ChangeToBullet(GameObject FIRE, int COLOR, int DAMAGE, float SPEED, Vector3 DIRECTION, float LIFE)
    {
        // 불꽃 설정
        FIRE.GetComponent<Fire>().isBullet = true;
        FIRE.GetComponent<Fire>().enabled = false;
        FIRE.GetComponent<Animator>().SetInteger("Color", COLOR);

        // 총알 설정
        FIRE.tag = "Bullet";
        FIRE.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        FIRE.GetComponent<SpriteRenderer>().sortingLayerName = "Bullet";
        FIRE.GetComponent<SpriteRenderer>().sortingOrder = 0;
        FIRE.AddComponent<FireBullet>();
        FIRE.GetComponent<FireBullet>().damage = DAMAGE;
        FIRE.GetComponent<FireBullet>().speed = SPEED;
        FIRE.GetComponent<FireBullet>().life = LIFE;
        FIRE.GetComponent<FireBullet>().Direction = DIRECTION;

        // 리지드 바디
        Rigidbody2D rb = FIRE.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = FIRE.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0;
    }

    private void RedFire()
    {
        int damage = 5; // 데미지
        float speed = 3; // 속도
        float life = 2; // 지속
        int count = 8; // 개수
        float angleStep = 360 / count; // 각도

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            GameObject fire = Instantiate(gameObject, pos, Quaternion.identity);

            ChangeToBullet(fire, 1, damage, speed, direction, life);
        }
    }

    private IEnumerator BlueFire()
    {
        int damage = 5; // 데미지
        float speed = 0; // 속도
        float life = 10; // 지속
        int count = 10; // 개수

        Vector3 direction = pos;

        while (count-- > 0)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                GameObject fire = Instantiate(gameObject, pos, Quaternion.identity);

                ChangeToBullet(fire, 2, damage, speed, direction, life);

                pos = transform.position + new Vector3(0, -.8f, 0);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator NavyFire()
    {
        int damage = 5; // 데미지
        float speed = 0; // 속도
        float life = 6; // 지속
        int count = 8; // 개수
        float angleStep = 360 / count; // 각도
        float distance = .8f; // 거리

        List<GameObject> fires = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            GameObject fire = Instantiate(gameObject, pos + direction * distance, Quaternion.identity);
            ChangeToBullet(fire, 3, damage, speed, direction, life);
            foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
            {
                if (wall != null && wall.GetComponent<Collider2D>() != null)
                {
                    Physics2D.IgnoreCollision(fire.GetComponent<Collider2D>(), wall.GetComponent<Collider2D>(), true);
                }
            }
            fires.Add(fire);

            StartCoroutine(FireMove(fire, direction, distance));

            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }

    private IEnumerator FireMove(GameObject FIRE, Vector3 DIRECTION, float DISTANCE)
    {
        while (FIRE != null)
        {
            FIRE.transform.position = pos + DIRECTION * DISTANCE;

            Collider2D[] hits = Physics2D.OverlapCircleAll(FIRE.transform.position, 0.5f);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("EnemyBullet"))
                {
                    Destroy(hit.gameObject);
                    Destroy(FIRE);
                    yield break;
                }
            }

            yield return null;
        }
    }

    private IEnumerator PurpleFire()
    {
        int damage = 5; // 데미지
        float speed = 2; // 속도
        float life = 10; // 지속
        int count = 8; // 개수

        // Rock 오브젝트의 콜라이더
        Collider2D[] rocks = FindObjectsOfType<Collider2D>().Where(c => c.gameObject.name.Contains("Rock")).ToArray();

        // 적을 대상으로 발사
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Vector3 direction = (enemy.transform.position - pos).normalized;

            GameObject fire = Instantiate(gameObject, pos, Quaternion.identity);
            ChangeToBullet(fire, 4, damage, speed, direction, life);

            // Rock과의 충돌 무시
            foreach (Collider2D rock in rocks)
            {
                Physics2D.IgnoreCollision(fire.GetComponent<Collider2D>(), rock, true);
            }

            yield return new WaitForSeconds(0.2f);

            if (--count <= 0) break;
        }

        // 보스를 대상으로 발사
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        if (bosses.Length > 0)
        {
            GameObject boss = bosses[0]; // 첫 번째 보스만 처리

            if (boss != null)
            {
                Vector3 direction = (boss.transform.position - pos).normalized;

                GameObject fire = Instantiate(gameObject, pos, Quaternion.identity);
                ChangeToBullet(fire, 4, damage * 3, speed * 1.5f, direction, life);
                fire.transform.localScale *= 2f;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}
