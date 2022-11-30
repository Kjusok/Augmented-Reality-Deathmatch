using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private List<GameObject> _healthDotsList;
    [SerializeField] private List<float> _valueForDestroyDotsList;
    [SerializeField] private WarriorController _health;
    [SerializeField] private GameObject _heathUpText;

    private float _healthInOneDot;
    private float _healthInStart;


    private void Awake()
    {
        _healthInStart = _health.Health;

        CalculateHealthAtOneDot();
    }

    private void Update()
    {
        CheckCurrentHealthDots();
        ChangeColorDots();

        if (Camera.main != null)
        {
            var camXform = Camera.main.transform;
            var forward = transform.position - camXform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
            _heathUpText.transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }

    private void CalculateHealthAtOneDot()
    {
        _healthInOneDot = (float)_health.Health / _healthDotsList.Count;

        var value = 0f;

        for (int i = 0; _valueForDestroyDotsList.Count < _healthDotsList.Count; i++)
        {
            _valueForDestroyDotsList.Add(value);
            value += _healthInOneDot;
        }
    }

    private void CheckCurrentHealthDots()
    {
        for (int i = 0; i < _valueForDestroyDotsList.Count; i++)
        {
            var dotValue = _valueForDestroyDotsList[i];

            if (_health.Health <= dotValue)
            {
                _healthDotsList[i].SetActive(false);
            }
            else
            {
                _healthDotsList[i].SetActive(true);
            }
        }
    }

    private void ChangeColorDots()
    {
        var value = _health.Health / _healthInStart;
        ;

        foreach (GameObject dot in _healthDotsList)
        {
            var color = dot.GetComponent<Image>();
            color.color = new Color(1, value, 0);
        }
    }
}
