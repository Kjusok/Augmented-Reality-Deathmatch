using UnityEngine;
using System;

public class WarriorAnimations : MonoBehaviour
{
    private const float MinForSpeedAttackAnim = 0.7f;
    private const float MaxForSpeedAttackAnim = 1.5f;

    [SerializeField] private Animator _animator;
    [SerializeField] private Warrior _animationFlag;

    private readonly int _attackSpeedStateName = Animator.StringToHash("AttackSpeed");
    private readonly int _rightTurnStateName = Animator.StringToHash("RightTurn");
    private readonly int _leftTurnStateName = Animator.StringToHash("LeftTurn");
    private readonly int _shootStateName = Animator.StringToHash("Shoot");
    private readonly int _idleEventStateName = Animator.StringToHash("IdleEvent");
    private readonly int _hitStateName = Animator.StringToHash("Hit");
    private readonly int _deathStateName = Animator.StringToHash("Death");
    private readonly int _jumpStateName = Animator.StringToHash("Jump");


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
        _animator.SetFloat(_attackSpeedStateName, speed);
    }

    private void AnimationRotation()
    {
        if (_animationFlag.PlayngAnimRotationRight)
        {
            _animator.SetBool(_rightTurnStateName, true);
        }
        else if(_animationFlag.PlayngAnimRotationLeft)
        {
            _animator.SetBool(_leftTurnStateName, true);
        }
        else if (!_animationFlag.PlayngAnimRotationRight && !_animationFlag.PlayngAnimRotationLeft)
        {
            _animator.SetBool(_rightTurnStateName, false);
            _animator.SetBool(_leftTurnStateName, false);
        }
    }

    private void AnimationShoting()
    {
        if (_animationFlag.PlayngAnimShooting)
        {
            _animator.SetBool(_shootStateName, true);
        }
        else
        {
            _animator.SetBool(_shootStateName, false);
        }
    }

    private void AnimationsIdleEvent()
    {
        if (_animationFlag.PlayngAnimIdleEvent)
        {
            _animator.SetBool(_idleEventStateName, true);
        }
        else
        {
            _animator.SetBool(_idleEventStateName, false);
        }
    }

    public void AnimationsTakeHit()
    {
        _animator.SetTrigger(_hitStateName);
    }

    public void DeathAnimation()
    {
        _animator.SetTrigger(_deathStateName);
    }

    public void JumpAnimation()
    {
        _animator.SetTrigger(_jumpStateName);
    }
}
