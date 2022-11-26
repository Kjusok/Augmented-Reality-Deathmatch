using UnityEngine;

public class Shot : MonoBehaviour
{
    private const float _timeForDestroyShot = 5;

    [SerializeField] private int _speed;
    [SerializeField] private ParticleSystem _sparksEffect;

    private float _lifeTimer;


    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        SpawnSparksEffect();
    }

    private void SpawnSparksEffect()
    {
        var effect = Instantiate(_sparksEffect.gameObject, transform.position, Quaternion.identity);
        Destroy(effect, _sparksEffect.main.startLifetime.constant);
    }

   

    private void Update()
    {
        _lifeTimer += Time.deltaTime;

        if(_lifeTimer > _timeForDestroyShot)
        {
            Destroy(gameObject);
        }

        transform.position += transform.forward * Time.deltaTime * _speed;
    }
}
