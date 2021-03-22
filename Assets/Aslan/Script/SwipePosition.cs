using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipePosition : MonoBehaviour
{
    private Touch touch;
    private float rotateSpeed = 0.2f;
    private GameObject gameObject = null;
    private Quaternion rotationY;
    private Vector3 pointObject;
    private Camera _camera;

    private void Start()
    {
        _camera = CameraCtrl.instance.GetCurrentCamera();
    }

    private Ray GenerateFingerRay()
    {
        Vector3 posFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.farClipPlane);
        Vector3 posNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.nearClipPlane);

        Vector3 mousePosF = _camera.ScreenToWorldPoint(posFar);
        Vector3 mousePosN = _camera.ScreenToWorldPoint(posNear);

        Ray mr = new Ray(mousePosN, mousePosF - mousePosN);
        return mr;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = GenerateFingerRay();
            RaycastHit hit;

            if(Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
            {
                gameObject = hit.transform.gameObject;
                pointObject = hit.transform.position - hit.point;
            }
        }
        else if (Input.GetMouseButton(0) && gameObject && gameObject.tag == "Position")
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4f);//3.8f
            gameObject.transform.position = _camera.ScreenToWorldPoint(mousePosition);
            //Debug.Log("position");
        }
        else if (Input.GetMouseButton(0) && gameObject && gameObject.tag == "Rotate")
        {
            rotationY = Quaternion.Euler(0f, -Input.mousePosition.x * rotateSpeed, 0f);
            gameObject.transform.rotation = rotationY * transform.rotation;
            //Debug.Log("rotate");
        }
        else if (Input.GetMouseButtonUp(0) && gameObject)
        {
            gameObject = null;
        }

                /*
         if (Input.touchCount > 0)
         {
             touch = Input.GetTouch(0);

             if (touch.phase == TouchPhase.Moved)
             {
                 transform.position = new Vector3(touch.deltaPosition.x, touch.deltaPosition.y, transform.position.z);
             }
         }

         if(Input.touchCount > 0)
         {
             touch = Input.GetTouch(0);

             if(touch.phase == TouchPhase.Moved)
             {
                 rotationY = Quaternion.Euler(0f, -touch.deltaPosition.x * rotateSpeed, 0f);
                 transform.rotation = rotationY * transform.rotation;
             }
         }
         */
    }
    
}
