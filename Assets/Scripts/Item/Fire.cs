using System.Collections;
using UnityEngine;

public class Fire : Item
{
    [Header("ÄÄÆ÷³ÍÆ®")]
    private SpriteRenderer sr;
    private Animator ani;

    [Header("³»ºÎ º¯¼ö")]
    public int color;
    public float lifeTime = 100f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            switch (ani.GetInteger("Color"))
            {
                case 0:
                    Debug.Log("ÁÖÈ²");
                    break;
                case 1:
                    Debug.Log("ÆÄ¶û");
                    break;
                case 2:
                    Debug.Log("³²»ö");
                    break;
                case 3:
                    Debug.Log("º¸¶ó");
                    break;
                case 4:
                    Debug.Log("ÇÏ¾ç");
                    break;
                case 5:
                    Debug.Log("»¡°­");
                    break;
            }
        }
    }

    public override void PickUpItem(GameObject player)
    {
        // ¾ÆÀÌÅÛ È¹µæ
        SoundManager.Instance.PickupItem();
        transform.SetParent(player.transform);
        transform.localPosition = new Vector3(0f, 1f, 0f);
        transform.localScale *= .8f;

        StartCoroutine(FireAttack());
    }

    IEnumerator FireAttack()
    {
        color = Random.Range(0, 6);

        while (true)
        {
            ani.SetInteger("Color", color++ % 6);

            yield return new WaitForSeconds(1f);
        }
    }
}
