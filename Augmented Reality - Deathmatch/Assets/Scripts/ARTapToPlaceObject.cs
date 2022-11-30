using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class ARTapToPlaceObject : MonoBehaviour
{
    private const float _midlleValue = 0.5f;

    [SerializeField] private GameObject _placementIndificator;
    [SerializeField] private GameObject _spawnPosition;

    private ARRaycastManager _arRaycatsManager;
    private Pose _placementPose;
    private bool _placementPoseIsValid = false;
    private CheckerRaycast _checkRaycast;


    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
       _arRaycatsManager = FindObjectOfType<ARRaycastManager>();
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(_midlleValue, _midlleValue));
        var hits = new List<ARRaycastHit>();
        _arRaycatsManager.Raycast(screenCenter, hits, TrackableType.Planes);

        _placementPoseIsValid = hits.Count > 0;

        if (_placementPoseIsValid)
        {
            _placementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            _placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (_placementPoseIsValid)
        {
            _placementIndificator.SetActive(true);
            _placementIndificator.transform.SetPositionAndRotation(_placementPose.position, _placementPose.rotation);
        }
        else
        {
            _placementIndificator.SetActive(false);
        }

    }

    private void Update()
    {
        if (GameManager.Instance.ToggleSetupMode.isOn)
        {
            _spawnPosition.SetActive(true);

            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }
        else
        {
            _spawnPosition.SetActive(false);
        }

        if (_placementPoseIsValid && Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began && GameManager.Instance.ToggleSetupMode.isOn)
            //&&
            //_checkRaycast.CanSpawn)
        {
            GameManager.Instance.InstantiateWarrior(_placementPose.position, _placementPose.rotation);
        }
    }
}
