using System.Collections;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int maxHp = 3;
    [SerializeField] private int currentHp;
    private HealthUI healthUI;

    SpriteRenderer body_sprite;
    public SpriteRenderer head_sprite;

    Vector3 BeforePos;
    Rigidbody2D rb;

    void Start()
    {
        currentHp = maxHp;
        healthUI = FindAnyObjectByType<HealthUI>();
        healthUI.UpdateHearts(currentHp, maxHp);

        body_sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(SavePosition());
    }

    IEnumerator SavePosition()
    {
        while (true)
        {
            BeforePos = transform.position;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0)
        {
            currentHp = 0;
        }
        healthUI.UpdateHearts(currentHp, maxHp);
        SoundManager.Instance.PlayerHert();
    }

    public void HealthUp(int value)
    {
        maxHp = Mathf.Min(maxHp + value, 20);
        currentHp = Mathf.Min(currentHp, maxHp);
        healthUI.UpdateHearts(currentHp, maxHp);

    }

    public void Healing(int value)
    {
        currentHp = Mathf.Min(currentHp + value, maxHp);
        healthUI.UpdateHearts(currentHp, maxHp);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            int damage = collision.gameObject.GetComponent<EnemyBullet>().damage;
            TakeDamage(damage);
        }

        if(collision.gameObject.CompareTag("Falling"))
        {
            TakeDamage(1);
            StartCoroutine(FlickPlayer());
            transform.position = BeforePos;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            TakeDamage(1);
        }

        if(collision.gameObject.CompareTag("Trap"))
        {
            TakeDamage(1);
            StartCoroutine(FlickPlayer());
        }

    }

    IEnumerator FlickPlayer()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        for (int i = 0; i < 3; i++)
        {
            body_sprite.color = new Color(1, 1, 1, 0);
            head_sprite.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.1f);
            body_sprite.color = new Color(1, 1, 1, 1);
            head_sprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
        }
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


}
