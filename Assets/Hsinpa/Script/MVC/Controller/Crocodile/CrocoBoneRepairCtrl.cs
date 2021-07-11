using Hsinpa.App;
using Hsinpa.GameInput;
using Hsinpa.View;
using Microsoft.Azure.SpatialAnchors.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Utility;
using View;

namespace Hsinpa.Ctrl
{
    public class CrocoBoneRepairCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private Transform _worldContainer;

        [SerializeField]
        private RaycastInputHandler _raycastInputHandler;

        [SerializeField]
        private ARFoundationHelper _arHelper;

        [SerializeField]
        private PaintingManager _paintingManager;

        [SerializeField]
        private BoneARTemplate crocodileTarget;

        [SerializeField]
        private BoneARTemplate crocodileTargetTimelineAnim;

        [SerializeField]
        private PlayableAsset StartPlayableAnim;

        [SerializeField]
        private PlayableAsset EndPlayableAnim;

        bool _arEnable = false;

        public GeneralFlag.GeneralState _state = GeneralFlag.GeneralState.Idle;

        private int currentToolIndex = -1;

        public System.Action<bool> OnEndGameEvent;

        private Vector3 offsetRotation = Vector3.zero;
        private float resetRotate;
        private float time = 2f;
        private bool resetDirection;

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
                        CheckAndProcessWithCompassAR(0);
                        //PerformPlaneARAction();
                    }
                    break;

                case EventFlag.Event.GameStart:
                    {
                        _arEnable = (bool)p_objects[0];
                    }
                    break;
            }
        }

        public void ReSetBone()
        {
            crocodileTargetTimelineAnim.gameObject.SetActive(false);
            crocodileTarget.gameObject.SetActive(false);
        }

        public void Start()
        {
            ReSetBone();
            _paintingManager.OnTargetDirtIsClear += OnPaintIsDone;
            _paintingManager.OnTargetPainting += OnTargetPainting;
            _raycastInputHandler.OnInputEvent += OnRaycastInputEvent;

            crocodileTargetTimelineAnim.SetUp(OnPlaneARReadyClick);
        }

        public void EnterGame(float yRotationOffset, bool p_arEnable)
        {   
            time = 2;
            ReSetBone();
            this._arEnable = p_arEnable;
            _worldContainer.gameObject.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
            OpenHintUIModal(TypeFlag.ARGameType.GamePrompt1);

            _paintingManager.ResetPaint();
            if (_arEnable)
            {
                CheckAndProcessWithCompassAR(yRotationOffset);
                //PerformPlaneARAction();
            }
            else
            {
                PerformNoARAction();
            }
        }

        private void PerformNoARAction()
        {
            //_arHelper.ActivateFullAR(false);

            Debug.Log("Croco PerformNoARAction");
            crocodileTargetTimelineAnim.gameObject.SetActive(true);
            crocodileTargetTimelineAnim.ShowConfirmBtn(false);
            crocodileTargetTimelineAnim.transform.position = new Vector3(0, 0, 0);

            _arHelper.SetARCameraPos(new Vector3(0, 0.5f, 0), Quaternion.Euler(90, 0, 0));
            _state = GeneralFlag.GeneralState.UnderGoing;

            InitPaintProcedure();
        }

        private void CheckAndProcessWithCompassAR(float yRotationOffset) {
            _arHelper.ActivateAR(true);

            crocodileTargetTimelineAnim.gameObject.SetActive(true);

            try {

                resetRotate = yRotationOffset;
                resetDirection = true;
                //Compass.Instance.SetUp(_worldContainer.gameObject, yRotationOffset);

                OnPlaneARReadyClick();
                }
                catch (System.Exception e)
                {
                    Debug.Log("Exception " + e.Message);

                    PerformPlaneARAction();
                }
        }

        // reset compass direction for 2 seconds
        private void ResetDirection()
        {
            if (time > 0)
            {
                Compass.Instance.SetUp(_worldContainer.gameObject, resetRotate);
                time -= Time.deltaTime;
            }
#if UNITY_ANDROID
            else
            {
                CameraCtrl.instance.OpenARPlaneManager(false);
            }
#endif
        }

        private void PerformPlaneARAction()
        {
            Debug.Log("Croco PerformPlaneARAction");
            crocodileTargetTimelineAnim.gameObject.SetActive(true);
            crocodileTargetTimelineAnim.transform.position = new Vector3(1000, 1000, 100);
            crocodileTargetTimelineAnim.ShowConfirmBtn(true);
            //_arHelper.ActivateAR(true);
            _arHelper.AcitvateARPlane(true);

            _state = GeneralFlag.GeneralState.Preparation;
        }

        private void OnPlaneARReadyClick()
        {
            _arHelper.AcitvateARPlane(false);
            crocodileTargetTimelineAnim.ShowConfirmBtn(false);


            _state = GeneralFlag.GeneralState.UnderGoing;
            InitPaintProcedure();
        }

        private void InitPaintProcedure() {

            crocodileTargetTimelineAnim.SetAndPlayTimelineAnim(StartPlayableAnim);

            _ = UtilityMethod.DoDelayWork((float)StartPlayableAnim.duration, () =>
            {
                currentToolIndex = 0;
                _paintingManager.EquipTool((ToolSRP.ToolEnum)currentToolIndex);

                GameModals.instance.GetModal<ARGameModal>().CloseBackButton(false);
                crocodileTargetTimelineAnim.gameObject.SetActive(false);
                crocodileTarget.gameObject.SetActive(true);
                crocodileTarget.transform.position = crocodileTargetTimelineAnim.transform.position;
                crocodileTarget.transform.rotation = crocodileTargetTimelineAnim.transform.rotation;
            });
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

                crocodileTargetTimelineAnim.transform.position = inputStruct.raycastPosition;
                crocodileTargetTimelineAnim.transform.rotation = faceQuat;
            }
        }

        private void OnDoubleTap()
        {
            Debug.Log("OnDoubleTap");
            if (_state == GeneralFlag.GeneralState.UnderGoing)
                _paintingManager.ShowColorHintEvent((_paintingManager.hintState == PaintingManager.HintState.Flash) ? PaintingManager.HintState.None : PaintingManager.HintState.Flash);
        }

        private void OnPaintIsDone(bool isDone) {
            if (!isDone) return;

            int nextCurrentToolIndex = currentToolIndex + 1;
            if (nextCurrentToolIndex > (int)ToolSRP.ToolEnum.Tool_2) {
                _paintingManager.ResetPaint();

                GameModals.instance.GetModal<ARGameModal>().CloseBackButton(true); // close UI back button

                crocodileTarget.gameObject.SetActive(false);

                crocodileTargetTimelineAnim.SetAndPlayTimelineAnim(EndPlayableAnim);

                _ = UtilityMethod.DoDelayWork((float)EndPlayableAnim.duration, () =>
                {
                    crocodileTargetTimelineAnim.gameObject.SetActive(false);
                    OpenHintUIModal(TypeFlag.ARGameType.GamePrompt3);

                    if (OnEndGameEvent != null)
                        OnEndGameEvent(true);
                });

                return;
            }

            if (nextCurrentToolIndex == (int)ToolSRP.ToolEnum.Tool_2)
                OpenHintUIModal(TypeFlag.ARGameType.GamePrompt2);


            currentToolIndex = nextCurrentToolIndex;
            _paintingManager.EquipTool((ToolSRP.ToolEnum)nextCurrentToolIndex);
        }

        private void OnTargetPainting(int itemIndex, bool isOnTarget) {
            if (isOnTarget && itemIndex == 0)
                SoundPlayerController.Instance.PenSoundEffect();

            if (isOnTarget && itemIndex == 1)
                SoundPlayerController.Instance.BrushSoundEffect();
        }

        private void OnDestroy()
        {
            _paintingManager.OnTargetDirtIsClear -= OnPaintIsDone;
            _paintingManager.OnTargetPainting -= OnTargetPainting;
        }

        private void OpenHintUIModal(TypeFlag.ARGameType type) {

            Debug.Log("CrocoModal Open " + type.ToString("g"));
            if (GameModals.instance == null) return;

            int missionindex = 7;
            var modal = GameModals.instance.OpenModal<ARGameModal>();

            if (modal != null)
                modal.ShowPrompt(missionindex, type);
        }

        private void Update()
        {
            if (resetDirection) { ResetDirection(); }
        }
    }
}