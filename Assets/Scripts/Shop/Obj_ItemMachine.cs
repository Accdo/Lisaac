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

enum Destroy_Reward
{
    Fire = 0,
    Bug
}

public class Obj_ItemMachine : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public Sprite DestroyIM_sprite;
    public GameObject[] Item;
    public Sprite[] sprites;
    public GameObject SmokeEffect;

    [Header("RouletteRollingInfo")]
    public float Slot_Speed = 10;
    public float ItemRollingTime = 1f;
    ItemSlot ItemSlot_1;
    ItemSlot ItemSlot_2;
    ItemSlot ItemSlot_3;
    SpriteRenderer ResultItem_1;
    SpriteRenderer ResultItem_2;
    SpriteRenderer ResultItem_3;
    
    [SerializeField] private Destroy_Reward destroy_Reward;

    // Random Value
    private int Rand;
    int[] Result_ItemValue = {0, 0, 0};

    // RouletteCount
    private int rouletteCount = 0;
    private int DestroyCount = 0;

    [Header("RouletteShakeInfo")]
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 10f;
    private Vector3 originalPos;

    // ETC
    private bool IsDestory = false;
    private bool IsRolling = false;

    private PlayerGold playerGold;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        ItemSlot_1 = transform.GetChild(0).GetComponent<ItemSlot>();
        ItemSlot_2 = transform.GetChild(1).GetComponent<ItemSlot>();
        ItemSlot_3 = transform.GetChild(2).GetComponent<ItemSlot>();

        ResultItem_1 = transform.GetChild(3).GetComponent<SpriteRenderer>();
        ResultItem_2 = transform.GetChild(4).GetComponent<SpriteRenderer>();
        ResultItem_3 = transform.GetChild(5).GetComponent<SpriteRenderer>();

        playerGold = FindAnyObjectByType<PlayerGold>();

        originalPos = transform.position;
    }
    
    void Destroy_ItemMachine()
    {
        animator.SetTrigger("Destroy");

        spriteRenderer.sprite = DestroyIM_sprite;
        SoundManager.Instance.SlotExplosion();

        ItemSlot_1.BehindItem();
        ItemSlot_2.BehindItem();
        ItemSlot_3.BehindItem();

        ResultItem_1.enabled = false;
        ResultItem_2.enabled = false;
        ResultItem_3.enabled = false;
    }

    void Reward()
    {
        if(destroy_Reward == Destroy_Reward.Fire)
        {
            GameObject FireItem = Instantiate(Item[3], transform.position, Quaternion.identity);
            FireItem.AddComponent<ShotItem>();
        }
        else if(destroy_Reward == Destroy_Reward.Bug)
        {
            GameObject BugItem = Instantiate(Item[1], transform.position, Quaternion.identity);
            BugItem.AddComponent<ShotItem>();
        }
    }

    IEnumerator ItemRolling(float duration = 1f)
    {
        float time = 0.0f;

        SoundManager.Instance.SlotTouch();

        IsRolling = true;
        animator.SetTrigger("ShutDown");
        playerGold.SpendGold(1);
        
        // Item Enable
        ResultItem_1.enabled = false;
        ResultItem_2.enabled = false;
        ResultItem_3.enabled = false;


        // Rolling Roulette
        while (time < 1.0f)
        {
            time += Time.deltaTime / duration;

            float shakeOffset = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            transform.position = originalPos + new Vector3(0, shakeOffset, 0);

            ItemSlot_1.MoveItem();
            ItemSlot_2.MoveItem();
            ItemSlot_3.MoveItem();

            yield return null;
        }

        // Destory Roulette
        ++DestroyCount;
        if(DestroyCount >= 10)
        {
            Rand = Random.Range(DestroyCount, 20);
            
            // Destory
            if(Rand >= 17)
            {
                IsDestory = true;
                Instantiate(SmokeEffect, transform.position + new Vector3(0,0.25f,0), Quaternion.identity);
                Reward();
                Destroy_ItemMachine();
                yield break;
            }
        }

        // Random Item
        ++rouletteCount;
        if (rouletteCount >= 5)
        {
            Rand = Random.Range(0, 4);
            ResultItem_1.sprite = sprites[Rand];
            Result_ItemValue[0] = Rand;

            ResultItem_2.sprite = sprites[Rand];
            Result_ItemValue[1] = Rand;

            ResultItem_3.sprite = sprites[Rand];
            Result_ItemValue[2] = Rand;

            rouletteCount = 0;
        }
        else
        {
            Rand = Random.Range(0, 4);
            ResultItem_1.sprite = sprites[Rand];
            Result_ItemValue[0] = Rand;

            Rand = Random.Range(0, 4);
            ResultItem_2.sprite = sprites[Rand];
            Result_ItemValue[1] = Rand;
            
            Rand = Random.Range(0, 4);
            ResultItem_3.sprite = sprites[Rand];
            Result_ItemValue[2] = Rand;
        }

        // Result Item
        if(Result_ItemValue[0] == (int)Item_Enum.Coin && Result_ItemValue[1] == (int)Item_Enum.Coin && Result_ItemValue[2] == (int)Item_Enum.Coin)
        {
            GameObject CoinItem = Instantiate(Item[0], transform.position, Quaternion.identity);
            CoinItem.AddComponent<ShotItem>();
        }
        else if(Result_ItemValue[0] == (int)Item_Enum.Bug && Result_ItemValue[1] == (int)Item_Enum.Bug && Result_ItemValue[2] == (int)Item_Enum.Bug)
        {
            GameObject BugItem = Instantiate(Item[1], transform.position, Quaternion.identity);
            BugItem.AddComponent<ShotItem>();
        }
        else if(Result_ItemValue[0] == (int)Item_Enum.Heart && Result_ItemValue[1] == (int)Item_Enum.Heart && Result_ItemValue[2] == (int)Item_Enum.Heart)
        {
            GameObject HeartItem = Instantiate(Item[2], transform.position, Quaternion.identity);
            HeartItem.AddComponent<ShotItem>();
        }
        else if(Result_ItemValue[0] == (int)Item_Enum.Miss && Result_ItemValue[1] == (int)Item_Enum.Miss && Result_ItemValue[2] == (int)Item_Enum.Miss)
        {
            Debug.Log("Miss ");
            Instantiate(SmokeEffect, transform.position + new Vector3(0,0.25f,0), Quaternion.identity);
        }

        ResultItem_1.enabled = true;
        ResultItem_2.enabled = true;  
        ResultItem_3.enabled = true;
        
        SoundManager.Instance.SlotSpawn();

        IsRolling = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(!IsDestory && !IsRolling && playerGold.gold > 0)
                StartCoroutine(ItemRolling(ItemRollingTime));
        }
    }
}