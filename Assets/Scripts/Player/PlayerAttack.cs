using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private ShotType currentShotType = ShotType.Normal; // 현재 공격 타입
    public GameObject bulletPrefab; // 기본 탄환 프리팹
    private PlayerActions actions; // 입력 액션 시스템
    [SerializeField] private PlayerAnimation playerAnimation; // 애니메이션 제어
    [SerializeField] private Transform[] attackPositions; // 0: 위, 1: 오른쪽, 2: 아래, 3: 왼쪽 공격 위치
    [SerializeField] private float fireRate = 0.2f; // 연사 속도
    [SerializeField] private float maxChargeTime = 1.0f; // 최대 차지 시간

    public Transform currentAttckPosition; // 현재 발사 위치
    private float currentAttackRotation;   // 현재 발사 각도
    private Vector2 attackDirection;       // 공격 방향
    private bool isShooting = false;       // 연사 중 여부
    private Coroutine shootingCoroutine;   // 연사 코루틴 참조
    private bool isCharging = false;       // 차징 중 여부
    public float chargeTime = 0f;          // 현재 차지 시간

    public int chargeItemIndex = 1;        // (아이템 슬롯 인덱스 등으로 사용 가능)
    private bool isShootingLocked = false;


    void Awake()
    {
        actions = new PlayerActions(); // 입력 액션 초기화
    }

    void Start()
    {
        // 공격 키를 누르면 차징 시작
        actions.Attack.ClickAttack.performed += ctx => StartCharging();

        // 공격 키를 떼면 차지샷 발사
        actions.Attack.ClickAttack.canceled += ctx => ReleaseChargeShot();
    }

    void Update()
    {
        GetFirePosition(); // 현재 공격 위치 계산

        // 차징 중이면 시간 누적
        Vector2 attackInput = actions.Attack.ClickAttack.ReadValue<Vector2>();

        if (isCharging && attackInput != Vector2.zero)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0, maxChargeTime);
        }

        UpdateAttackPosition(); // 발사 위치 갱신
    }

    void FixedUpdate()
    {
        ReadAttack(); // 공격 방향 처리
    }

    void ReadAttack()
    {
        if (isShootingLocked) return;

        attackDirection = actions.Attack.ClickAttack.ReadValue<Vector2>().normalized;

        if (attackDirection == Vector2.zero)
        {
            playerAnimation.SetBoolAttackTransition(false);
            return;
        }

        playerAnimation.SetBoolAttackTransition(true);
        playerAnimation.SetAttackAnimation(attackDirection); // 애니메이션 방향 지정
    }

    // 연사 시작
    void StartShooting()
    {
        if (!isShooting)
        {
            isShooting = true;
            shootingCoroutine = StartCoroutine(ShootCoroutine());
        }
    }

    // 연사 중단
    void StopShooting()
    {
        isShooting = false;
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    // 연사 루프
    IEnumerator ShootCoroutine()
    {
        while (isShooting)
        {
            FireBullet();
            yield return new WaitForSeconds(fireRate);
        }
    }

    // 탄환 타입 설정
    public void SetShotType(ShotType type)
    {
        currentShotType = type;
    }

    // 탄환 프리팹 변경
    public void SetBulletPrefab(GameObject bullet)
    {
        bulletPrefab = bullet;
    }

    // 탄환 발사 처리
    void FireBullet()
    {
        if (currentAttckPosition != null)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, currentAttackRotation));

            GameObject bullet;

            // 차지샷일 경우 (조건 만족 시)
            if (currentShotType == ShotType.Charged && chargeTime >= maxChargeTime)
            {
                bullet = Instantiate(bulletPrefab, currentAttckPosition.position, rotation);
                SoundManager.Instance.PlayerChargingBulletShoot(); // 차지샷 사운드
            }
            else
            {
                bullet = Instantiate(bulletPrefab, currentAttckPosition.position, rotation);
                SoundManager.Instance.PlayerBullet(); // 일반 발사 사운드
            }

            bullet.GetComponent<PlayerBullet>().Direction = Vector3.up; // 발사 방향 설정 (위 기준)
        }
    }

    // 방향에 따라 발사 위치/회전 계산
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

    // 외부에서 공격 위치 갱신 요청 시 호출
    public void UpdateAttackPosition()
    {
        GetFirePosition();
    }

    // 차지샷 또는 일반 공격 시작
    void StartCharging()
    {
        if (currentShotType == ShotType.Charged)
        {
            if (chargeTime < maxChargeTime)
            {
                isCharging = true;
                playerAnimation.SetBoolChargingTransition(true);
                //chargeTime = 0f; // 아직 완충이 안 된 경우에만 초기화
            }
            // 이미 충전 완료 상태면 애니메이션만 유지
            else
            {
                isCharging = true;
                playerAnimation.SetBoolChargingTransition(true);
                // chargeTime은 그대로 유지
            }
        }
        else
        {
            StartShooting(); // 일반 연사 시작
        }
    }

    // 공격 버튼을 뗐을 때 차지샷 발사
    void ReleaseChargeShot()
    {
        if (currentShotType == ShotType.Charged)
        {
            Debug.Log("챠징중중");

            if (isCharging && chargeTime >= maxChargeTime)
            {
                FireBullet(); // 발사
                Debug.Log("발사 ");
                playerAnimation.SetBoolChargingShoot(); // 발사 애니메이션
                Debug.Log("발사 애니메이션 출력");

                isShootingLocked = true;

                StartCoroutine(ResetChargingShootAnimation()); // 애니메이션 종료 타이머
                isCharging = false;
            }
        }

        playerAnimation.SetBoolChargingTransition(false);
        chargeTime = 0f; // 차징 초기화
        StopShooting();  // 연사 정지 (일반 타입 대비)
    }

    // 차지샷 애니메이션 자동 해제
    IEnumerator ResetChargingShootAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        isShootingLocked = false;
    }

    // 입력 시스템 활성화
    void OnEnable()
    {
        actions.Enable();
    }

    // 입력 시스템 비활성화
    void OnDisable()
    {
        actions.Disable();
    }
}
