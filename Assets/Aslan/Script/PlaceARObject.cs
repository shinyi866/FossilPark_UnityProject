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
    private GameObject noARanimalObject;
    private float rotateSpeed = 0.1f;

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
        
        if (currentType == TypeFlag.ARObjectType.Animals)
            material.mainTexture = animalMarkTexture[currentAnimal - 2];
        if (currentType == TypeFlag.ARObjectType.Dinosaurls || currentType == TypeFlag.ARObjectType.DinosaurlBaby)
            material.mainTexture = animalMarkTexture[animalMarkTexture.Length - 1];
    }

    public void EnterARAndroid(int index, TypeFlag.ARObjectType type)
    {
        _camera = CameraCtrl.instance.ARcamera.GetComponent<Camera>();
        //_camera.transform.position = new Vector3(0, 0, 0);

        var _frontPos = _camera.transform.forward;
        var _upPos = _camera.transform.up;

        switch (type)
        {
            case TypeFlag.ARObjectType.Animals:
                var animalTransform = animalObjects[index - 2].transform;
                spawnedObject = Instantiate(animalObjects[index - 2], new Vector3(animalTransform.position.x, animalTransform.position.y, 1.5f), animalTransform.rotation);

                if (index == 3)
                {
                    spawnedObject.transform.position = _camera.transform.position + _upPos * -1.5f + _frontPos * 1.5f;
                    spawnedObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    spawnedObject.transform.position = _camera.transform.position + _upPos * -1.8f + _frontPos * 2f;
                    spawnedObject.transform.rotation = Quaternion.Euler(0, 140, 0);
                }

                break;
            case TypeFlag.ARObjectType.Dinosaurls:
                var dinosaurlTransform = dinosaurlObjects[index - 2].transform;
                spawnedObject = Instantiate(dinosaurlObjects[index - 2], new Vector3(dinosaurlTransform.position.x, dinosaurlTransform.position.y, 1.5f), dinosaurlTransform.rotation);
                spawnedObject.transform.position = _camera.transform.position + _upPos * -1.8f + _frontPos * 5f;
                spawnedObject.transform.rotation = Quaternion.Euler(0, 110, 0);
                break;
            case TypeFlag.ARObjectType.DinosaurlBaby:
                var dinosaurlBabyTransform = dinosaurlBabyObjects[index].transform;
                spawnedObject = Instantiate(dinosaurlBabyObjects[index], new Vector3(dinosaurlBabyTransform.position.x, dinosaurlBabyTransform.position.y, 1.5f), dinosaurlBabyTransform.rotation);
                spawnedObject.transform.position = _camera.transform.position + _upPos * -1.8f + _frontPos * 2f;
                break;
        }
    }

    public void EnterNoAR(int index, TypeFlag.ARObjectType type)
    {   
        _camera = CameraCtrl.instance.GetCurrentCamera();
        _camera.transform.position = new Vector3(0, 0, 0);
        var _frontPos = _camera.transform.forward;
        var _upPos = _camera.transform.up;
        
        switch (type)
        {
            case TypeFlag.ARObjectType.Animals:
                var animalTransform = animalObjects[index - 2].transform;
                noARanimalObject = Instantiate(animalObjects[index - 2], new Vector3(animalTransform.position.x, animalTransform.position.y, 1.5f), animalTransform.rotation);

                if(index == 3)
                {
                    noARanimalObject.transform.position = _camera.transform.position + _upPos * -1.5f + _frontPos * 1.5f;
                    noARanimalObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    noARanimalObject.transform.position = _camera.transform.position + _upPos * -1.8f + _frontPos * 2f;
                    noARanimalObject.transform.rotation = Quaternion.Euler(0, 140, 0);
                }
                
                break;
            case TypeFlag.ARObjectType.Dinosaurls:
                var dinosaurlTransform = dinosaurlObjects[index - 2].transform;
                noARanimalObject = Instantiate(dinosaurlObjects[index - 2], new Vector3(dinosaurlTransform.position.x, dinosaurlTransform.position.y, 1.5f), dinosaurlTransform.rotation);
                noARanimalObject.transform.position = _camera.transform.position + _upPos * -1.8f + _frontPos * 5f;
                noARanimalObject.transform.rotation = Quaternion.Euler(0, 110, 0);
                break;
            case TypeFlag.ARObjectType.DinosaurlBaby:
                var dinosaurlBabyTransform = dinosaurlBabyObjects[index].transform;
                noARanimalObject = Instantiate(dinosaurlBabyObjects[index], new Vector3(dinosaurlBabyTransform.position.x, dinosaurlBabyTransform.position.y, 1.5f), dinosaurlBabyTransform.rotation);
                noARanimalObject.transform.position = _camera.transform.position + _upPos * -1.8f + _frontPos * 2f;
                break;
        }
    }

    public void CloseAR()
    {
        isARpage = false;
        planeManager.enabled = false;
        Destroy(spawnedObject);
        Destroy(noARanimalObject);
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

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    spawnedObject.transform.position = hitPos.position;
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                var rotateY = Quaternion.Euler(0f, -Input.GetTouch(0).deltaPosition.x * rotateSpeed, 0f);
                spawnedObject.transform.rotation = rotateY * spawnedObject.transform.rotation;

                //spawnedObject.transform.Rotate(new Vector3(0, 10, 0) * Time.deltaTime * rotateSpeed);
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
                lookAtTarget = new Vector3(_camera.transform.position.x, spawnedObject.transform.position.y, _camera.transform.position.z);
                spawnedObject.transform.LookAt(lookAtTarget);

                if (currentAnimal == 5)
                    noARanimalObject.transform.rotation = Quaternion.Euler(0, 0, 0);

                break;
            case TypeFlag.ARObjectType.Dinosaurls:
                spawnedObject = Instantiate(dinosaurlObjects[currentAnimal-2], placementPose.position, animalObjects[currentAnimal - 2].transform.rotation);
                lookAtTarget = new Vector3(_camera.transform.position.x, spawnedObject.transform.position.y, _camera.transform.position.z);
                spawnedObject.transform.LookAt(lookAtTarget);
                break;
            case TypeFlag.ARObjectType.DinosaurlBaby:
                spawnedObject = Instantiate(dinosaurlBabyObjects[currentAnimal], placementPose.position, animalObjects[currentAnimal].transform.rotation);
                lookAtTarget = new Vector3(_camera.transform.position.x, spawnedObject.transform.position.y, _camera.transform.position.z);
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
