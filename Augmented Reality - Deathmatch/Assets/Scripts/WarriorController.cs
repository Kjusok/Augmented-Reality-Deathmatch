using System;
using UnityEngine;

public class WarriorController : MonoBehaviour
{
    [SerializeField] private GameObject _currentEnemy;
    [SerializeField] private int _speed;
    [SerializeField] private Shot _shotPrefab;
    [SerializeField] private Transform _ganBarrel;
    [SerializeField] private ParticleSystem _sparksEffectFromBarrelGun;


    private bool _isShoting = false;
    private bool _isSpawned = false;
    private float _targetRotation;
    private float _startRotation;

    public bool PlayngAnimRotationRight
    {
        get; private set;
    }
    public bool PlayngAnimRotationLeft
    {
        get; private set;
    }

    private void Awake()
    {
        PlayngAnimRotationLeft = false;
        PlayngAnimRotationRight = false;
    }

    private GameObject FindClosestEnemy()
    {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject enemy in GameManager.Instance.Enemies)
        {
            if (enemy != null && position != enemy.transform.position)
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

        _targetRotation = (float)Math.Round(rotation.y, 1);

        if (_startRotation < _targetRotation)
        {
            PlayngAnimRotationRight = true;
            _startRotation = (float)Math.Round(transform.rotation.y, 1);
        }
        else
        {
            PlayngAnimRotationLeft = true;
            _startRotation = (float)Math.Round(transform.rotation.y, 1);
        }

        if (Math.Abs(_startRotation) - Math.Abs(_targetRotation) > -0.1f )
        {
            PlayngAnimRotationRight = false;
            PlayngAnimRotationLeft = false;
            _startRotation = 0;
        }
    }

    private void Shoting()
    {
        if (_isShoting)
        {
            Instantiate(_shotPrefab, _ganBarrel.position, transform.rotation);
            SpawnSparksFromBarrelGun();
        }

        _isShoting = true;
    }
    private void SpawnFlagForAction()
    {
        _isSpawned = true;
    }

    private void SpawnSparksFromBarrelGun()
    {
        var effect = Instantiate(_sparksEffectFromBarrelGun.gameObject, new Vector3( _ganBarrel.position.x, _ganBarrel.position.y-0.25f, _ganBarrel.position.z + 0.6f), _ganBarrel.rotation);
        Destroy(effect, _sparksEffectFromBarrelGun.main.startLifetime.constant);
    }

    private void Update()
    {
        Debug.Log(( Math.Abs(_startRotation) - Math.Abs(_targetRotation)));
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(_startRotation);
            Debug.Log(_targetRotation);
        }

        if (FindClosestEnemy())
        {
            if (_isSpawned)
            {
                RotationToClosestEnemy();
            }
        }
        else
        {
            PlayngAnimRotationRight = false;
            PlayngAnimRotationLeft = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isShoting = false;
        }
    }
}
