using UnityEngine;

public class RoomTestManager : MonoBehaviour
{
    public static RoomTestManager Instance;

    public Transform player_Pos;
    public Transform camera_Pos;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextRoom(string dir)
    {
        if(dir == "Left")
        {
            player_Pos.position = new Vector3(player_Pos.position.x - 5.5f, player_Pos.position.y, player_Pos.position.z);
            camera_Pos.position = new Vector3(camera_Pos.position.x - 18f, camera_Pos.position.y, camera_Pos.position.z);
        }
        else if(dir == "Right")
        {
            player_Pos.position = new Vector3(player_Pos.position.x + 5.5f, player_Pos.position.y, player_Pos.position.z);
            camera_Pos.position = new Vector3(camera_Pos.position.x + 18f, camera_Pos.position.y, camera_Pos.position.z);
        }
        else if(dir == "Up")
        {
            player_Pos.position = new Vector3(player_Pos.position.x, player_Pos.position.y + 4.5f, player_Pos.position.z);
            camera_Pos.position = new Vector3(camera_Pos.position.x, camera_Pos.position.y + 10.5f, camera_Pos.position.z);
        }
        else if(dir == "Down")
        {
            player_Pos.position = new Vector3(player_Pos.position.x, player_Pos.position.y - 4.5f, player_Pos.position.z);
            camera_Pos.position = new Vector3(camera_Pos.position.x, camera_Pos.position.y - 10.5f, camera_Pos.position.z);
        }
        else
        {
            Debug.Log(dir + " is not find");
        }
    }
}
