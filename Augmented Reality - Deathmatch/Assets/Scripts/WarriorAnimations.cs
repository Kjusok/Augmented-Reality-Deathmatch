using UnityEngine;

public class WarriorAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private readonly int _attackSpeedStateName = Animator.StringToHash("AttackSpeed");
    private readonly int _hitStateName = Animator.StringToHash("Hit");
    private readonly int _deathStateName = Animator.StringToHash("Death");
    private readonly int _jumpStateName = Animator.StringToHash("Jump");
    private readonly int _rotationStateName = Animator.StringToHash("Rotation");
    private readonly int _shotStateName = Animator.StringToHash("Shot");


    public void CreateSpeedAttack(float speed)
    {
        _animator.SetFloat(_attackSpeedStateName, speed);
    }

    public void PlayRotation(float rotation)
    {
        _animator.SetFloat(_rotationStateName, rotation);
    }

    public void PlayShot()
    {
        _animator.SetTrigger(_shotStateName);
    }
   
    public void PlayTakeHit()
    {
        _animator.SetTrigger(_hitStateName);
    }

    public void PlayDeath()
    {
        _animator.SetTrigger(_deathStateName);
    }

    public void PlayJump()
    {
        _animator.SetTrigger(_jumpStateName);
    }
}
