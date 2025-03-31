using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource myAudio;

    [SerializeField] private AudioClip bulletSound;
    [SerializeField] private AudioClip heavyBulletSound;
    [SerializeField] private AudioClip hitDamageSound;
    [SerializeField] private AudioClip hitBoss;
    [SerializeField] private AudioClip pickupItem;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip bloodShoot;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip pickupCoin;
    [SerializeField] private AudioClip step;
    [SerializeField] private AudioClip playerHurt;
    [SerializeField] private AudioClip playerChargingBulletShoot;
    [SerializeField] private AudioClip bloodHit;
    [SerializeField] private AudioClip bossMove;
    [SerializeField] private AudioClip bossAttack;
    [SerializeField] private AudioClip bossJump;
    [SerializeField] private AudioClip fireOn;
    [SerializeField] private AudioClip fireOff;

    [SerializeField] private AudioClip fly_Buzz;
    [SerializeField] private AudioClip slotTouch;
    [SerializeField] private AudioClip slotSpawn;
    [SerializeField] private AudioClip slotExplosion;

    [SerializeField] private AudioClip exoMechTargetSelect;
    [SerializeField] private AudioClip apolloMissileLaunch;
    [SerializeField] private AudioClip artemisShotgunLaser;
    [SerializeField] private AudioClip aresNukeCharge;
    [SerializeField] private AudioClip aresNukeExplosion;
    [SerializeField] private AudioClip aresLaserCharge;
    [SerializeField] private AudioClip aresLaserShot;
    [SerializeField] private AudioClip aresTeslaShot;
    [SerializeField] private AudioClip exoLaserShoot;
    [SerializeField] private AudioClip exoPlasmaShoot;
    [SerializeField] private AudioClip exoDeath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // �ߺ� ���� ����
        }
    }

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }
    public void SoundOnOff()
    {
        myAudio.mute = !myAudio.mute;
    }
    public void SetSoundVolume(float _vlome)
    {
        myAudio.volume = _vlome;
    }

    public void PlayerBullet() { myAudio.PlayOneShot(bulletSound); }
    public void PlayerBulletHeavy() { myAudio.PlayOneShot(heavyBulletSound); }
    public void HitDamage() { myAudio.PlayOneShot(hitDamageSound); }
    public void HitBoss() { myAudio.PlayOneShot(hitBoss); }
    public void PickupItem() { myAudio.PlayOneShot(pickupItem); }
    public void Explosion() { myAudio.PlayOneShot(explosion); }
    public void BloodShoot() { myAudio.PlayOneShot(bloodShoot); }
    public void DieSound() { myAudio.PlayOneShot(die); }
    public void PickupCoin() { myAudio.PlayOneShot(pickupCoin); }
    public void Step() { myAudio.PlayOneShot(step); }
    public void PlayerHert() { myAudio.PlayOneShot(playerHurt); }
    public void PlayerChargingBulletShoot() { myAudio.PlayOneShot(playerChargingBulletShoot); }
    public void BloodHit() { myAudio.PlayOneShot(bloodHit); }

    public void BossMove() { myAudio.PlayOneShot(bossMove); }
    public void BossAttack() { myAudio.PlayOneShot(bossAttack); }
    public void BossJump() { myAudio.PlayOneShot(bossJump); }
    public void Fly_Buzz() { myAudio.PlayOneShot(fly_Buzz); }
    public void SlotTouch() { myAudio.PlayOneShot(slotTouch); }
    public void SlotSpawn() { myAudio.PlayOneShot(slotSpawn); }
    public void SlotExplosion() { myAudio.PlayOneShot(slotExplosion); }

    public void ExoMechTargetSelect() { myAudio.PlayOneShot(exoMechTargetSelect); }
    public void ApolloMissileLaunch() { myAudio.PlayOneShot(apolloMissileLaunch); }
    public void ArtemisShotgunLaser() { myAudio.PlayOneShot(artemisShotgunLaser); }
    public void AresNukeCharge() { myAudio.PlayOneShot(aresNukeCharge); }
    public void AresNukeExplosion() { myAudio.PlayOneShot(aresNukeExplosion); }
    public void AresLaserCharge() { myAudio.PlayOneShot(aresLaserCharge); }
    public void AresLaserShot() { myAudio.PlayOneShot(aresLaserShot); }
    public void AresTeslaShot() { myAudio.PlayOneShot(aresTeslaShot); }
    public void ExoLaserShoot() { myAudio.PlayOneShot(exoLaserShoot); }
    public void ExoPlasmaShoot() { myAudio.PlayOneShot(exoPlasmaShoot); }
    public void ExoDeath() { myAudio.PlayOneShot(exoDeath); }

    public void FireOn() { myAudio.PlayOneShot(fireOn); }
    public void FireOff() { myAudio.PlayOneShot(fireOff); }
}
