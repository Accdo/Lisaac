using Unity.VisualScripting;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    public int maxHp = 1;
    public int currentHp;
    [SerializeField] private GameObject[] dorpItems;
    private EnemyShrink enemyShrink;


    void Start()
    {
        currentHp = maxHp;
        enemyShrink = GetComponent<EnemyShrink>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            PlayerBullet bullet = collision.GetComponent<PlayerBullet>();
            TakeDamage(bullet.damage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
        else
        {
            if (enemyShrink != null)
            {
                enemyShrink.UpdateSprite(maxHp - currentHp);
            }
        }

    }

    void Die()
    {
        SoundManager.Instance.DieSound();
        DropItem();
        Destroy(gameObject);
    }

    void DropItem()
    {
        if (dorpItems != null && dorpItems.Length > 0)
        {
            int randomIndex = Random.Range(0, dorpItems.Length);
            Instantiate(dorpItems[randomIndex], transform.position, Quaternion.identity);
        }
    }

}
