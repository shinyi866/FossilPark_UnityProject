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
    public Sprite[] animalMarkSprite;
    //public MeshRenderer mesh;
    public GameObject placeMark;

    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placePosIsValid = false;
    private bool isARpage;
    private Camera _camera;
    private int currentAnimal;
    private TypeFlag.ARObjectType currentType;
    private GameObject spawnedObject;
    //private Material material;
    private Vector2 offset;

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

    public void EnterAR(int index, TypeFlag.ARObjectType type)
    {
        isARpage = true;
        currentType = type;
        currentAnimal = index;
        //material = mesh.material;
        placeMark.SetActive(true);
    }

    public void CloseAR()
    {
        isARpage = false;
        Destroy(spawnedObject);
        spawnedObject = null;
    }

    private void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        _camera = CameraCtrl.instance.GetCurrentCamera();
    }

    private void Update()
    {
        if (!isARpage) return;
        
        var hits = new List<ARRaycastHit>();

        if (spawnedObject != null)
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            if (raycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
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
        
        switch (currentType)
        {
            case TypeFlag.ARObjectType.Animals:
                //material.mainTextureOffset = animalMarkSprite[currentAnimal].textureRectOffset;
                spawnedObject = Instantiate(animalObjects[currentAnimal-2], new Vector3(placementPose.position.x, -1, placementPose.position.z), placementPose.rotation);
                break;
            case TypeFlag.ARObjectType.Dinosaurls:
                spawnedObject = Instantiate(dinosaurlObjects[currentAnimal], placementPose.position, placementPose.rotation);
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
