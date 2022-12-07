using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private List<GameObject> _healthChanksList;
    [SerializeField] private List<float> _valueForDestroyChanksList;
    [SerializeField] private Warrior _health;
    [SerializeField] private GameObject _heathUpText;

    private float _healthInOneChunk;
    private float _healthInStart;
    private Camera _mainCamera;


    private void Awake()
    {
        _healthInStart = _health.Health;

        CalculateHealthAtOneChunk();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckCurrentHealthChunks();
        ChangeColorChunks();

        if (_mainCamera != null)
        {
            var cameraXTransform = _mainCamera.transform;
            var forward = transform.position - cameraXTransform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, cameraXTransform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
            _heathUpText.transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }

    private void CalculateHealthAtOneChunk()
    {
        _healthInOneChunk = (float)_health.Health / _healthChanksList.Count;

        var value = 0f;

        while (_valueForDestroyChanksList.Count < _healthChanksList.Count)
        {
            _valueForDestroyChanksList.Add(value);
            value += _healthInOneChunk;
        }
    }

    private void CheckCurrentHealthChunks()
    {
        for (int i = 0; i < _valueForDestroyChanksList.Count; i++)
        {
            var unitValue = _valueForDestroyChanksList[i];

            if (_health.Health <= unitValue)
            {
                _healthChanksList[i].SetActive(false);
            }
            else
            {
                _healthChanksList[i].SetActive(true);
            }
        }
    }

    private void ChangeColorChunks()
    {
        var value = _health.Health / _healthInStart;

        foreach (GameObject unit in _healthChanksList)
        {
            var color = unit.GetComponent<Image>();
            color.color = new Color(1, value, 0);
        }
    }
}
