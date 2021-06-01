using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class DinosaurlModal : Modal
    {
        [SerializeField]
        public Button[] dinosaurlButtons;
        [SerializeField]
        private GameObject promptObject;

        [SerializeField]
        private Button BackButton;

        private void Awake()
        {
            BackButton.onClick.AddListener(() =>
            {
                Modals.instance.CloseModal(); // TODO error?
            });

            Setup();
        }

        public void Setup()
        {
            for (int i = 0; i < dinosaurlButtons.Length; i++)
            {
                int closureIndex = i;
                dinosaurlButtons[closureIndex].onClick.AddListener(() =>
                {
                    var modal = Modals.instance.OpenModal<InfoModal>();
                    modal.ShowInfo(closureIndex, TypeFlag.InfoType.Dinosaurl);
                });
            }
        }

        public void BackToDinosaurl(bool back)
        {
            foreach (var btn in dinosaurlButtons) { btn.interactable = back; }
            promptObject.SetActive(!back);
        }
    }
}
