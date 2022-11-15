using UnityEngine;
using UnityEngine.EventSystems;

public class WarriorAnimationsController : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Animator _animator;

    private float _timerForAnimationIdle;

    
    private void Update()
    {
        AnimationRotation();
        Shooting();
        AnimationsIdleEvent();
        DeathAnimation();
    }

    private void AnimationRotation()
    {
        var horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(0.0f, horizontal * _speed, 0.0f, Space.World);

        if (horizontal > 0)
        {
            _animator.SetBool("RightTurn", true);
        }
        else
        {
            _animator.SetBool("LeftTurn", true);

        }

        if (horizontal == 0)
        {
            _animator.SetBool("RightTurn", false);
            _animator.SetBool("LeftTurn", false);

        }
    }

    private void Shooting()
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
