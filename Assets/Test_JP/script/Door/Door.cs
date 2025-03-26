using Unity.VisualScripting;
using UnityEngine;
using static RoomType;

public abstract class Door : MonoBehaviour
{
    //메인카메라 이동거리
	protected const float PADDINGX = 18;
	protected const float PADDINGY = 10;

	[SerializeField]
    protected Sprite normalOpenDoor;
	[SerializeField]
	protected Sprite normalCloseDoor;
    [SerializeField]
    protected Sprite itemOpenDoor;
    [SerializeField]
    protected Sprite itemCloseDoor;
    [SerializeField]
    protected Sprite bossOpenDoor;
    [SerializeField]
    protected Sprite bossCloseDoor;

    protected bool isOpen = false;
	protected BoxCollider2D col;
	protected SpriteRenderer spRender;

    protected RoomTypeEnum doorType;

    //몬스터유무 체크해서 문열린상태 체크
    protected void DoorCheck()
    {
        isOpen = RoomManager.Instance.nonMonster;

        if(doorType == RoomTypeEnum.Item)
        {
            // 문 타입이 아이템일경우
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
            // 문 타입이 보스일경우
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
            // 문 타입이 시작지점 이거나 노말일 경우
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
