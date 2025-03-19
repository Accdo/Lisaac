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



    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlayerBullet()
    {
        myAudio.PlayOneShot(bulletSound);
    }
    public void PlayerBulletHeavy()
    {
        myAudio.PlayOneShot(heavyBulletSound);
    }
    public void HitDamage()

    {
        myAudio.PlayOneShot(hitDamageSound);
    }
    public void HitBoss()
    {
        myAudio.PlayOneShot(hitBoss);
    }

    public void PickupItem()
    {
        myAudio.PlayOneShot(pickupItem);
    }
    public void Explosion()
    {
        myAudio.PlayOneShot(explosion);
    }

    public void BloodShoot()
    {
        myAudio.PlayOneShot(bloodShoot);
    }

    public void DieSound()
    {
        myAudio.PlayOneShot(die);
    }

    public void PickupCoin()
    {
        myAudio.PlayOneShot(pickupCoin);
    }

    public void Step()
    {
        myAudio.PlayOneShot(step);
    }
    public void PlayerHert()
    {
        myAudio.PlayOneShot(playerHurt);
    }
    public void PlayerChargingBulletShoot()
    {
        myAudio.PlayOneShot(playerChargingBulletShoot);
    }

    public void BloodHit()
    {
        myAudio.PlayOneShot(bloodHit);

    }


}
