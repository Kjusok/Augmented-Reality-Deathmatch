using UnityEngine;
using UnityEngine.EventSystems;

public class CheckerRaycast : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public bool CanSpawn
    {
        get; private set;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CanSpawn = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CanSpawn = false;
    }
}
