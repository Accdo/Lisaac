using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    private const float PADDINGX = 70;
    private const float PADDINGY = 50;

    private GameObject roomManager;
    private Dictionary<Vector2Int, RoomData> roomDatas;
    [SerializeField]
    private GameObject roomUIPrefab;
    [SerializeField]
    private GameObject roomUIParent;

    private Dictionary<RoomData, GameObject> roomUIs = new Dictionary<RoomData, GameObject>();

    void Start()
    {
        roomManager = GameObject.FindGameObjectWithTag("RoomManager");
		roomDatas = roomManager.GetComponent<DungeonGenerator>().GetRooms();
        SpawmRoom();
		roomUIParent.gameObject.SetActive(false);
    }

    void Update()
    {
        KeyEvent();
		FodOfWar();
	}

    private void KeyEvent()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
			roomUIParent.gameObject.SetActive(true);
        }

        if(Input.GetKeyUp(KeyCode.Tab))
        {
			roomUIParent.gameObject.SetActive(false);
        }
    }

    private void SpawmRoom()
    {
        foreach(var roomdata in roomDatas)
        {
            Vector2 pos = new Vector2(roomdata.Key.x * PADDINGX, roomdata.Key.y * PADDINGY);
            GameObject go = Instantiate(roomUIPrefab, roomUIParent.transform);
            go.GetComponent<RectTransform>().anchoredPosition = pos;
			roomUIs.Add(roomdata.Value, go);
        }
    }

    private void FodOfWar()
    {
        foreach(var roomUI in roomUIs)
        {
            Vector2Int roomdataPos;

            roomUI.Key.SetRoomBool();

            if(!roomUI.Key.playerFirstIn)
            {
			    roomUI.Value.GetComponent<Image>().color = new Color(1, 1, 1, 0.0f);
                roomUI.Value.GetComponent<RoomUI>().SetPlayerUI(roomUI.Key.roomType, roomUI.Key.isInPlayer, roomUI.Key.playerFirstIn);
            }
            else if(roomUI.Key.isInPlayer)
            {
			    roomUI.Value.GetComponent<Image>().color = new Color(1, 1, 1, 1.0f);        
                roomUI.Value.GetComponent<RoomUI>().SetPlayerUI(roomUI.Key.roomType, roomUI.Key.isInPlayer, roomUI.Key.playerFirstIn);

				if (roomUI.Key.doors[0])
                {
					roomdataPos = new Vector2Int(roomUI.Key.position.x, roomUI.Key.position.y + 1);
                    roomDatas[roomdataPos].playerFirstIn = true;
					roomUIs[roomDatas[roomdataPos]].GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
				}

				if(roomUI.Key.doors[1])
				{
					roomdataPos = new Vector2Int(roomUI.Key.position.x, roomUI.Key.position.y - 1);
                    roomDatas[roomdataPos].playerFirstIn = true;
					roomUIs[roomDatas[roomdataPos]].GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
				}

                if (roomUI.Key.doors[2])
				{
					roomdataPos = new Vector2Int(roomUI.Key.position.x - 1, roomUI.Key.position.y);
                    roomDatas[roomdataPos].playerFirstIn = true;
					roomUIs[roomDatas[roomdataPos]].GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
				}

                if (roomUI.Key.doors[3])
				{
					roomdataPos = new Vector2Int(roomUI.Key.position.x + 1, roomUI.Key.position.y);
                    roomDatas[roomdataPos].playerFirstIn = true;
					roomUIs[roomDatas[roomdataPos]].GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
				}
			}
            else
            {
			    roomUI.Value.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);        
                roomUI.Value.GetComponent<RoomUI>().SetPlayerUI(roomUI.Key.roomType, roomUI.Key.isInPlayer, roomUI.Key.playerFirstIn);
            }

        }
    }
}
