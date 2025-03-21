using Unity.VisualScripting;
using UnityEngine;

public abstract class Door : MonoBehaviour
{
    //����ī�޶� �̵��Ÿ�
	protected const float PADDINGX = 18;
	protected const float PADDINGY = 10;

	[SerializeField]
    protected Sprite openDoor;
	[SerializeField]
	protected Sprite closeDoor;

	protected bool isOpen = false;
	protected BoxCollider2D col;
	protected SpriteRenderer spRender;

    //�������� üũ�ؼ� ���������� üũ
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
