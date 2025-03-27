using UnityEngine;

public class UpDoor : Door
{
	// 플레이어가 위쪽방으로 이동할 때의 시작 위치
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
		if (collision.CompareTag("Player") && isOpen)
		{
			MapMove(collision.gameObject);
		}
	}

	// 맵 위쪽으로 이동
	protected override void MapMove(GameObject player)
	{
		mainCam.transform.position += new Vector3(0, PADDINGY, 0);
		player.transform.position += new Vector3(PLAYERSTARTX, PLAYERSTARTY, 0);
	}
}
