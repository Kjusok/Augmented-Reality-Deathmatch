using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Instance not specified");
            }
            return _instance;
        }
    }

    [SerializeField] private GameObject _warrior;
    [SerializeField] public List<GameObject> Enemies;


    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        
    }
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
    private void InstantiateWarrior()
    {
        var enemy = Instantiate(_warrior, new Vector3(Random.Range(-12, 12), 0.062f, Random.Range(3, 20)), Quaternion.identity);
        Enemies.Add(enemy);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            InstantiateWarrior();
        }
    }
}
