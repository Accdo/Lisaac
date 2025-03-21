using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorLocation = 0;
    DungeonGenerator dg;
    void Start()
    {
        dg = GetComponentInParent<DungeonGenerator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2Int playerLocation = collision.gameObject.GetComponent<PlayerLocation>().PlayerLocationGet();
        Vector2Int aa = new Vector2Int(0, 1);
        Debug.Log(playerLocation.x + " : " + playerLocation.y);
        RoomData roomData = dg.RoomInfo(playerLocation);
        
        switch(doorLocation)
        {
            case 0:
                Debug.Log(0);
                break;
            case 1:
                Debug.Log(1);
                break;
            case 2:
                Debug.Log(2);
                break;
            case 3:
                Debug.Log(3);
                break;
        }
    }
}
