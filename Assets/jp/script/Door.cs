using Unity.VisualScripting;
using UnityEngine;

public abstract class Door : MonoBehaviour
{
	protected const float PADDINGX = 100;
	protected const float PADDINGY = 100;

	[SerializeField]
    protected Sprite openDoor;
	[SerializeField]
	protected Sprite closeDoor;

	protected bool isOpen = false;
	protected BoxCollider2D col;
	protected SpriteRenderer spRender;

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

    protected abstract void DoorCol(GameObject player);
}
