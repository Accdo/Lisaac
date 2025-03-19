using UnityEngine;

public class HealingPotion : Item
{
    public int healAmount = 1;

    public override void PickUpItem(GameObject player)
    {
        PlayerHP playerHP = player.GetComponent<PlayerHP>();
        if (playerHP != null)
        {
            playerHP.Healing(healAmount);
        }
        SoundManager.Instance.PickupItem();
        Destroy(gameObject);
    }

}
