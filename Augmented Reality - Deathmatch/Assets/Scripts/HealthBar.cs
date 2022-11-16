using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private int _health = 100;

    [SerializeField] private List<Image> _healthDots;


    private void TakeDamage()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (_health != 0)
            {
                --_health;
            }

            if (_health % 10 == 0 && _health != 100)
            {
                Destroy(_healthDots[_healthDots.Count - 1]);
                _healthDots.RemoveAt(_healthDots.Count - 1);

                ChangeColorDots();
            }
        }
    }

    private void ChangeColorDots()
    {
       foreach (Image color in _healthDots)
        {
            color.color = new Color(1,(float) _health / 100, 0);
        }
    }

   private void Update()
    {
        TakeDamage();
        
        if (Camera.main != null)
        {
            var camXform = Camera.main.transform;
            var forward = transform.position - camXform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }
}
