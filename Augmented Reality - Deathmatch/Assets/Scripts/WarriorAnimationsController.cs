using UnityEngine;
using System;

public class WarriorAnimationsController : MonoBehaviour
{
    private const float MinForSpeedAttackAnim = 0.7f;
    private const float MaxForSpeedAttackAnim = 1.5f;

    [SerializeField] private Animator _animator;
    [SerializeField] private WarriorController _animationFlag; 


    private void Awake()
    {
        CreateSpeedAttack();
    }

    private void Update()
    {
        AnimationRotation();
        AnimationShoting();
        AnimationsIdleEvent();
    }

    private void CreateSpeedAttack()
    {
        var speed = (float) Math.Round(UnityEngine.Random.Range(MinForSpeedAttackAnim, MaxForSpeedAttackAnim),2);
        _animator.SetFloat("AttackSpeed", speed);
    }

    private void AnimationRotation()
    {
        if (_animationFlag.PlayngAnimRotationRight)
        {
            _animator.SetBool("RightTurn", true);
        }
        else if(_animationFlag.PlayngAnimRotationLeft)
        {
            _animator.SetBool("LeftTurn", true);
        }
        else if (!_animationFlag.PlayngAnimRotationRight && !_animationFlag.PlayngAnimRotationLeft)
        {
            _animator.SetBool("RightTurn", false);
            _animator.SetBool("LeftTurn", false);
        }
    }

    private void AnimationShoting()
    {
        if (_animationFlag.PlayngAnimShooting)
        {
            _animator.SetBool("Shoot", true);
        }
        else
        {
            _animator.SetBool("Shoot", false);
        }
    }

    private void AnimationsIdleEvent()
    {
        if (_animationFlag.PlayngAnimIdleEvent)
        {
            _animator.SetBool("IdleEvent", true);
        }
        else
        {
            _animator.SetBool("IdleEvent", false);
        }
    }

    public void AnimationsTakeHit()
    {
        _animator.SetTrigger("Hit");
    }

    public void DeathAnimation()
    {
        _animator.SetTrigger("Death");
    }

    public void JumpAnimation()
    {
        _animator.SetTrigger("Jump");
    }
}
