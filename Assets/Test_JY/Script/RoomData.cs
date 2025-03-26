using UnityEngine;
using static RoomType;

public class RoomData
{
    public Vector2Int position;
    public Room roomObj;
    public bool[] doors = new bool[4];
    public RoomTypeEnum roomType;
    

    public RoomData(Vector2Int pos)
    {
        position = pos;
    }
}
