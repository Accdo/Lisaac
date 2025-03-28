using UnityEngine;

public class EnemyBulletCalc : MonoBehaviour
{
    public Transform player;
    public int damage = 1;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
            SoundManager.Instance.HitDamage();
        }

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
