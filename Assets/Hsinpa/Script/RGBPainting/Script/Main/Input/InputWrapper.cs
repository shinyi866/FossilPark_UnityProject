using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hsinpa.Input {

    public class InputWrapper : MonoBehaviour
    {
        [SerializeField]
        private Camera standaloneAsset;

        public GameObject cameraObject {
            get {
#if UNITY_EDITOR
                return standaloneAsset.gameObject;
#else
#endif
            }
        }


        public InputInterface platformInput;

        private static InputWrapper _instance;
        public static InputWrapper instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InputWrapper>();
                    _instance.SetUp();
                }
                return _instance;
            }
        }        

        public void SetUp() {
            standaloneAsset.gameObject.SetActive(false);

#if UNITY_EDITOR
            standaloneAsset.gameObject.SetActive(true);
            platformInput = new InputStandalone(standaloneAsset);
#elif UNITY_ANDROID
            //waveAsset.SetActive(true);
            //platformInput = new InputWave(waveInputManager, waveCtrlLoader);
#endif
        }
    }
}
