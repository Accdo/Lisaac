using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int maxHp = 3;
    [SerializeField] private int currentHp;
    private HealthUI healthUI;

    void Start()
    {
        currentHp = maxHp;
        healthUI = FindAnyObjectByType<HealthUI>();
        healthUI.UpdateHearts(currentHp, maxHp);
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
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            TakeDamage(1);
        }
    }


}
