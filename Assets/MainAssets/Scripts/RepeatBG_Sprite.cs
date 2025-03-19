using UnityEngine;

public class RepeatBG_Sprite : MonoBehaviour
{
    // 이동 속도 (1~200 사이에서 조절 가능)
    [SerializeField][Range(0.5f, 200f)] float speed = 3f;

    // 배경이 반복되는 거리 (Sprite 크기로 자동 계산)
    float repeatDistance;

    // 배경의 초기 위치
    Vector2 startPos;

    // 배경의 새로운 위치 계산용 변수
    float newPos;

    void Start()
    {
        // 현재 오브젝트의 초기 위치 저장
        startPos = transform.position;

        // SpriteRenderer를 사용해 배경 스프라이트의 가로 크기를 계산
        repeatDistance = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Mathf.Repeat를 사용해 반복적인 X축 이동 거리 계산
        newPos = Mathf.Repeat(Time.time * speed, repeatDistance);

        // 배경을 왼쪽으로 이동하며 초기 위치에서 newPos만큼 이동
        transform.position = startPos + Vector2.left * newPos;
    }
}
