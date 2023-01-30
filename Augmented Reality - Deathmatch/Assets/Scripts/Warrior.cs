using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Warrior : MonoBehaviour, IPointerDownHandler
{
    private const int MinHeath = 100;
    private const int MaxHeath = 201;
    private const int MinDamage = 5;
    private const int MaxDamage = 9;
    private const int ValueForePercents = 4;
    private const float MinRotationValue = -0.1f;
    private const float MaxRotationValue = 0.1f;
    private const float MinCorrectionForSpawnSparks = 0.25f;
    private const float MaxCorrectionForSpawnSparks = 0.6f;
    private const float TimeForDeath = 5;
    private const float TimeForIdleEventAnim = 10;

    [SerializeField] private Warrior _currentEnemy;
    [SerializeField] private int _speed;
    [SerializeField] private Shot _shotPrefab;
    [SerializeField] private Transform _ganBarrel;
    [SerializeField] private ParticleSystem _sparksEffectFromBarrelGun;
    [SerializeField] private WarriorAnimations _warriorAnimations;
    [SerializeField] private GameObject _healthBar;

    private GameManager _gameManager;
    private bool _isShooting = false;
    private bool _isSpawned = false;
    private float _targetRotation;
    private float _startRotation;
    private int _healthInStart;
    private int _damage;
    private int _currentEnemyHealth;
    private float _timerForDeath;
    private float _timerForIdleEventAnim = TimeForIdleEventAnim;
    private bool _playngAnimShooting;
    private bool _playngAnimRotationRight;
    private bool _playngAnimRotationLeft;
    private bool _playngAnimIdleEvent;

    public event Action<Warrior> Dead;

    public bool IsDead
    {
        get; private set;
    }
    public int Health
    {
        get; private set;
    }


    private void Awake()
    {
        _playngAnimRotationLeft = false;
        _playngAnimRotationRight = false;
        _playngAnimShooting = false;
        _playngAnimIdleEvent = false;
        Health = UnityEngine.Random.Range(MinHeath, MaxHeath);
        _damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
        _healthInStart = Health;
    }

    private void Update()
    {
        ShowsHealthBar();
        DestroyAfterDeath();
        CheckFlagsForAnimations();

        if (FindClosestEnemy())
        {
            _currentEnemyHealth = _currentEnemy.GetComponent<Warrior>().Health;

            if (_isSpawned && !IsDead)
            {
                RotationToClosestEnemy();
                Shooting();
            }
        }
        else
        {
            _playngAnimRotationRight = false;
            _playngAnimRotationLeft = false;
            _playngAnimShooting = false;

            _timerForIdleEventAnim -= Time.deltaTime;

            if (_timerForIdleEventAnim <= 0)
            {
                _timerForIdleEventAnim = TimeForIdleEventAnim;
                _playngAnimIdleEvent = true;
            }
            else
            {
                _playngAnimIdleEvent = false;
            }
        }
    }

    private void CheckFlagsForAnimations()
    {
        _warriorAnimations.CheckAnimationShoting(_playngAnimShooting);
        _warriorAnimations.CheckAnimationRotation(_playngAnimRotationRight, _playngAnimRotationLeft);
        _warriorAnimations.CheckAnimationsIdleEvent(_playngAnimIdleEvent);
    }

    private Warrior FindClosestEnemy()
    {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (Warrior enemy in _gameManager.Enemies)
        {
            if (!enemy.IsDead && position != enemy.transform.position)
            {
                Vector3 diff = enemy.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    _currentEnemy = enemy;
                    distance = curDistance;
                }
            }
        }

        return _currentEnemy;
    }

    private void RotationToClosestEnemy()
    {
        Vector3 direction = _currentEnemy.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _speed * Time.deltaTime);

        _targetRotation = (float)Math.Round(rotation.y, 2);

        if (_startRotation < _targetRotation)
        {
            _playngAnimRotationRight = true;
            _startRotation = (float)Math.Round(transform.rotation.y, 2);
        }
        else
        {
            _playngAnimRotationLeft = true;
            _startRotation = (float)Math.Round(transform.rotation.y, 2);
        }
        if ((Math.Abs(_startRotation) - Math.Abs(_targetRotation)) > MinRotationValue &&
            (Math.Abs(_startRotation) - Math.Abs(_targetRotation)) < MaxRotationValue)
        {
            _playngAnimRotationRight = false;
            _playngAnimRotationLeft = false;
        }
    }

    private void SpawnLaserBullet()
    {
        if (_isShooting)
        {
            var bullet = Instantiate(_shotPrefab, _ganBarrel.position, transform.rotation);
            bullet.GetComponent<Shot>().Damage = _damage;
            SpawnSparksFromBarrelGun();
        }

        _isShooting = true;
    }

    private void Shooting()
    {
        if (Math.Abs(_startRotation) - Math.Abs(_targetRotation) == 0)
        {
            _playngAnimShooting = true;
        }
        else
        {
            _playngAnimShooting = false;
        }

        if (_currentEnemyHealth <= 0)
        {
            _playngAnimShooting = false;
        }
    }

    private void SpawnFlagForAction()
    {
        _isSpawned = true;
    }

    private void SpawnSparksFromBarrelGun()
    {
        var effect = Instantiate(_sparksEffectFromBarrelGun.gameObject,
            new Vector3(_ganBarrel.position.x, _ganBarrel.position.y - MinCorrectionForSpawnSparks, _ganBarrel.position.z + MaxCorrectionForSpawnSparks),
            _ganBarrel.rotation);

        Destroy(effect, _sparksEffectFromBarrelGun.main.startLifetime.constant);
    }

    private void DestroyAfterDeath()
    {
        if (IsDead)
        {
            _timerForDeath += Time.deltaTime;

            if (_timerForDeath > TimeForDeath)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ShowsHealthBar()
    {
        if (Health == _healthInStart)
        {
            _healthBar.SetActive(false);
        }
        else if (!IsDead)
        {
            _healthBar.SetActive(true);
        }
    }

    private void DeathState()
    {
        IsDead = true;
        _healthBar.SetActive(false);
        _warriorAnimations.DeathAnimation();

        Dead?.Invoke(this);
    }

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        _warriorAnimations.AnimationsTakeHit();

        if (Health < 0)
        {
            Health = 0;
        }

        if (Health == 0 && !IsDead)
        {
            DeathState();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isSpawned && Health < _healthInStart && !IsDead && !_gameManager.IsDestroyState)
        {
            Health += _healthInStart / ValueForePercents;

            if (Health > _healthInStart)
            {
                Health = _healthInStart;
            }

            _warriorAnimations.JumpAnimation();
        }

        if (_gameManager.IsDestroyState)
        {
            Health = 0;
            DeathState();
        }
    }
}

