using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class PlaceARObject : MonoBehaviour
{
    public GameObject[] animalObjects;
    public GameObject[] dinosaurlObjects;
    public GameObject[] dinosaurlBabyObjects;
    public Texture[] animalMarkTexture;
    public ARPlaneManager planeManager;
    public GameObject placeMark;
    public Material material;
    public GameObject spawnedObject;

    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placePosIsValid = false;
    private bool isARpage;
    private Camera _camera;
    private int currentAnimal;
    private TypeFlag.ARObjectType currentType;

    private static PlaceARObject _instance;

    public static PlaceARObject instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlaceARObject>();
            }

            return _instance;
        }
    }

    private void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    public void EnterAR(int index, TypeFlag.ARObjectType type)
    {
        _camera = CameraCtrl.instance.GetCurrentCamera();
        isARpage = true;
        currentType = type;
        currentAnimal = index;
        planeManager.enabled = true;        
        placeMark.SetActive(true);

        if(currentType == TypeFlag.ARObjectType.Animals)
            material.mainTexture = animalMarkTexture[currentAnimal - 2];
        if (currentType == TypeFlag.ARObjectType.Dinosaurls || currentType == TypeFlag.ARObjectType.DinosaurlBaby)
            material.mainTexture = animalMarkTexture[animalMarkTexture.Length - 1];
    }

    public void EnterNoAR(int index)
    {
        var animalTransform = animalObjects[index - 2].transform;
        _camera = CameraCtrl.instance.GetCurrentCamera();

        Instantiate(animalObjects[index - 2], new Vector3(animalTransform.position.x, animalTransform.position.y, 1.5f), animalTransform.rotation);
        _camera.transform.position = new Vector3(0 ,0.5f, 0);
    }

    public void CloseAR()
    {
        isARpage = false;
        planeManager.enabled = false;
        Destroy(spawnedObject);
        spawnedObject = null;
    }
    
    private void Update()
    {
        if (!isARpage) return;
        
        var hits = new List<ARRaycastHit>();

        if (spawnedObject != null)
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            if (raycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
            {
                var hitPos = hits[0].pose;

                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    spawnedObject.transform.position = hitPos.position;
                }
            }
        }
        else
        {
            UpdatePlacementPose(hits);

            if (placePosIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                PlaceObject();
            }
        }
    }
    
    private void PlaceObject()
    {
        placeMark.SetActive(false);
        Vector3 lookAtTarget;
        switch (currentType)
        {
            case TypeFlag.ARObjectType.Animals:
                spawnedObject = Instantiate(animalObjects[currentAnimal-2], placementPose.position, animalObjects[currentAnimal - 2].transform.rotation);
                //spawnedObject.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
                lookAtTarget = new Vector3(_camera.transform.forward.x, spawnedObject.transform.position.y, _camera.transform.forward.z);
                spawnedObject.transform.LookAt(lookAtTarget);
                break;
            case TypeFlag.ARObjectType.Dinosaurls:
                spawnedObject = Instantiate(dinosaurlObjects[currentAnimal-2], placementPose.position, animalObjects[currentAnimal - 2].transform.rotation);
                //spawnedObject.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
                lookAtTarget = new Vector3(_camera.transform.forward.x, spawnedObject.transform.position.y, _camera.transform.forward.z);
                spawnedObject.transform.LookAt(lookAtTarget);
                break;
            case TypeFlag.ARObjectType.DinosaurlBaby:
                spawnedObject = Instantiate(dinosaurlBabyObjects[currentAnimal], placementPose.position, animalObjects[currentAnimal].transform.rotation);
                //spawnedObject.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
                lookAtTarget = new Vector3(_camera.transform.forward.x, spawnedObject.transform.position.y, _camera.transform.forward.z);
                spawnedObject.transform.LookAt(lookAtTarget);
                break;
        }        
    }

    private void UpdatePlacementPose(List<ARRaycastHit> hits)
    {
        var screenCenter = _camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        
        raycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        placePosIsValid = hits.Count > 0;

        if(placePosIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = _camera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

            placeMark.SetActive(true);
            placeMark.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placeMark.SetActive(false);
        }
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0 && !IsPointOverUI(Input.GetTouch(0).position))
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private bool IsPointOverUI(Vector2 screenPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}
