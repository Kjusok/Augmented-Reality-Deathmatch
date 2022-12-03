using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private List<GameObject> _healthUnitsList;
    [SerializeField] private List<float> _valueForDestroyUnitsList;
    [SerializeField] private Warrior _health;
    [SerializeField] private GameObject _heathUpText;

    private float _healthInOneUnit;
    private float _healthInStart;
    private Camera _mainCamera;


    private void Awake()
    {
        _healthInStart = _health.Health;

        CalculateHealthAtOneUnit();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckCurrentHealthUnits();
        ChangeColorUnits();

        if (_mainCamera != null)
        {
            var camXForm = _mainCamera.transform;
            var forward = transform.position - camXForm.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXForm.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
            _heathUpText.transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }

    private void CalculateHealthAtOneUnit()
    {
        _healthInOneUnit = (float)_health.Health / _healthUnitsList.Count;

        var value = 0f;

        for (int i = 0; _valueForDestroyUnitsList.Count < _healthUnitsList.Count; i++)
        {
            _valueForDestroyUnitsList.Add(value);
            value += _healthInOneUnit;
        }
    }

    private void CheckCurrentHealthUnits()
    {
        for (int i = 0; i < _valueForDestroyUnitsList.Count; i++)
        {
            var unitValue = _valueForDestroyUnitsList[i];

            if (_health.Health <= unitValue)
            {
                _healthUnitsList[i].SetActive(false);
            }
            else
            {
                _healthUnitsList[i].SetActive(true);
            }
        }
    }

    private void ChangeColorUnits()
    {
        var value = _health.Health / _healthInStart;

        foreach (GameObject unit in _healthUnitsList)
        {
            var color = unit.GetComponent<Image>();
            color.color = new Color(1, value, 0);
        }
    }
}
