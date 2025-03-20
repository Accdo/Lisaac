using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private ShotType currentShotType = ShotType.Normal;
    public GameObject bulletPrefab;
    private PlayerActions actions;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private Transform[] attackPositions;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float maxChargeTime = 1.0f;


    public Transform currentAttckPosition;
    private float currentAttackRotation;
    private Vector2 attackDirection;
    private bool isShooting = false;
    private Coroutine shootingCoroutine;
    private bool isCharging = false;
    public float chargeTime = 0f;

    public int chargeItemIndex = 1;



    void Awake()
    {
        actions = new PlayerActions();

    }

    void Start()
    {
        actions.Attack.ClickAttack.performed += ctx => StartCharging();

        actions.Attack.ClickAttack.canceled += ctx => ReleaseChargeShot();

    }

    void Update()
    {
        GetFirePosition();

        if (isCharging)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0, maxChargeTime);
        }
        UpdateAttackPosition();
    }

    void FixedUpdate()
    {

        ReadAttack();
    }



    void ReadAttack()
    {
        attackDirection = actions.Attack.ClickAttack.ReadValue<Vector2>().normalized;
        if (attackDirection == Vector2.zero)
        {
            playerAnimation.SetBoolAttackTransition(false);
            return;
        }

        playerAnimation.SetBoolAttackTransition(true);
        playerAnimation.SetAttackAnimation(attackDirection);

    }
    void StartShooting()
    {
        if (!isShooting)
        {
            isShooting = true;
            shootingCoroutine = StartCoroutine(ShootCoroutine());
        }
    }

    void StopShooting()
    {
        isShooting = false;
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    IEnumerator ShootCoroutine()
    {
        while (isShooting)
        {
            FireBullet();
            yield return new WaitForSeconds(fireRate);
        }

    }


    public void SetShotType(ShotType type)
    {
        currentShotType = type;
    }
    public void SetBulletPrefab(GameObject bullet)
    {
        bulletPrefab = bullet;
    }

    void FireBullet()
    {
        if (currentAttckPosition != null)
        {

            Quaternion rotation =
                Quaternion.Euler(new Vector3(0f, 0f, currentAttackRotation));

            GameObject bullet;

            if (currentShotType == ShotType.Charged && chargeTime >= maxChargeTime)
            {

                bullet =
                Instantiate(bulletPrefab, currentAttckPosition.position, rotation);

                SoundManager.Instance.PlayerChargingBulletShoot();

            }
            else
            {
                bullet =
                Instantiate(bulletPrefab, currentAttckPosition.position, rotation);
                SoundManager.Instance.PlayerBullet();
            }




            bullet.GetComponent<PlayerBullet>().Direction = Vector3.up;
        }
    }

    void GetFirePosition()
    {
        switch (attackDirection.x)
        {
            case > 0f:
                currentAttckPosition = attackPositions[1];
                currentAttackRotation = -90f;
                break;
            case < 0f:
                currentAttckPosition = attackPositions[3];
                currentAttackRotation = -270f;
                break;
        }
        switch (attackDirection.y)
        {
            case > 0f:
                currentAttckPosition = attackPositions[0];
                currentAttackRotation = 0f;
                break;
            case < 0f:
                currentAttckPosition = attackPositions[2];
                currentAttackRotation = -180f;
                break;
        }
    }
    public void UpdateAttackPosition()
    {
        GetFirePosition(); // 현재 공격 위치를 업데이트
    }

    void StartCharging()
    {
        if (currentShotType == ShotType.Charged)
        {
            if (!isCharging)
            {
                isCharging = true;
                chargeTime = 0f;
                playerAnimation.SetBoolChargingTransition(true);
            }

        }
        else
        {
            StartShooting();
        }

    }
    void ReleaseChargeShot()
    {

        if (currentShotType == ShotType.Charged)
        {
            Debug.Log("챠징중중");
            if (isCharging && chargeTime >= maxChargeTime)
            {
                FireBullet();
                Debug.Log("발사 ");
                playerAnimation.SetBoolChargingShoot();
                Debug.Log("발사 애니메이션 출력");
                StartCoroutine(ResetChargingShootAnimation());

                isCharging = false;
            }

        }


        playerAnimation.SetBoolChargingTransition(false);
        chargeTime = 0f; // 차징 초기화

        StopShooting();


    }
    IEnumerator ResetChargingShootAnimation()
    {
        yield return new WaitForSeconds(2.0f); // 차징샷 발사 후 2초 후 자동으로 끄기

    }




    void OnEnable()
    {
        actions.Enable();

    }

    void OnDisable()
    {
        actions.Disable();
    }
}
