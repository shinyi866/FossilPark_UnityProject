using Hsinpa.Ctrl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hsinpa.Input {

    public class InputWrapper : MonoBehaviour
    {
        [HideInInspector]
        public Camera arCamera;

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
            //If is set already
            if (arCamera != null) return;

            arCamera = Camera.main;
            platformInput = new InputStandalone(arCamera);
        }
    }
}
