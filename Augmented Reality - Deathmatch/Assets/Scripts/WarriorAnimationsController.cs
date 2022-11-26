using UnityEngine;
using UnityEngine.EventSystems;

public class WarriorAnimationsController : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    [SerializeField] private Animator _animator;
    [SerializeField] private WarriorController _rotationFlag; 

    private float _timerForAnimationIdle;

    private void Awake()
    {
        CreateSpeedAttack();
    }

    private void Update()
    {
        AnimationRotation();
        AnimationShoting();
        AnimationsIdleEvent();
        DeathAnimation();

        if (Input.GetKeyDown(KeyCode.J))
        {
            _animator.SetBool("Hit", true);
        }
        else
        {
            _animator.SetBool("Hit", false);
        }
    }

    private void CreateSpeedAttack()
    {
        var speed = Random.Range(1, 4);
        _animator.SetFloat("AttackSpeed", speed);
    }

    private void AnimationRotation()
    {
        if (_rotationFlag.PlayngAnimRotationRight)
        {
            _animator.SetBool("RightTurn", true);
        }
        else if(_rotationFlag.PlayngAnimRotationLeft)
        {
            _animator.SetBool("LeftTurn", true);
        }
        else if (!_rotationFlag.PlayngAnimRotationRight && !_rotationFlag.PlayngAnimRotationLeft)
        {
            _animator.SetBool("RightTurn", false);
            _animator.SetBool("LeftTurn", false);
        }
    }

    private void AnimationShoting()
    {
        if (Input.GetKey(KeyCode.Space))
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
        _timerForAnimationIdle += Time.deltaTime;

        if (_timerForAnimationIdle >= 10f)
        {
            _animator.SetBool("IdleEvent", true);
            _timerForAnimationIdle = 0;
        }
        else
        {
            _animator.SetBool("IdleEvent", false);
        }
    }

    private void DeathAnimation()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _animator.SetTrigger("Death");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _animator.SetTrigger("Jump");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }
}
