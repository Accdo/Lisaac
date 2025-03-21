using UnityEngine;

public class UpDoor : Door
{
	//�÷��̾� �̵��Ÿ�
	private const float PLAYERSTARTX = 0;  
	private const float PLAYERSTARTY = 3.5f;  

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
		if(collision.CompareTag("Player") && isOpen)
		{
			MapMove(collision.gameObject);
		}
	}

	//ī�޶� �÷��̾� ���̵�
	protected override void MapMove(GameObject player)
	{
		mainCam.transform.position += new Vector3(0, PADDINGY, 0);
		player.transform.position += new Vector3(PLAYERSTARTX, PLAYERSTARTY, 0);
	}
}
