using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Shader;
using Hsinpa.Input;
using UnityEngine.SocialPlatforms;

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

        [SerializeField]
        private Hsinpa.View.ToolItemSwitcher toolSwitcher;

        RaycastHit[] m_Results = new RaycastHit[1];

        private int toolIndex;
        private int toolCount;
        private int layerMask = 1 << 22;

        public System.Action<bool> OnTargetDirtIsClear;
        public System.Action<int, bool> OnTargetPainting;

        public enum HintState { None, Flash };
        private HintState _hintState;
        public HintState hintState => _hintState;

        private float checkCompleteTime = 3f;
        private float recordCompleteTime = 0;
        private Vector2 previousMousePoint = new Vector2();
        private Quaternion orientationAngle = Quaternion.identity;

        private void Start()
        {
            drawToTexture.SetUp(targetMaterial);
            toolCount = _toolSRP.tools.Length;
            toolIndex = -1;

            toolSwitcher.HideAll();
        }

        private void Update()
        {
            if (toolIndex >= 0)
            {
                float raycastLength = InputWrapper.instance.platformInput.raycastLength;
                Vector3 diretion = InputWrapper.instance.platformInput.faceDir;
                diretion.y = -diretion.y;

                //Physics.Raycast
                Ray ray = InputWrapper.instance.platformInput.GetRay();
                int hits = Physics.RaycastNonAlloc(InputWrapper.instance.platformInput.GetRay(), m_Results, raycastLength, layerMask);

                //Debug.Log("Hits Count " + hits);

                if (hits > 0)
                {
                    drawToTexture.DrawOnMesh(m_Results[0].textureCoord, _toolSRP.tools[toolIndex].mask_color);

                    toolSwitcher.gameObject.SetActive(true);
                    toolSwitcher.EnableCurrentToolParticle(true);

                    toolSwitcher.transform.position = m_Results[0].point;
                    Vector2 currentScreenPoint = new Vector2(m_Results[0].point.x, m_Results[0].point.z);
                    Vector2 deltaScreenPoint = (previousMousePoint - currentScreenPoint).normalized;

                    //Vibration
                    if (toolSwitcher.currentObject != null && toolSwitcher.currentObject.vibrate) {
                        Vector3 currentPosition = m_Results[0].point;
                        float randomRange = 0.001f;
                        toolSwitcher.transform.position= new Vector3(currentPosition.x + Random.Range(-randomRange, randomRange), 
                                                                    currentPosition.y, currentPosition.z + Random.Range(-randomRange, randomRange));
                    }

                    if (toolSwitcher.currentObject != null && toolSwitcher.currentObject.deltaOrientation)
                        HandleItemOrientation(deltaScreenPoint, toolSwitcher.currentObject);

                    previousMousePoint.Set(m_Results[0].point.x, m_Results[0].point.z);
                }
                else {
                    toolSwitcher.EnableCurrentToolParticle(false);
                    toolSwitcher.gameObject.SetActive(false);
                }

                if (recordCompleteTime < Time.time) {
                    recordCompleteTime = Time.time + checkCompleteTime;
                    CheckIfSocreIsMeet();
                }

                if (OnTargetPainting != null)
                    OnTargetPainting(toolIndex, hits > 0);

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
            Debug.Log("Color Score " + colorScore +", pass " + (dirtIsClear));
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
                ShowColorHintEvent((HintState)(((int)_hintState + 1) % 2));
            }
        }

        private void HandleItemOrientation(Vector2 normalizeDelta, View.ToolItemObject toolItem) {
            float diff = normalizeDelta.magnitude;

            if (diff < 0.01f) return;

            float m_Angle = -(Mathf.Atan2(normalizeDelta.y, normalizeDelta.x) * Mathf.Rad2Deg - 90);
            Quaternion currentDir = Quaternion.Euler(-112.26f, m_Angle, 0);

            orientationAngle = Quaternion.Lerp(orientationAngle, currentDir, 0.1f);

            //Debug.Log("m_Angle " + m_Angle);
            toolItem.transform.localRotation = orientationAngle;
        }

        public void ShowColorHintEvent(HintState hState)
        {
            _hintState = hState;
            drawToTexture.EnableColorHint(hState);
        }

        public ToolSRP.Tool EquipTool(ToolSRP.ToolEnum toolEnum) {
            toolIndex = (int)toolEnum;
            _hintState = HintState.None;
            previousMousePoint = Vector2.zero;
            orientationAngle = Quaternion.identity;
            drawToTexture.SetPaintColor(_toolSRP.tools[toolIndex].mask_color);

            toolSwitcher.EnableTool((int)toolEnum);

            return _toolSRP.tools[toolIndex];
        }

        public void UnEquip() {
            toolIndex = -1;
            _hintState = HintState.None;
            drawToTexture.EnableColorHint( HintState.None );
            toolSwitcher.HideAll();
            previousMousePoint = Vector2.zero;
        }

        public void ResetPaint() {
            UnEquip();
            drawToTexture.ResetBuffer();
        }
    }
}