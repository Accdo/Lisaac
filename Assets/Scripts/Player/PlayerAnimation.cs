using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private readonly int moveX = Animator.StringToHash("MoveX");
    private readonly int moveY = Animator.StringToHash("MoveY");
    private readonly int moving = Animator.StringToHash("Moving");
    private readonly int die = Animator.StringToHash("Die");
    private readonly int revive = Animator.StringToHash("Revive");
    private readonly int attackX = Animator.StringToHash("AttackX");
    private readonly int attackY = Animator.StringToHash("AttackY");
    private readonly int attacking = Animator.StringToHash("Attack");
    private readonly int charging = Animator.StringToHash("Charging");
    private readonly int chargingShoot = Animator.StringToHash("ChargingShoot");

    private Animator bodyAnimator;
    private Animator headAnimator;

    void Awake()
    {
        bodyAnimator = GetComponent<Animator>();
        headAnimator = transform.Find("Head").GetComponent<Animator>(); // 자식 오브젝트인 Head의 애니메이터 가져오기
    }
    public void SetDieAnimation()
    {
        bodyAnimator.SetTrigger(die);
        headAnimator.SetTrigger(die);
    }

    public void SetMoveBoolTransition(bool value)
    {
        bodyAnimator.SetBool(moving, value);
        // headAnimator.SetBool(moving, value);
    }

    public void SetMoveAnimation(Vector2 dir)
    {
        bodyAnimator.SetFloat(moveX, dir.x);
        bodyAnimator.SetFloat(moveY, dir.y);
        headAnimator.SetFloat(moveX, dir.x);
        headAnimator.SetFloat(moveY, dir.y);
    }
    public void SetBoolAttackTransition(bool value)
    {
        headAnimator.SetBool(attacking, value);
    }
    public void SetAttackAnimation(Vector2 dir)
    {
        headAnimator.SetFloat(attackX, dir.x);
        headAnimator.SetFloat(attackY, dir.y);

    }

    public void SetBoolChargingTransition(bool value)
    {
        headAnimator.SetBool(charging, value);
    }
    public void SetBoolChargingShoot(bool value)
    {
        headAnimator.SetBool(chargingShoot, value);
    }




    public void ResetPlayer()
    {
        SetMoveAnimation(Vector2.down);
        bodyAnimator.SetTrigger(revive);
        headAnimator.SetTrigger(revive);
    }
}
