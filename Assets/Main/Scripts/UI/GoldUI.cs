using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldTxet;
    private PlayerGold playerGold;

    void Start()
    {
        playerGold = FindAnyObjectByType<PlayerGold>();
        UpdateGoldUI();
    }

    public void UpdateGoldUI()
    {
        goldTxet.text = $"X {playerGold.gold}";
    }
}
