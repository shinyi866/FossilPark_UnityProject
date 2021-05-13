using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LightHouse.Edit
{
    public class InputEditTranslate
    {
        float threshold = 4;
        float dragSpeed = 0.6f;

        GameObject targetObject;

        private bool isDragging;
        private Vector3 lastMousePosition;

        public InputEditTranslate() { 
        
        }

        public void SetUp(GameObject targetObject) {
            this.targetObject = targetObject;
            lastMousePosition = Input.mousePosition;
        }

        public void OnUpdate()
        {
            if (!isDragging && Input.GetMouseButton(0))
            {
                isDragging = CanStartTranslateObj();
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging) {
                MoveObjectAround();
            }

            lastMousePosition = Input.mousePosition;
        }

        private bool CanStartTranslateObj() {
            float dist = (lastMousePosition - Input.mousePosition).sqrMagnitude;

            lastMousePosition = Input.mousePosition;

            return dist > threshold;
        }

        private void MoveObjectAround() {
            Vector3 offSet = (Input.mousePosition - lastMousePosition).normalized * dragSpeed * Time.deltaTime;

            Transform parentObject = targetObject.transform.parent;

            if (parentObject == null) return;

            Vector3 forward = parentObject.transform.forward * offSet.y;
            forward.y = 0;

            Vector3 right = parentObject.transform.right * offSet.x;
            right.y = 0;

            targetObject.transform.position = targetObject.transform.position + forward + right;
        }

        public void Reset() { 
        
        }
    }
}