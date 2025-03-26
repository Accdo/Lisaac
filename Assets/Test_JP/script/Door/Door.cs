using Unity.VisualScripting;
using UnityEngine;
using static RoomType;

public abstract class Door : MonoBehaviour
{
    // 메인 카메라 이동 거리 (가로, 세로)
    protected const float PADDINGX = 18;
    protected const float PADDINGY = 10;

	[SerializeField]
    protected Sprite normalOpenDoor; // 일반방 문이 열렸을 때의 스프라이트
	[SerializeField]
	protected Sprite normalCloseDoor; // 일반방 문이 닫혔을 때의 스프라이트
    [SerializeField]
    protected Sprite itemOpenDoor; // 아이템방 문이 열렸을 때의 스프라이트
    [SerializeField]
    protected Sprite itemCloseDoor; // 아이템방 문이 닫혔을 때의 스프라이트
    [SerializeField]
    protected Sprite bossOpenDoor; // 보스방 문이 열렸렸을 때의 스프라이트
    [SerializeField]
    protected Sprite bossCloseDoor; // 보스방 문이 닫혔을 때의 스프라이트

    protected bool isOpen = false;
	protected BoxCollider2D col;
	protected SpriteRenderer spRender;

    protected RoomTypeEnum doorType;

    // 방에 몬스터가 없으면 문을 연다
    protected void DoorCheck()
    {
        isOpen = RoomManager.Instance.nonMonster;

        if(doorType == RoomTypeEnum.Item)
        {
            // �� Ÿ���� �������ϰ��
            if (isOpen)
            {
                col.enabled = true;
                spRender.sprite = itemOpenDoor;
            }
            else
            {
                col.enabled = false;
                spRender.sprite = itemCloseDoor;
            }
        }
        else if (doorType == RoomTypeEnum.Boss)
        {
            // �� Ÿ���� �����ϰ��
            if (isOpen)
            {
                col.enabled = true;
                spRender.sprite = bossOpenDoor;
            }
            else
            {
                col.enabled = false;
                spRender.sprite = bossCloseDoor;
            }
        }
        else
        {
            // �� Ÿ���� �������� �̰ų� �븻�� ���
            if (isOpen)
            {
                col.enabled = true;
                spRender.sprite = normalOpenDoor;
            }
            else
            {
                col.enabled = false;
                spRender.sprite = normalCloseDoor;
            }
        }
    }

    public void DoorTypeSelect(RoomTypeEnum inputDoorType)
    {
        doorType = inputDoorType;
    }

    protected abstract void MapMove(GameObject player);
}
