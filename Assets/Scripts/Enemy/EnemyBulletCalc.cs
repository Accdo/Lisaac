using UnityEngine;

public class EnemyBulletCalc : MonoBehaviour
{
    public Transform player;
    public int damage = 1;
    public bool donDestroy = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Wall"))
        {
            if (!donDestroy)
            {
                Destroy(gameObject);
            }
            SoundManager.Instance.HitDamage();
        }

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
