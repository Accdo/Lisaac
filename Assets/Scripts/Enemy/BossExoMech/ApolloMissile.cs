using UnityEngine;

public class ApolloMissile : MonoBehaviour
{
    public GameObject boom;
    public float speed = 2f;
    public float lifeTime = 3f;
    public int damage = 1;

    private Transform playerLocation;

    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        Invoke("SelfDestruct", lifeTime);
    }

    void Update()
    {
        Vector2 direction = (playerLocation.position - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        // 시선이 위쪽이여서 -90도를 하여 플레이어를 바라보게함
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void SelfDestruct()
    {
        Instantiate(boom, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
