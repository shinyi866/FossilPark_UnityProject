using Hsinpa.App;
using Hsinpa.GameInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Ctrl
{
    public class CrocoBoneRepairCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private RaycastInputHandler _raycastInputHandler;

        [SerializeField]
        private PaintingManager _paintingManager;

        bool _arEnable = false;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.OnCrocodileARMode_NoAR:
                    {
                        PerformNoARAction();
                    }
                    break;

                case EventFlag.Event.OnCrocodileARMode_PlaneAR:
                    {
                        PerformPlaneARAction();

                    }
                    break;

                case EventFlag.Event.GameStart:
                    {
                        _arEnable = (bool)p_objects[0];
                    }
                    break;
            }
        }

        public void Start()
        {
            _paintingManager.gameObject.SetActive(false);
            _raycastInputHandler.OnInputEvent += OnRaycastInputEvent;
        }

        public void EnterGame()
        {

            if (_arEnable)
            {
                PerformPlaneARAction();
            }
            else
            {
                PerformNoARAction();
            }
        }

        private void PerformNoARAction()
        {
            Debug.Log("Croco PerformNoARAction");
            _paintingManager.gameObject.SetActive(true);
            _paintingManager.gameObject.transform.position = new Vector3(0, 0, 1.2f);
            InitPaintProcedure();
        }

        private void PerformPlaneARAction()
        {
            Debug.Log("Croco PerformPlaneARAction");
            _paintingManager.gameObject.SetActive(true);
            InitPaintProcedure();
        }

        private void InitPaintProcedure() {
            _paintingManager.EquipTool(ToolSRP.ToolEnum.Tool_1);
        }

        private void OnRaycastInputEvent(RaycastInputHandler.InputStruct inputStruct)
        {

        }
    }
}