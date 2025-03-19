using UnityEngine;


public class Item : MonoBehaviour
{
    public virtual void PickUpItem(GameObject player)
    {
        Destroy(gameObject);
    }

    public virtual void UseItem()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickUpItem(collision.gameObject);
        }
    }
}