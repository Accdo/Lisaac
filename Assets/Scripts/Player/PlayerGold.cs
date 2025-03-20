using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public int gold = 0;
    private GoldUI goldUI;

    void Start()
    {
        goldUI = FindAnyObjectByType<GoldUI>();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        goldUI.UpdateGoldUI();
    }

    public void SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
        }
        else
        {
            Debug.Log("골드 부족");
        }
        goldUI.UpdateGoldUI();
    }
}
