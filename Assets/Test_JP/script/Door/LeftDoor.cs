using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class LeftDoor : Door
{
	//�÷��̾� �̵��Ÿ�
	private const float PLAYERSTARTX = 0;
	private const float PLAYERSTARTY = 10;

	private Camera mainCam;

	void Start()
	{
		col = GetComponent<BoxCollider2D>();
		spRender = GetComponent<SpriteRenderer>();
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	void Update()
	{
		RoomManager.Instance.CheckMonster();
		DoorCheck();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//���� �������¿��� �÷��̾�� �����ϸ� ����
		if (collision.CompareTag("Player") && isOpen)
		{
			DoorCol(collision.gameObject);
		}
	}

	//ī�޶� �÷��̾� ���̵�
	protected override void DoorCol(GameObject player)
	{
		mainCam.transform.position += new Vector3(PADDINGX, 0, 0);
		player.transform.position += new Vector3(PLAYERSTARTX, PLAYERSTARTY, 0);
	}
}
