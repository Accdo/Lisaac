using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerActions actions;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private Transform[] attackPositions;
    [SerializeField] private PlayerBullet[] playerBullet;
    [SerializeField] private float fireRate = 0.2f;
    private int currentBulletIndex = 0;


    private Transform currentAttckPosition;
    private float currentAttackRotation;
    private Vector2 attackDirection;
    private bool isShooting = false;
    private Coroutine shootingCoroutine;



    void Awake()
    {
        actions = new PlayerActions();

    }

    void Start()
    {
        actions.Attack.ClickAttack.performed += ctx => StartShooting();
        actions.Attack.ClickAttack.canceled += ctx => StopShooting();
    }

    void Update()
    {
        GetFirePosition();
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


    void FireBullet()
    {
        if (currentAttckPosition != null)
        {
            Quaternion rotation =
                Quaternion.Euler(new Vector3(0f, 0f, currentAttackRotation));
            PlayerBullet pBullet =
                Instantiate(playerBullet[currentBulletIndex], currentAttckPosition.position, rotation);
            SoundManager.Instance.PlayerBullet();
            pBullet.Direction = Vector3.up;
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


    void OnEnable()
    {
        actions.Enable();
    }

    void OnDisable()
    {
        actions.Disable();
    }
}
