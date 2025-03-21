using Unity.VisualScripting;
using UnityEngine;

public abstract class Door : MonoBehaviour
{
    //메인카메라 이동거리
	protected const float PADDINGX = 18;
	protected const float PADDINGY = 10;

	[SerializeField]
    protected Sprite openDoor;
	[SerializeField]
	protected Sprite closeDoor;

	protected bool isOpen = false;
	protected BoxCollider2D col;
	protected SpriteRenderer spRender;

    //몬스터유무 체크해서 문열린상태 체크
	protected void DoorCheck()
    {
        isOpen = RoomManager.Instance.nonMonster;

        if(isOpen)
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

    protected abstract void MapMove(GameObject player);
}
