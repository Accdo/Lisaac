using UnityEngine;

public class ChangeBody : MonoBehaviour
{
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private Animator headAnimator;

    public void ChangeAnimators(RuntimeAnimatorController newBodyAnimator, RuntimeAnimatorController newHeadAnimator)
    {
        if (newBodyAnimator != null)
        {
            bodyAnimator.runtimeAnimatorController = newBodyAnimator;

        }

        if (newHeadAnimator != null)
        {
            headAnimator.runtimeAnimatorController = newHeadAnimator;

        }
    }
}
