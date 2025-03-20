using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthUI : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartContainer;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private List<GameObject> hearts = new List<GameObject>();

    public void UpdateHearts(int currentHp, int maxHp)
    {
        foreach (GameObject heart in hearts)
        {
            Destroy(heart);
        }
        hearts.Clear();

        int heartCount = Mathf.CeilToInt(maxHp / 2f);

        for (int i = 0; i < heartCount; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(heart);
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            Image heartImage = hearts[i].GetComponent<Image>();

            if (currentHp >= (i + 1) * 2)
            {
                heartImage.sprite = fullHeart; // 꽉 찬 하트
            }
            else if (currentHp > i * 2)
            {
                heartImage.sprite = halfHeart; // 반 하트
            }
            else
            {
                heartImage.sprite = emptyHeart; // 빈 하트
            }
        }
    }

}
