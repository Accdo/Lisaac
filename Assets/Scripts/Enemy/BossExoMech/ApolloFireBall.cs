using UnityEngine;

public class ApolloFireBall : MonoBehaviour
{

    public float speed = 5f;
    public float lifeTime = 3f;
    public int damage = 10;
    public Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifeTime);
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

}
