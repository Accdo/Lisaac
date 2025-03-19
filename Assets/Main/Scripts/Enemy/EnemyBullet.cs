using Unity.Mathematics;
using UnityEngine;


public class EnemyBullet : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float speed;
    public int damage = 1;
    public GameObject dieBulletAnim;
    Vector2 dirNo;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player != null)
        {
            dirNo = (player.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(dirNo.x, dirNo.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }


    void Update()
    {
        Move();
    }

    void Move()
    {
        if (player == null) return;
        transform.Translate(dirNo * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
            BulletAnim();
            SoundManager.Instance.HitDamage();
        }

    }

    void BulletAnim()
    {

        GameObject go = Instantiate(dieBulletAnim, transform.position, Quaternion.identity);

        float animLength = go.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(go, animLength);

    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
