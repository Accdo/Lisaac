using UnityEngine;

public class AresBody : MonoBehaviour
{
    // 필요 정보
    private Transform playerLocation;

    // 상위 관리자
    private GameObject exoMechManager;
    private ExoMech exoMechComponet;

    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        exoMechManager = GameObject.FindWithTag("ExoMech");

        exoMechComponet = exoMechManager.GetComponent<ExoMech>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            PlayerBullet bullet = collision.GetComponent<PlayerBullet>();
            exoMechComponet.ExoMechsOnHit(bullet.damage);
        }
    }
}
