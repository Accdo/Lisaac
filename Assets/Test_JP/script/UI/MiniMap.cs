using System.Collections.Generic;
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
            roomUI.Key.SetRoomBool();

            if(!roomUI.Key.playerFirstIn)
            {
			    roomUI.Value.GetComponent<Image>().color = new Color(1, 1, 1, 0.0f);        
            }
            else if(roomUI.Key.isInPlayer)
            {
			    roomUI.Value.GetComponent<Image>().color = new Color(1, 1, 1, 1.0f);        
            }
            else
            {
			    roomUI.Value.GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);        
            }

        }
    }
}
