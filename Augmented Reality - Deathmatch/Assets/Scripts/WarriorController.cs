using System.Collections;
using UnityEngine;

public class WarriorController : MonoBehaviour
{
    [SerializeField] private GameObject _currentEnemy;
    [SerializeField] private int _speed;
    [SerializeField] private Shot _shotPrefab;
    [SerializeField] private Transform _ganBarrel;

    private float _delayForNextShot = 0.6f;


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
    }

    private IEnumerator Shoting()
    {
        while (Input.GetKey(KeyCode.Space))
        {
            yield return new WaitForSeconds(_delayForNextShot);

             Instantiate(_shotPrefab, _ganBarrel.position, transform.rotation);
            _delayForNextShot = 0.5f;
        }
    }

    private void Update()
    {
        if (FindClosestEnemy())
        {
            RotationToClosestEnemy();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shoting());
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            StopAllCoroutines();
            _delayForNextShot = 0.6f;
        }
    }
}
