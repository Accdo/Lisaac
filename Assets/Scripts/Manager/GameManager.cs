using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerGold playerGold;
    
    void Start()
    {
        playerGold = FindAnyObjectByType<PlayerGold>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            playerGold.AddGold(100);
        }
    }

}
