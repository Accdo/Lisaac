using UnityEngine;

public class PooterBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    
    public int damage = 2;
    Vector3 Direction;

    void Update()
    {
        transform.Translate(Direction * (speed * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Enemy"))
        {
            SoundManager.Instance.HitDamage();
        }
        if (collision.CompareTag("Boss"))
        {
            SoundManager.Instance.HitBoss();
        }
    }
}
