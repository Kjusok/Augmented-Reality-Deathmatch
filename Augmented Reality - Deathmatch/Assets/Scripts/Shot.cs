using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] private int _speed;


    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * _speed;
    }
}
