using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimator : MonoBehaviour
{
    public readonly int IdleAnimationHash = Animator.StringToHash("Idle");
    public readonly int AttackAnimationHash = Animator.StringToHash("Attack");

    [SerializeField]
    private Animator animator;

    public UnityEvent OnAttackHitAnimationEvent;

    private void Start()
    {
        StartIdleAnimation();
    }

    public void StartIdleAnimation()
    {
        animator.Play(IdleAnimationHash);
    }

    public void StartAttackAnimation()
    {
        animator.Play(AttackAnimationHash);
    }

    public void AttackAnimationHitCallback()
    {
        OnAttackHitAnimationEvent?.Invoke();
    }
}
