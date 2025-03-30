using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fire : Item
{
    [Header("컴포넌트")]
    private SpriteRenderer sprite;
    private Animator ani;

    [Header("내부 변수")]
    private int fireColor = 0;
    public float coolTime = 10f;
    public float lifeTime = 120f;
    private bool isBullet = false;
    Vector3 pos;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        pos = GameObject.FindGameObjectsWithTag("Player")
            .FirstOrDefault(p => p.name == "Head")
            .transform.position - new Vector3(0f, 0.12f, 0f);
    }

    public override void PickUpItem(GameObject player)
    {
        if (!isBullet)
        {
            Transform fireTransform = player.transform.Find("Fire");
            if (fireTransform != null && fireTransform.gameObject.CompareTag("Item"))
            {
                Destroy(fireTransform.gameObject);
            }

            // 아이템 획득
            SoundManager.Instance.PickupItem();
            transform.SetParent(player.transform);
            transform.localPosition = new Vector3(0f, 1f, 0f);
            transform.localScale *= .8f;

            fireColor = Random.Range(0, 4) + 1;
            StartCoroutine(ChangeFire());
            StartCoroutine(FireAttack());
        }
    }

    IEnumerator ChangeFire()
    {
        while (fireColor != 5)
        {
            fireColor = (fireColor % 4) + 1;
            ani.SetInteger("Color", fireColor);

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
                int originColor = fireColor;
                fireColor = 5;
                ani.SetInteger("Color", fireColor);
                sprite.color = new Color(1f, 1f, 1f, 0.7f);

                yield return new WaitForSeconds(coolTime);

                // 다시 시작
                fireColor = originColor;
                ani.SetInteger("Color", fireColor);
                StartCoroutine(ChangeFire());
                sprite.color = new Color(1f, 1f, 1f, 1f);
            }

            yield return null;
        }
    }

    private void MakeBullet(GameObject FIRE, int COLOR, int DAMAGE, float SPEED, Vector3 DIRECTION, float LIFE)
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
        FIRE.GetComponent<FireBullet>().Create(DAMAGE, SPEED, DIRECTION, LIFE);

        // 리지드 바디
        Rigidbody2D rb = FIRE.GetComponent<Rigidbody2D>();
        if (rb == null) rb = FIRE.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void RedFire()
    {
        int damage = 5; // 데미지
        float speed = 3; // 속도
        float life = 3; // 지속
        int count = 8; // 개수
        float angleStep = 360 / count; // 각도

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            GameObject fire = Instantiate(gameObject, pos, Quaternion.identity);
            MakeBullet(fire, 1, damage, speed, direction, life);
        }
    }

    private IEnumerator BlueFire()
    {
        int damage = 5; // 데미지
        float speed = 0; // 속도
        float life = 8; // 지속
        int count = 10; // 개수

        Vector3 direction = pos;

        while (count > 0)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                GameObject fire = Instantiate(gameObject, pos, Quaternion.identity);
                MakeBullet(fire, 2, damage, speed, direction, life);

                count--;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator NavyFire()
    {
        int damage = 5; // 데미지
        float speed = 0; // 속도
        float life = 10; // 지속
        int count = 8; // 개수
        float angleStep = 360 / count; // 각도
        float distance = .8f; // 거리

        List<GameObject> fires = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            GameObject fire = Instantiate(gameObject, pos + direction * distance, Quaternion.identity);
            MakeBullet(fire, 3, damage, speed, direction, life);

            foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
            {
                if (wall != null && wall.GetComponent<Collider2D>() != null)
                {
                    Physics2D.IgnoreCollision(fire.GetComponent<Collider2D>(), wall.GetComponent<Collider2D>(), true);
                }
            }
            fires.Add(fire);

            StartCoroutine(FireFollow(fire, direction, distance));

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator FireFollow(GameObject FIRE, Vector3 DIRECTION, float DISTANCE)
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
        float speed = 1; // 속도
        float life = 10; // 지속
        int count = 5; // 개수

        // 타겟팅
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy")
            .Concat(GameObject.FindGameObjectsWithTag("Boss"))
            .Where(b => b != null && b.GetComponent<EnemyHp>() != null || b.name.Contains("Boss"))
            .OrderBy(b => Vector3.Distance(pos, b.transform.position))
            .ToArray();

        foreach (GameObject target in targets)
        {
            if (target == null) continue;

            count -= target.CompareTag("Boss") ? 3 : 1;
            Vector3 direction = (target.transform.position - pos).normalized;

            GameObject fire = Instantiate(gameObject, pos + direction * .8f, Quaternion.identity);
            MakeBullet(fire, 4, target.CompareTag("Boss") ? damage * 3 : damage, 0, direction, life);
            fire.transform.localScale *= target.CompareTag("Boss") ? 2f : 1f;

            foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall").Where(w => !w.name.Contains("Wall")))
            {
                if (wall != null && wall.GetComponent<Collider2D>() != null)
                {
                    Physics2D.IgnoreCollision(fire.GetComponent<Collider2D>(), wall.GetComponent<Collider2D>(), true);
                }
            }

            StartCoroutine(FireMove(fire, target, targets, speed));

            if (count <= 0) yield break;

            yield return new WaitForSeconds(.1f);
        }
    }

    private IEnumerator FireMove(GameObject FIRE, GameObject TARGET, GameObject[] TARGETS, float SPEED)
    {
        while (FIRE != null)
        {
            if (TARGET == null)
            {
                TARGET = TARGETS.FirstOrDefault(t => t != null && t.activeInHierarchy);

                if (TARGET == null)
                {
                    SoundManager.Instance.FireOff();
                    Destroy(FIRE);
                    yield break;
                }
            }
            else
            {
                Vector3 DIRECTION = (TARGET.transform.position - FIRE.transform.position).normalized;
                FIRE.transform.position += DIRECTION * SPEED * Time.deltaTime;
            }

            yield return null;
        }
    }
}
