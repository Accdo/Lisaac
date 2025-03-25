using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class RightDoor : Door
{
	// 플레이어가 오른쪽방으로 이동할 때의 시작 위치
	private const float PLAYERSTARTX = 5.5f;
	private const float PLAYERSTARTY = 0;

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

	// 맵 오른쪽으로 이동
	protected override void MapMove(GameObject player)
	{
		mainCam.transform.position += new Vector3(PADDINGX, 0, 0);
		player.transform.position += new Vector3(PLAYERSTARTX, PLAYERSTARTY, 0);
	}
}
