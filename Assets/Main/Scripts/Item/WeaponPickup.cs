using UnityEngine;

public class WeaponPickup : Item
{
    [SerializeField] private WeaponItem weaponData;

    public override void PickUpItem(GameObject player)
    {
        // 플레이어의 애니메이터 변경
        ChangeBody changeBody = player.GetComponent<ChangeBody>();
        if (changeBody != null)
        {
            changeBody.ChangeAnimators(weaponData.bodyAnimatorController, weaponData.headAnimatorController);
        }

        // 플레이어의 공격 설정 변경
        PlayerAttack playerAttack = player.GetComponentInChildren<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.SetShotType(weaponData.shotType); //  차징 여부 설정
            playerAttack.SetBulletPrefab(weaponData.bulletPrefab); //  탄환 변경

        }

        Destroy(gameObject);
    }
}
