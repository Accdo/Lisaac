using UnityEngine;

public class Gold : Item
{
    public int goldAmount = 1;
    public override void PickUpItem(GameObject player)
    {
        PlayerGold playerGold = player.GetComponent<PlayerGold>();

        if (playerGold != null)
        {
            playerGold.AddGold(goldAmount);
        }
        SoundManager.Instance.PickupCoin();
        Destroy(gameObject);
    }



}
