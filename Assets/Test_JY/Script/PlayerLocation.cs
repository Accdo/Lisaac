using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public int playerX = 0;
    public int playerY = 0;
    public Vector2Int playerXY = new Vector2Int();

    public void Start()
    {
        playerXY.x = 0;
        playerXY.y = 0;
    }

    public Vector2Int PlayerLocationGet()
    {
        return playerXY;
    }

    public void PlayerLocationSet(Vector2Int location)
    {
        playerXY = location;
    }

}
