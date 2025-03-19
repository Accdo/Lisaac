using System.Collections;
using UnityEngine;

public class ChargedBullet : PlayerBullet
{
    [SerializeField] private float dotDamageInterval = 0.5f; // 도트 데미지 간격
    [SerializeField] private float animationDuration = 10f;
    private bool isDealingDamage = false;
    private Coroutine damageCoroutine;
    private Transform pos;

    [SerializeField] private PlayerAttack playerAttack;


    void Start()
    {

        playerAttack = GameObject.FindAnyObjectByType<PlayerAttack>();

    }


    void Update()
    {

        playerAttack.UpdateAttackPosition();
        pos = playerAttack.currentAttckPosition;



        if (pos != null)
        {
            transform.position = pos.position;
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!isDealingDamage)
            {
                damageCoroutine = StartCoroutine(ApplyDotDamage(collision.GetComponent<EnemyHp>()));
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine); // 적이 나가면 도트 데미지 중지
                damageCoroutine = null;
                isDealingDamage = false;
            }
        }
    }

    IEnumerator ApplyDotDamage(EnemyHp enemy)
    {
        isDealingDamage = true;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            if (enemy == null) yield break; // 적이 사라지면 중지

            enemy.TakeDamage(damage); // 지속 피해 적용

            SoundManager.Instance.BloodHit();
            yield return new WaitForSeconds(dotDamageInterval); // 일정 간격으로 피해 적용
            elapsedTime += dotDamageInterval;
        }

        isDealingDamage = false;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
