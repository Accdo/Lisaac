using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Fire : Item
{
    [Header("������Ʈ")]
    private SpriteRenderer sr;
    private Animator ani;

    [Header("���� ����")]
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
            // ������ ȹ��
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
                        Debug.Log("����");
                        RedFire();
                        break;
                    case 2:
                        Debug.Log("�Ķ�");
                        StartCoroutine(BlueFire());
                        break;
                    case 3:
                        Debug.Log("����");
                        StartCoroutine(NavyFire());
                        break;
                    case 4:
                        Debug.Log("����");
                        StartCoroutine(PurpleFire());
                        break;
                    case 5:
                        Debug.Log("�Ͼ�");
                        break;
                }

                // �Ͻ� ����
                int originColor = color;
                color = 5;
                ani.SetInteger("Color", color);
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

                yield return new WaitForSeconds(coolTime);

                // �ٽ� ����
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
        // �Ҳ� ����
        FIRE.GetComponent<Fire>().isBullet = true;
        FIRE.GetComponent<Fire>().enabled = false;
        FIRE.GetComponent<Animator>().SetInteger("Color", COLOR);

        // �Ѿ� ����
        FIRE.tag = "Bullet";
        FIRE.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        FIRE.GetComponent<SpriteRenderer>().sortingLayerName = "Bullet";
        FIRE.GetComponent<SpriteRenderer>().sortingOrder = 0;
        FIRE.AddComponent<FireBullet>();
        FIRE.GetComponent<FireBullet>().damage = DAMAGE;
        FIRE.GetComponent<FireBullet>().speed = SPEED;
        FIRE.GetComponent<FireBullet>().life = LIFE;
        FIRE.GetComponent<FireBullet>().Direction = DIRECTION;

        // ������ �ٵ�
        Rigidbody2D rb = FIRE.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = FIRE.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0;
    }

    private void RedFire()
    {
        int damage = 5; // ������
        float speed = 3; // �ӵ�
        float life = 2; // ����
        int count = 8; // ����
        float angleStep = 360 / count; // ����

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
        int damage = 5; // ������
        float speed = 0; // �ӵ�
        float life = 10; // ����
        int count = 10; // ����

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
        int damage = 5; // ������
        float speed = 0; // �ӵ�
        float life = 6; // ����
        int count = 8; // ����
        float angleStep = 360 / count; // ����
        float distance = .8f; // �Ÿ�

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
        int damage = 5; // ������
        float speed = 2; // �ӵ�
        float life = 10; // ����
        int count = 8; // ����

        // Rock ������Ʈ�� �ݶ��̴�
        Collider2D[] rocks = FindObjectsOfType<Collider2D>().Where(c => c.gameObject.name.Contains("Rock")).ToArray();

        // ���� ������� �߻�
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Vector3 direction = (enemy.transform.position - pos).normalized;

            GameObject fire = Instantiate(gameObject, pos, Quaternion.identity);
            ChangeToBullet(fire, 4, damage, speed, direction, life);

            // Rock���� �浹 ����
            foreach (Collider2D rock in rocks)
            {
                Physics2D.IgnoreCollision(fire.GetComponent<Collider2D>(), rock, true);
            }

            yield return new WaitForSeconds(0.2f);

            if (--count <= 0) break;
        }

        // ������ ������� �߻�
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        if (bosses.Length > 0)
        {
            GameObject boss = bosses[0]; // ù ��° ������ ó��

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
