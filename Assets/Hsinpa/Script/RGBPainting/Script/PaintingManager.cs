using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Shader;
using Hsinpa.Input;

namespace Hsinpa.App {
    public class PaintingManager : MonoBehaviour
    {
        [SerializeField]
        private Material targetMaterial;

        [SerializeField]
        private DrawToTexture drawToTexture;

        [SerializeField]
        private ToolSRP _toolSRP;

        [SerializeField]
        private Transform targetModel;

        RaycastHit[] m_Results = new RaycastHit[1];

        private int toolIndex;
        private int toolCount;
        private int layerMask = 1 << 9;

        public System.Action<bool> OnTargetDirtIsClear;
        public System.Action<bool> OnTargetPainting;

        public enum HintState { None, Flash };
        private HintState _hintState;

        private float checkCompleteTime = 3f;
        private float recordCompleteTime = 0;

        private void Start()
        {
            drawToTexture.SetUp(targetMaterial);
            toolCount = _toolSRP.tools.Length;
            toolIndex = -1;
        }

        private void Update()
        {
            if (toolIndex >= 0)
            {
                float raycastLength = InputWrapper.instance.platformInput.raycastLength;
                Vector3 diretion = InputWrapper.instance.platformInput.faceDir;
                diretion.y = -diretion.y;

                //Physics.Raycast
                int hits = Physics.RaycastNonAlloc(InputWrapper.instance.platformInput.GetRay(), m_Results, raycastLength, layerMask);

                if (hits > 0)
                {
                    drawToTexture.DrawOnMesh(m_Results[0].textureCoord, _toolSRP.tools[toolIndex].mask_color);
                }

                if (recordCompleteTime < Time.time) {
                    recordCompleteTime = Time.time + checkCompleteTime;
                    CheckIfSocreIsMeet();
                }

                if (OnTargetPainting != null)
                    OnTargetPainting(hits > 0);

                CheckShowColorHintEvent();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                drawToTexture.ResetBuffer();
            }

            CheckIfToolIsPick();
            //RotateTargetModelManually();
        }

        private async void CheckIfSocreIsMeet() {
            float colorScore = await drawToTexture.CalScoreOnDrawMat(_toolSRP.tools[toolIndex].mask_color);
            bool dirtIsClear = colorScore < _toolSRP.tools[toolIndex].difficulty;

            if (OnTargetDirtIsClear != null) {
                OnTargetDirtIsClear(dirtIsClear);
            }
            Debug.LogError("Color Score " + colorScore +", pass " + (dirtIsClear));
        }

        private void CheckIfToolIsPick() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
                EquipTool(ToolSRP.ToolEnum.Tool_1);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
                EquipTool(ToolSRP.ToolEnum.Tool_2);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
                EquipTool(ToolSRP.ToolEnum.Tool_3);
        }

        private void RotateTargetModelManually() {
            int speed = 100;
            if (InputWrapper.instance.platformInput.SwipeRight())
                targetModel.transform.Rotate(Vector3.up * Time.deltaTime * speed);

            if (InputWrapper.instance.platformInput.SwipeLeft())
                targetModel.transform.Rotate(-Vector3.up * Time.deltaTime * speed);
        }

        private void CheckShowColorHintEvent() {
            if (InputWrapper.instance.platformInput.ClickOnMenuKey()) {
                _hintState = (HintState)(((int)_hintState + 1) % 2);
                drawToTexture.EnableColorHint(_hintState);
            }
        }

        public ToolSRP.Tool EquipTool(ToolSRP.ToolEnum toolEnum) {
            toolIndex = (int)toolEnum;
            _hintState = HintState.None;
            drawToTexture.SetPaintColor(_toolSRP.tools[toolIndex].mask_color);

            return _toolSRP.tools[toolIndex];
        }

        public void UnEquip() {
            toolIndex = -1;
            _hintState = HintState.None;
            drawToTexture.EnableColorHint( HintState.None );
        }

        public void ResetPaint() {
            drawToTexture.ResetBuffer();

            _hintState = HintState.None;
            drawToTexture.EnableColorHint(HintState.None );
        }
    }
}