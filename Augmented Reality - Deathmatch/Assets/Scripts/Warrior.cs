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
    private const float MinForSpeedAttackAnim = 0.7f;
    private const float MaxForSpeedAttackAnim = 1.5f;
    private const float MinCorrectionForSpawnSparks = 0.25f;
    private const float MaxCorrectionForSpawnSparks = 0.6f;
    private const float TimeForDeath = 5f;
    private const float ShotAnimationLength = 0.15f;

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
    private float _timerForShot;
    private float _speedOfShooting;

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
        _speedOfShooting = (float)Math.Round(UnityEngine.Random.Range(MinForSpeedAttackAnim, MaxForSpeedAttackAnim), 2);
        Health = UnityEngine.Random.Range(MinHeath, MaxHeath);
        _damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
        _healthInStart = Health;

        _warriorAnimations.CreateSpeedAttack(_speedOfShooting);
    }

    private void Update()
    {
        ShowsHealthBar();
        DestroyAfterDeath();
       
        if (FindClosestEnemy())
        {
            _currentEnemyHealth = _currentEnemy.GetComponent<Warrior>().Health;

            if (_isSpawned && !IsDead)
            {
                RotationToClosestEnemy();
                Shooting();
            }
        }
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
        _startRotation = (float)Math.Round(transform.rotation.y, 2);

        _warriorAnimations.PlayRotation((Math.Abs(_startRotation) - Math.Abs(_targetRotation)));
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
        if (Math.Abs(_startRotation) - Math.Abs(_targetRotation) == 0 && _currentEnemyHealth > 0)
        {
            if (_timerForShot <= 0)
            {
                _warriorAnimations.PlayShot();
                _timerForShot = ShotAnimationLength / _speedOfShooting;
            }
            else
            {
                _timerForShot -= Time.deltaTime;
            }
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
        _warriorAnimations.PlayDeath();

        Dead?.Invoke(this);
    }

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        _warriorAnimations.PlayTakeHit();

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

            _warriorAnimations.PlayJump();
        }

        if (_gameManager.IsDestroyState)
        {
            Health = 0;
            DeathState();
        }
    }
}

