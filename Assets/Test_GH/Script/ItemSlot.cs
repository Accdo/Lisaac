using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public float Slot_Speed = 10;

    public void MoveItem()
    {
        transform.localPosition += Vector3.down * Slot_Speed * Time.deltaTime;

        if(transform.localPosition.y <= -0.95f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -0.331f, 0);
        }
    }

    public void BehindItem()
    {
        this.gameObject.SetActive(false);
    }
}
