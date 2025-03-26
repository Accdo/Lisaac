using UnityEngine;

public class RoomData
{
    public Vector2Int position;
    public Room roomObj;
    public bool[] doors = new bool[4];
	public bool isInPlayer = false;
	public bool playerFirstIn = false;

    public void SetRoomBool()
    {
        isInPlayer = roomObj.isInPlayer;
        playerFirstIn = roomObj.playerFirstIn;
    }

	public RoomData(Vector2Int pos)
    {
        position = pos;
    }
}
