using UnityEngine;

public class Apollo : MonoBehaviour
{
    Transform playerLocation;
    //아폴로
    void Start()
    {
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        /*float playerLoocking;
        Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        Vector3 dir = playerLocation.position - pos;

        playerLoocking = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, playerLoocking);*/

        Vector2 direction = (playerLocation.position - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
