using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class ARTapToPlaceObject : MonoBehaviour
{
    private const float PositionÌalue = 0.5f;

    [SerializeField] private GameObject _placementIndificator;
    [SerializeField] private GameObject _spawnPosition;
    [SerializeField] private ARRaycastManager _arRaycastManager;

    private Pose _placementPose;
    private bool _placementPoseIsValid = false;


    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
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
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(PositionÌalue, PositionÌalue));
        var hits = new List<ARRaycastHit>();
        _arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

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

    public void SpawnWarriorOnPlacementIndicatorPosition()
    {
        if (_placementPoseIsValid && GameManager.Instance.ToggleSetupMode.isOn)
        {
            GameManager.Instance.TryInstantiateWarrior(_placementPose.position, _placementPose.rotation);
        }
    }
}
