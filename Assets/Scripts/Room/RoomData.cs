using UnityEngine;
using static RoomType;

public class RoomData
{
    public Vector2Int position;
    public Room roomObj;
    public bool[] doors = new bool[4];
    public RoomTypeEnum roomType;
    
	public bool isInPlayer = false;
	public bool playerFirstIn = false;

    public void SetRoomBool()
    {
        if(roomType == RoomType.RoomTypeEnum.Start)
        {
            playerFirstIn = true;
        }

        isInPlayer = roomObj.isInPlayer;
    }

	public RoomData(Vector2Int pos)
    {
        position = pos;
    }
}
