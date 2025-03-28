using UnityEngine;

public class ApolloFireBall : MonoBehaviour
{

    public float speed = 5f;
    public float lifeTime = 3f;
    public int damage = 1;
    private Vector2 direction;

    public GameObject fireBallMini;

    void Start()
    {
        Invoke("ExplosionFireball", lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어 데미지 로직
            Destroy(gameObject);
        }
    }

    void ExplosionFireball()
    {
        Vector3 vector = new Vector3(0f, 0f, 45f);
        GameObject missile1 = Instantiate(fireBallMini, transform.position, Quaternion.Euler(vector));
        vector.z += 90;
        GameObject missile2 = Instantiate(fireBallMini, transform.position, Quaternion.Euler(vector));
        vector.z += 90;
        GameObject missile3 = Instantiate(fireBallMini, transform.position, Quaternion.Euler(vector));
        vector.z += 90;
        GameObject missile4 = Instantiate(fireBallMini, transform.position, Quaternion.Euler(vector));

        Destroy(gameObject);
    }

}
