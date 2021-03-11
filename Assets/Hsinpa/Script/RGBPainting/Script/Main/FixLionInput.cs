using Expect.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Input;

namespace Expect.Input
{
    public class FixLionInput : MonoBehaviour
    {
        public System.Action<ToolItem> HoldItemEvent;
        private RaycastHit[] m_Results = new RaycastHit[2];

        public float raycastlength;

        int layerMask;


        public void SetUp() {
            layerMask = LayerMask.GetMask("Interactable");
        }

        public void OnUpdate() {
            if (InputWrapper.instance.platformInput.GetMouseDown()) {
                ToolItem toolItem = DetectAvailableTool();

                if (toolItem != null && HoldItemEvent != null)
                    HoldItemEvent(toolItem);
            }
        }

        private ToolItem DetectAvailableTool() {
            Ray ray = InputWrapper.instance.platformInput.GetRay();
            int hits = Physics.RaycastNonAlloc(ray, m_Results, raycastlength, layerMask);

            if (hits > 0)
            {
                Debug.LogError("Hit " + m_Results[0].collider.gameObject.name);
                return m_Results[0].collider.GetComponent<ToolItem>();
            }

            return null;
        }
    }
}