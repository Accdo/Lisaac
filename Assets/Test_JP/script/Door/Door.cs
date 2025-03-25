using Unity.VisualScripting;
using UnityEngine;

public abstract class Door : MonoBehaviour
{
    // 메인 카메라 이동 거리 (가로, 세로)
    protected const float PADDINGX = 18;
    protected const float PADDINGY = 10;

    [SerializeField] protected Sprite openDoor;  // 문이 열렸을 때의 스프라이트
    [SerializeField] protected Sprite closeDoor; // 문이 닫혔을 때의 스프라이트

    protected bool isOpen = false;               // 문 열림 여부
    protected BoxCollider2D col;                 // 충돌 판정
    protected SpriteRenderer spRender;           // 스프라이트 렌더러

    // 방에 몬스터가 없으면 문을 연다
    protected void DoorCheck()
    {
        isOpen = RoomManager.Instance.nonMonster;

        if (isOpen)
        {
            col.enabled = true;
            spRender.sprite = openDoor;
        }
        else
        {
            col.enabled = false;
            spRender.sprite = closeDoor;
        }
    }

    // 플레이어가 문과 충돌했을 때 호출될 맵 이동 추상 메서드
    protected abstract void MapMove(GameObject player);
}
