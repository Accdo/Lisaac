using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    public int damage = 1;

    public Vector3 Direction { get; set; }

    public GameObject dieBulletAnim;




    void Update()
    {
        transform.Translate(Direction * (speed * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Enemy"))
        {
            BulletAnim();
            SoundManager.Instance.HitDamage();
        }
        if (collision.CompareTag("Boss"))
        {
            BulletAnim();
            SoundManager.Instance.HitBoss();
        }

    }

    void BulletAnim()
    {
        Destroy(gameObject);
        GameObject go = Instantiate(dieBulletAnim, transform.position, Quaternion.identity);

        float animLength = go.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length; //  애니메이션 길이 가져오기
        Destroy(go, animLength);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
