using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class EggModal : Modal
    {
        [SerializeField]
        private Button BackButton;

        [SerializeField]
        private Button ARButton;

        private void Awake()
        {
            BackButton.onClick.AddListener(() =>
            {
                Modals.instance.CloseModal(); // TODO error?
            });

            ARButton.onClick.AddListener(() =>
            {
                //Modals.instance.OpenAR();
            });
        }
    }
}
