using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Input;

public class InputStandalone : InputInterface
{
    private Camera _camera;
    Vector2 mousePos = new Vector2();

    public InputStandalone(Camera p_camera) {
        this._camera = p_camera;
    }

    public Vector3 faceDir { get =>  (GetMouseWorldPos() - _camera.transform.position).normalized; }

    public float raycastLength => 1000;

    public bool GetMouse()
    {
        return Input.GetMouseButton(0);
    }

    public bool GetMouseDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool GetMouseUp()
    {
        return Input.GetMouseButtonUp(0);
    }

    public bool SwipeLeft()
    {
        return Input.GetKey(KeyCode.RightArrow);
    }

    public bool SwipeRight()
    {
        return Input.GetKey(KeyCode.LeftArrow);
    }

    public Vector3 GetMouseWorldPos()
    {
        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = Input.mousePosition.x;
        mousePos.y = _camera.pixelHeight - Input.mousePosition.y;

        return _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _camera.nearClipPlane));
    }

    public Ray GetRay()
    {
        return _camera.ScreenPointToRay(Input.mousePosition);
    }

    public Transform GetParent()
    {
        return _camera.transform;
    }

    public void SwitchControllerModel(bool isOn, string exception = null)
    {
    }

    public bool ClickOnMenuKey()
    {
        return Input.GetKeyDown(KeyCode.H);
    }

    public bool HasControlLoader()
    {
        return true;
    }

    public bool HasCtrlLoader()
    {
        return true;
    }
}
