using UnityEngine;

public class Shot : MonoBehaviour
{
    private const float TimeForDestroyShot = 5;

    [SerializeField] private int _speed;
    [SerializeField] private ParticleSystem _sparksEffect;

    private float _lifeTimer;

    public int Damage;


    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<WarriorController>().TakeDamage(Damage);
       
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

        if(_lifeTimer > TimeForDestroyShot)
        {
            Destroy(gameObject);
        }

        transform.position += transform.forward * Time.deltaTime * _speed;
    }
}
