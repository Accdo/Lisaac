using UnityEngine;
public enum ShotType
{
    Normal, // 일반 샷
    Charged // 차징 샷
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponItem : ScriptableObject
{
    public string weaponName;
    public RuntimeAnimatorController bodyAnimatorController;
    public RuntimeAnimatorController headAnimatorController;
    public ShotType shotType;
    public GameObject bulletPrefab;
}
