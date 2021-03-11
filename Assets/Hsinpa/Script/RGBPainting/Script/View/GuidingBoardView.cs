using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class GuidingBoardView : MonoBehaviour
    {
        [SerializeField]
        private GuideBoardSRP GuideBoardSRP;

        [SerializeField]
        private Text title;

        [SerializeField]
        private Text content;

        [SerializeField]
        private Image sideSprite;

        [SerializeField]
        private Button engGuideAuidoBtn;

        [SerializeField]
        private Button chtGuideAuidoBtn;

        private void Start()
        {
            SetContent(GuideBoardSRP);
        }

        private void SetContent(GuideBoardSRP p_guideBoardSRP) {
            title.text = p_guideBoardSRP.title;
            content.text = p_guideBoardSRP.textAsset.text;
            sideSprite.sprite = p_guideBoardSRP.sprite;

            SetAudioBtnEvent(engGuideAuidoBtn, p_guideBoardSRP.enAudioGuide);
            SetAudioBtnEvent(chtGuideAuidoBtn, p_guideBoardSRP.chtAudioGuide);
        }

        private void SetAudioBtnEvent(Button targetBtn, AudioClip audio) {
            targetBtn.onClick.RemoveAllListeners();

            targetBtn.onClick.AddListener(() =>
            {
                //UniversalAudioSolution.instance.PlayAudio(UniversalAudioSolution.AudioType.AudioClip2D, audio);
            });
        }

    }
}