using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameMission;

namespace View
{
    public class GameModals : MonoBehaviour
    {
        private Modal[] modals;

        private static GameModals _instance;

        private Modal currentModal;

        public static GameModals instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<GameModals>();
                    _instance.SetUp();
                }

                return _instance;
            }
        }

        private void SetUp()
        {
            modals = GetComponentsInChildren<Modal>();
        }

        public T GetModal<T>() where T : Modal
        {
            return modals.First(x => typeof(T) == x.GetType()) as T;
        }

        public T OpenModal<T>() where T : Modal
        {
            if (modals == null) return null;

            Modal targetModal = null;

            foreach(Modal modal in modals)
            {
                modal.Show(false);

                if (typeof(T) == modal.GetType())
                {
                    targetModal = modal;
                    targetModal.Show(true);
                }

                currentModal = targetModal;
            }

            return targetModal as T;
        }

        public void CloseModal()
        {
            if (currentModal != null)
                currentModal.Show(false);
        }

        public void CloseAllModal()
        {
            foreach(Modal modal in modals){ modal.Show(false); }
        }

        public void OpenAR()
        {
            Modals.instance.CloseAllModal();
            CameraCtrl.instance.SwitchCamera(true);
        }

        public void GetBackAnimalAR(int index, TypeFlag.ARObjectType type)
        {
            foreach (Modal modal in modals) { modal.Show(false); }

            CameraCtrl.instance.SwitchCamera(true);

            var mainModal = Modals.instance.GetModel<MainModal>();
            mainModal.GetBackAnimal(index);

            ARModal arModal = Modals.instance.GetModel<ARModal>(); // call ARModal direct, will not change currentModal(last modal)
            arModal.ShowView(false);
            arModal.Show(true);

            PlaceARObject.instance.EnterAR(index, type);
        }

        // Round missions notify (3-8m)
        public void RoundNotify(int index)
        {
            var model = _instance.OpenModal<NotifyModal>();
            model.ShowInfo(index, TypeFlag.NotifyType.RoundNotify);
        }
        // Show and enter missions (1-2m)
        public void ShowNotify(int index)
        {
            var model = _instance.OpenModal<NotifyModal>();
            model.ShowInfo(index, TypeFlag.NotifyType.EnterNotify);
            model.ConfirmButton.onClick.AddListener(() => EnterMessage(index));
        }

        private void EnterMessage(int index)
        {
            var model = _instance.OpenModal<DialogModal>();
            model.ShowInfo(index, TypeFlag.DialogType.EnterDialog);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                CloseModal();
                GameMissions.instance.EnterGame();
            });
        }
    }
}

