using UnityEngine;

public class RoomData
{
    public Vector2Int position;
    public Room roomObj;
    public bool[] doors = new bool[4];

    public RoomData(Vector2Int pos)
    {
        position = pos;
    }
}
