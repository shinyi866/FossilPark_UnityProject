using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.Input;
using Expect.View;
using Hsinpa.App;
using Hsinpa.Input;
using Hsinpa.View;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
using Utility;

namespace Expect.App {
    public class FixLionManager : MonoBehaviour
    {
        [SerializeField]
        FixLionInput FixLionInput;

        [SerializeField]
        PaintingManager PaintingManager;

        [Header("Lighting")]
        [SerializeField]
        InputWrapper inputWrapper;

        [SerializeField]
        GameObject targetGameObject;

        [SerializeField]
        GameObject lightingObject;

        [Header("UI Components")]
        [SerializeField]
        private Button SkipIntroTimelineBtn;

        [SerializeField]
        private Button GameStartBtn;

        [SerializeField]
        private Sprite RestartGameSprite;

        [SerializeField]
        private AudioClip ToolCompleteSound;

        [SerializeField]
        private Canvas EyeFrontCanvas;


        [Header("Timeline Components")]
        [SerializeField]
        private PlayableDirector timeline;

        [SerializeField]
        private TimelineAsset EndingTimeAsset;

        [SerializeField]
        private TimelineAsset StartTimeAsset;

        private int progress = 0;
        private int maxProgress = 3;
        private ToolItem currentHoldItem = null;
        private ToolSRP.Tool currentToolSRP;

        private enum GameState { Pregame, Start, End};
        private GameState currentGameState;

        // Start is called before the first frame update
        void Start()
        {
            inputWrapper.SetUp();
            FixLionInput.SetUp();
            FixLionInput.HoldItemEvent += OnTouchToolEvent;
            PaintingManager.OnTargetDirtIsClear += OnDirtIsCleared;
            PaintingManager.OnTargetPainting += OnTargetPaintingEvent;

            currentGameState = GameState.Pregame;
            SkipIntroTimelineBtn.gameObject.SetActive(false);
            SkipIntroTimelineBtn.onClick.AddListener(OnSkipBtnClick);
            GameStartBtn.onClick.AddListener(OnStartBtnClick);
        }

        // Update is called once per frame
        void Update()
        {
            FixLionInput.OnUpdate();
            UpdateLightDirAccordingToTarget();
        }

        private void DisplayAfterCleanTourGuide() {
            Debug.LogError("Clean all done");

            timeline.playableAsset = EndingTimeAsset;
            timeline.Play();

            currentGameState = GameState.End;

            _ = UtilityMethod.DoDelayWork(6, () =>
            {
                SkipIntroTimelineBtn.gameObject.SetActive(true);
                SkipIntroTimelineBtn.GetComponent<Image>().sprite = RestartGameSprite;
            });
        }

        private void OnTouchToolEvent(ToolItem tool) {
            if (currentHoldItem != null || currentGameState != GameState.Start) return;

            //StringAsset.GetToolStatusType statusType = FixLionUtility.IsGivenToolAllowToProceed(tool.name, this.progress);

            //if (statusType != StringAsset.GetToolStatusType.Available) {
            //    string unavailableMsg = (statusType == StringAsset.GetToolStatusType.AlreadyUsed) ? StringAsset.LionRepairing.ToolUsedMessage : UtilityMethod.GetFromDict<string>(StringAsset.UnavilableTipTable, tool.name, "");
            //    tool.ShowTipIntruction(unavailableMsg);

            //    return;
            //}

            currentHoldItem = tool;

            InputWrapper.instance.platformInput.SwitchControllerModel(false);
            currentHoldItem.PairToParent(InputWrapper.instance.platformInput.GetParent());

            //currentToolSRP = PaintingManager.EquipTool(StringAsset.GetToolEnumByID(currentHoldItem.name));
        }

        private void OnDirtIsCleared(bool isCleared) {
            if (!isCleared)
            {
                if (currentHoldItem != null && InputWrapper.instance.platformInput.HasCtrlLoader()) {
                    InputWrapper.instance.platformInput.SwitchControllerModel(false, currentHoldItem.name);
                    currentHoldItem.PairToParent(InputWrapper.instance.platformInput.GetParent());
                }
                return;
            }

            this.progress += 1;

            PaintingManager.UnEquip();
            InputWrapper.instance.platformInput.SwitchControllerModel(true);
            PlayToolSound(currentToolSRP, false);

            if (currentHoldItem != null)
                currentHoldItem.Return();

            currentHoldItem = null;
            currentToolSRP = default(ToolSRP.Tool);

            if (this.progress == maxProgress)
            {
                DisplayAfterCleanTourGuide();
            }
            else {
                EyeFrontCanvas.gameObject.SetActive(true);

                _ = UtilityMethod.DoDelayWork(3, () =>
                {
                    EyeFrontCanvas.gameObject.SetActive(false);
                });
            }

            //UniversalAudioSolution.instance.PlayAudio(UniversalAudioSolution.AudioType.UI, ToolCompleteSound);
        }

        private void OnTargetPaintingEvent(int toolIndex, bool isHit) {
            if (currentHoldItem == null) return;

            currentHoldItem.EnableParticles(isHit);
            PlayToolSound(currentToolSRP, isHit);
        }

        private void OnStartBtnClick() {
            SkipIntroTimelineBtn.gameObject.SetActive(true);

            GameStartBtn.gameObject.SetActive(false);

            timeline.playableAsset = StartTimeAsset;
            timeline.Play();

            _ = UtilityMethod.DoDelayWork((float)timeline.duration, () =>
            {
                if (currentGameState == GameState.Pregame)
                    currentGameState = GameState.Start;
            });
        }

        private void OnSkipBtnClick() {
            if (currentGameState == GameState.End)
            {
                SceneManager.LoadScene(0);
                return;
            }

            SkipIntroTimelineBtn.gameObject.SetActive(false);

            if (timeline.state == PlayState.Playing && currentGameState == GameState.Pregame) {
                timeline.time = 81.5f;
                currentGameState = GameState.Start;
            }
        }

        private void UpdateLightDirAccordingToTarget() {
            Vector3 direction = (targetGameObject.transform.position - inputWrapper.arCamera.transform.position);
            lightingObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        private IEnumerator PairToolsBackToController() {

            while (!InputWrapper.instance.platformInput.HasCtrlLoader())
            {
                yield return new WaitForSeconds(1f);
            }

            InputWrapper.instance.platformInput.SwitchControllerModel(false);
            currentHoldItem.PairToParent(InputWrapper.instance.platformInput.GetParent());
        }

        public void ResetLevel() {
            progress = 0;
            PaintingManager.ResetPaint();
        }

        public void PlayToolSound(ToolSRP.Tool tool, bool isEnable)
        {
            if (!tool.isValid) return;

            //if (isEnable && !UniversalAudioSolution.instance.IsAudioPlaying(UniversalAudioSolution.AudioType.AudioClip2D))
            //    UniversalAudioSolution.instance.PlayAudio(UniversalAudioSolution.AudioType.AudioClip2D, tool.audioClip, loop: true);
            //else if (!isEnable)
            //    UniversalAudioSolution.instance.StopAudio(UniversalAudioSolution.AudioType.AudioClip2D);
        }

        //void OnApplicationFocus(bool hasFocus)
        //{
        //    if (hasFocus  && currentHoldItem != null) {
        //        Debug.LogError("HasFocus " + hasFocus);

        //        PairToolsBackToController();
        //    }
        //}

        void OnApplicationPause(bool pauseStatus)
        {
            if (currentHoldItem != null)
            {
                Debug.LogError("pauseStatus " + pauseStatus);

                if (!pauseStatus)
                {
                    StartCoroutine(PairToolsBackToController());
                }
                else {
                    currentHoldItem.Return();
                }
            }
        }
    }
}
