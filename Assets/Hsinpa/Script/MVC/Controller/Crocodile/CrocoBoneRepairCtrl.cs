using Hsinpa.App;
using Hsinpa.GameInput;
using Hsinpa.View;
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
        private ARFoundationHelper _arHelper;

        [SerializeField]
        private PaintingManager _paintingManager;

        [SerializeField]
        private BoneARTemplate crocodileTarget;

        bool _arEnable = false;

        public GeneralFlag.GeneralState _state = GeneralFlag.GeneralState.Idle;

        private int currentToolIndex = -1;

        public System.Action<bool> OnEndGameEvent;

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
            crocodileTarget.gameObject.SetActive(false);
            _paintingManager.OnTargetDirtIsClear += OnPaintIsDone;
            _raycastInputHandler.OnInputEvent += OnRaycastInputEvent;

            crocodileTarget.SetUp(OnPlaneARReadyClick);
        }

        public void EnterGame()
        {

            _paintingManager.ResetPaint();
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
            crocodileTarget.gameObject.SetActive(true);
            crocodileTarget.ShowConfirmBtn(false);

            _arHelper.SetARCameraPos(new Vector3(0, 0.8f, 0), Quaternion.Euler(90, 0, 0));
            _state = GeneralFlag.GeneralState.UnderGoing;

            InitPaintProcedure();
        }

        private void PerformPlaneARAction()
        {
            Debug.Log("Croco PerformPlaneARAction");
            crocodileTarget.gameObject.SetActive(true);
            crocodileTarget.transform.position = new Vector3(1000, 1000, 100);
            crocodileTarget.ShowConfirmBtn(true);
            _arHelper.ActivateAR(true);
            _arHelper.AcitvateARPlane(true);

            _state = GeneralFlag.GeneralState.Preparation;
        }

        private void OnPlaneARReadyClick()
        {
            _arHelper.AcitvateARPlane(false);
            crocodileTarget.ShowConfirmBtn(false);

            _state = GeneralFlag.GeneralState.UnderGoing;
            InitPaintProcedure();
        }

        private void InitPaintProcedure() {
            currentToolIndex = 0;
            _paintingManager.EquipTool((ToolSRP.ToolEnum) currentToolIndex);
        }

        private void OnRaycastInputEvent(RaycastInputHandler.InputStruct inputStruct)
        {
            switch (inputStruct.inputType)
            {
                case RaycastInputHandler.InputType.SingleTap:
                    {
                        OnSingleTap(inputStruct);
                    }
                    break;
                case RaycastInputHandler.InputType.DoubleTap:
                    {
                        OnDoubleTap();
                    }
                    break;
            }
        }

        private void OnSingleTap(RaycastInputHandler.InputStruct inputStruct)
        {
            if (inputStruct.gameObject.layer == GeneralFlag.Layer.PlaneInt && _state == GeneralFlag.GeneralState.Preparation)
            {
                Vector3 dir = (inputStruct.raycastPosition - _arHelper.arCamera.transform.position).normalized;
                dir.y = 0;
                Quaternion faceQuat = Quaternion.LookRotation(dir);

                crocodileTarget.transform.position = inputStruct.raycastPosition;
                crocodileTarget.transform.rotation = faceQuat;
            }
        }

        private void OnDoubleTap()
        {

        }

        private void OnPaintIsDone(bool isDone) {
            if (!isDone) return;

            int nextCurrentToolIndex = currentToolIndex + 1;
            if (nextCurrentToolIndex > (int)ToolSRP.ToolEnum.Tool_3) {
                _paintingManager.ResetPaint();

                if (OnEndGameEvent != null)
                    OnEndGameEvent(true);

                return;
            }

            currentToolIndex = nextCurrentToolIndex;
            _paintingManager.EquipTool((ToolSRP.ToolEnum)nextCurrentToolIndex);
        }

        private void OnDestroy()
        {
            _paintingManager.OnTargetDirtIsClear -= OnPaintIsDone;
        }
    }
}