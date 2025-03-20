using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Sprite openDoor;
	[SerializeField]
	private Sprite closeDoor;

	private bool isOpen = false;
    private BoxCollider2D col;
    private SpriteRenderer spRender;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
		spRender = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        RoomManager.Instance.CheckMonster();
        DoorCheck();
	}

    private void DoorCheck()
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
}
