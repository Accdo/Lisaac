using UnityEngine;

public class PooterBullet : PlayerBullet
{
    [SerializeField] private float speed;

    void Update()
    {
        transform.Translate(Direction * (speed * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Enemy"))
        {
            SoundManager.Instance.HitDamage();
            Destroy(gameObject);
        }
        if (collision.CompareTag("Boss"))
        {
            SoundManager.Instance.HitBoss();
            Destroy(gameObject);
        }
    }
}
