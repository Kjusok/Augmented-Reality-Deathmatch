using UnityEngine;
using System;

public class WarriorAnimations : MonoBehaviour
{
    private const float MinForSpeedAttackAnim = 0.7f;
    private const float MaxForSpeedAttackAnim = 1.5f;

    [SerializeField] private Animator _animator;

    private readonly int _attackSpeedStateName = Animator.StringToHash("AttackSpeed");
    private readonly int _rightTurnStateName = Animator.StringToHash("RightTurn");
    private readonly int _leftTurnStateName = Animator.StringToHash("LeftTurn");
    private readonly int _shootStateName = Animator.StringToHash("Shoot");
    private readonly int _hitStateName = Animator.StringToHash("Hit");
    private readonly int _deathStateName = Animator.StringToHash("Death");
    private readonly int _jumpStateName = Animator.StringToHash("Jump");


    private void Awake()
    {
        CreateSpeedAttack();
    }

    private void CreateSpeedAttack()
    {
        var speed = (float) Math.Round(UnityEngine.Random.Range(MinForSpeedAttackAnim, MaxForSpeedAttackAnim),2);
        _animator.SetFloat(_attackSpeedStateName, speed);
    }

    public void CheckAnimationRotation(bool isRightTurn, bool isLeftTurn)
    {
        if (isRightTurn)
        {
            _animator.SetBool(_rightTurnStateName, true);
        }
        else if (isLeftTurn)
        {
            _animator.SetBool(_leftTurnStateName, true);
        }
        else if (!isRightTurn && !isLeftTurn)
        {
            _animator.SetBool(_rightTurnStateName, false);
            _animator.SetBool(_leftTurnStateName, false);
        }
    }

    public void CheckAnimationShoting(bool isShoting)
    {
        if (isShoting)
        {
            _animator.SetBool(_shootStateName, true);
        }
        else
        {
            _animator.SetBool(_shootStateName, false);
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
