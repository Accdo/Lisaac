using UnityEngine;

public class UpDoor : Door
{
	//플레이어 이동거리
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
		//문이 열린상태에서 플레이어와 접촉하면 실행
		if(collision.CompareTag("Player") && isOpen)
		{
			MapMove(collision.gameObject);
		}
	}

	//카메라 플레이어 맵이동
	protected override void MapMove(GameObject player)
	{
		mainCam.transform.position += new Vector3(0, PADDINGY, 0);
		player.transform.position += new Vector3(PLAYERSTARTX, PLAYERSTARTY, 0);
	}
}
