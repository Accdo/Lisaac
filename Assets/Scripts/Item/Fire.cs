using System.Collections;
using UnityEngine;

public class Fire : Item
{
    [Header("컴포넌트")]
    private SpriteRenderer sr;
    private Animator ani;

    [Header("내부 변수")]
    public int color = 0;
    public float lifeTime = 100f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        Destroy(gameObject, lifeTime);
    }

    public override void PickUpItem(GameObject player)
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

    IEnumerator ChangeFire()
    {
        while (true)
        {
            color = (color % 4) + 1;
            ani.SetInteger("Color", color);

            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator FireAttack()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                switch (ani.GetInteger("Color"))
                {
                    case 1:
                        Debug.Log("빨강");
                        RedFire();
                        break;
                    case 2:
                        Debug.Log("파랑");
                        break;
                    case 3:
                        Debug.Log("남색");
                        break;
                    case 4:
                        Debug.Log("보라");
                        break;
                    case 5:
                        Debug.Log("하양");
                        break;
                }

                yield return new WaitForSeconds(1f);
            }

            yield return null;
        }
    }

    private void RedFire()
    {
        int fireCount = 8; // 개수
        float speed = 3f; // 속도
        float angleStep = 360f / fireCount; // 각도
        float lifeTime = 2f; // 지속

        for (int i = 0; i < fireCount; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject fire = Instantiate(gameObject, transform.position, Quaternion.identity);
            fire.GetComponent<Fire>().enabled = false;

            // 불꽃 색 = 빨강
            fire.GetComponent<Fire>().color = 1; 
            fire.GetComponent<Animator>().SetInteger("Color", 1);

            // 리지드 바디
            Rigidbody2D rb = fire.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = fire.AddComponent<Rigidbody2D>();
            }
            rb.gravityScale = 0;

            rb.linearVelocity = direction * speed; // 이동

            // 일정 시간 후 파괴
            Destroy(fire, lifeTime);
        }
    }
}
