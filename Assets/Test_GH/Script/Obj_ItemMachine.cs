using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

enum Item_Enum
{
    Coin = 0,
    Bug = 1,
    Heart = 2,
    Miss = 3
}

public class Obj_ItemMachine : MonoBehaviour
{
    public GameObject[] Item;

    public Sprite[] sprites;

    GameObject ItemSlot_1;
    GameObject ItemSlot_2;
    GameObject ItemSlot_3;

    SpriteRenderer ResultItem_1;
    SpriteRenderer ResultItem_2;
    SpriteRenderer ResultItem_3;
    public float Slot_Speed = 10;
    public float ItemRollingTime = 1f;

    int Rand; // Random Value

    int[] Result_ItemValue = {0, 0, 0};

    void Start()
    {
        ItemSlot_1 = transform.Find("slot_1").gameObject;
        ItemSlot_2 = transform.Find("slot_2").gameObject;
        ItemSlot_3 = transform.Find("slot_3").gameObject;

        ResultItem_1 = transform.GetChild(3).GetComponent<SpriteRenderer>();
        ResultItem_2 = transform.GetChild(4).GetComponent<SpriteRenderer>();
        ResultItem_3 = transform.GetChild(5).GetComponent<SpriteRenderer>();
    }

    IEnumerator ItemRolling(float duration = 1f)
    {
        float time = 0.0f;
        
        // Item Enable
        ResultItem_1.enabled = false;
        ResultItem_2.enabled = false;  
        ResultItem_3.enabled = false;

        while (time < 1.0f)
        {
            time += Time.deltaTime / duration;
            
            // Move Item in ItemSlot
            ItemSlot_1.transform.localPosition += Vector3.down * Slot_Speed * Time.deltaTime;
            ItemSlot_2.transform.localPosition += Vector3.down * Slot_Speed * Time.deltaTime;
            ItemSlot_3.transform.localPosition += Vector3.down * Slot_Speed * Time.deltaTime;

            // Return Item in ItemSlot
            if(ItemSlot_1.transform.localPosition.y <= -0.95f)
            {
                ItemSlot_1.transform.localPosition = new Vector3(ItemSlot_1.transform.localPosition.x, -0.331f, 0);
            }
            if(ItemSlot_2.transform.localPosition.y <= -0.95f)
            {
                ItemSlot_2.transform.localPosition = new Vector3(ItemSlot_2.transform.localPosition.x, -0.331f, 0);
            }
            if(ItemSlot_3.transform.localPosition.y <= -0.95f)
            {
                ItemSlot_3.transform.localPosition = new Vector3(ItemSlot_3.transform.localPosition.x, -0.031f, 0);
            }
            yield return null;
        }

        // Random Item Change
        Rand = Random.Range(0, 4);
        ResultItem_1.sprite = sprites[Rand];
        Result_ItemValue[0] = Rand;
        Rand = Random.Range(0, 4);
        ResultItem_2.sprite = sprites[Rand];
        Result_ItemValue[1] = Rand;
        Rand = Random.Range(0, 4);
        ResultItem_3.sprite = sprites[Rand];
        Result_ItemValue[2] = Rand;

        if(Result_ItemValue[0] == (int)Item_Enum.Coin && Result_ItemValue[1] == (int)Item_Enum.Coin && Result_ItemValue[2] == (int)Item_Enum.Coin)
        {
            Debug.Log("Coin " + Result_ItemValue[0] + " " + Result_ItemValue[1] + " " + Result_ItemValue[2]);
            GameObject CoinItem = Instantiate(Item[0], transform.position, Quaternion.identity);
            CoinItem.AddComponent<ShotItem>();
        }
        else if(Result_ItemValue[0] == (int)Item_Enum.Bug && Result_ItemValue[1] == (int)Item_Enum.Bug && Result_ItemValue[2] == (int)Item_Enum.Bug)
        {
            Debug.Log("Bug ");
        }
        else if(Result_ItemValue[0] == (int)Item_Enum.Heart && Result_ItemValue[1] == (int)Item_Enum.Heart && Result_ItemValue[2] == (int)Item_Enum.Heart)
        {
            Debug.Log("Heart " + Result_ItemValue[0] + " " + Result_ItemValue[1] + " " + Result_ItemValue[2]);
            GameObject HeartItem = Instantiate(Item[1], transform.position, Quaternion.identity);
            HeartItem.AddComponent<ShotItem>();
        }
        else if(Result_ItemValue[0] == (int)Item_Enum.Miss && Result_ItemValue[1] == (int)Item_Enum.Miss && Result_ItemValue[2] == (int)Item_Enum.Miss)
        {
            Debug.Log("Miss ");
        }

        ResultItem_1.enabled = true;
        ResultItem_2.enabled = true;  
        ResultItem_3.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            
            StartCoroutine(ItemRolling(ItemRollingTime));
        }
    }


}
