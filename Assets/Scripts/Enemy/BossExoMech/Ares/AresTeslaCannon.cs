using UnityEngine;

public class AresTeslaCannon : MonoBehaviour
{
    public Transform firePoint;

    // 필요 정보
    private Transform playerLocation;
    private SpriteRenderer spriteRenderer;
    private Animator apolloAnimator;

    // 상위 관리자
    private GameObject exoMechManager;
    private ExoMech exoMechComponet;
    private EnemyHp exoMechHp;

    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
        exoMechManager = GameObject.FindWithTag("ExoMech");

        spriteRenderer = GetComponent<SpriteRenderer>();
        apolloAnimator = GetComponent<Animator>();
        /*exoMechComponet = exoMechManager.GetComponent<ExoMech>();
        exoMechHp = exoMechManager.GetComponent<EnemyHp>();*/
    }

    void Update()
    {
        PlayerGaze();
    }

    void PlayerGaze()
    {
        Vector2 direction = (playerLocation.position - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        // 시선이 위쪽이여서 -90도를 하여 플레이어를 바라보게함
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
