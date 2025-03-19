using UnityEngine;

public class EnemyShrink : MonoBehaviour
{
    public Sprite[] shrinkSprites;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void UpdateSprite(int damageStage)
    {

        if (damageStage < shrinkSprites.Length)
        {
            spriteRenderer.sprite = shrinkSprites[damageStage];
        }
    }


}
