using UnityEngine;

public class WarriorController : MonoBehaviour
{
    [SerializeField] private GameObject _currentEnemy;
    [SerializeField] private int _speed;
    [SerializeField] private Shot _shotPrefab;
    [SerializeField] private Transform _ganBarrel;


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

    private void Update()
    {
        if (FindClosestEnemy())
        {
            RotationToClosestEnemy();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            var shot = Instantiate(_shotPrefab, _ganBarrel.position, transform.rotation);
            //shot.transform.SetParent(_ganBarrel.transform, true);
        }
    }
}
