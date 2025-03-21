using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private static RoomManager instance;
    public static RoomManager Instance { get { return instance; } }

    public bool nonMonster { get; private set; }

	private void Awake()
	{
		if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}

    //���� ����üũ
    public void CheckMonster()
    {
        if(GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            nonMonster = true;
        }
        else
        {
            nonMonster = false;
        }
    }
}
