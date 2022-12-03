using UnityEngine;
using UnityEngine.EventSystems;

public class CheckerRaycast : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private ARTapToPlaceObject _arTapToPlaceObject;
   

    public void OnPointerDown(PointerEventData eventData)
    {
        _arTapToPlaceObject.SpawnWarriorOnPlacementIndicatorPosition();
    }
}
