using UnityEngine;

public class DownDoor : Door
{
	// 플레이어가 아래방으로 이동할 때의 시작 위치
	private const float PLAYERSTARTX = 0;
	private const float PLAYERSTARTY = -3.5f;

	private Camera mainCam;

	void Start()
	{
		// 필요한 컴포넌트 가져오기
		col = GetComponent<BoxCollider2D>();
		spRender = GetComponent<SpriteRenderer>();
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	void Update()
	{
		RoomManager.Instance.CheckMonster(); // 몬스터 유무 확인
		DoorCheck();                         // 문 상태 확인
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 문이 열렸고, 플레이어가 닿았을 때 이동
		if (collision.CompareTag("Player") && isOpen)
		{
			MapMove(collision.gameObject);
		}
	}

	// 맵 아래로 이동
	protected override void MapMove(GameObject player)
	{
		mainCam.transform.position += new Vector3(0, -PADDINGY, 0); // 카메라 이동
		player.transform.position += new Vector3(PLAYERSTARTX, PLAYERSTARTY, 0); // 플레이어 위치 이동
	}
}
