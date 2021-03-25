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
            var model = _instance.OpenModal<TitleModal>();
            model.ShowInfo(index, TypeFlag.TitleType.RoundTitleNotify);
        }

        // Show and enter missions (1-2m) mission0-1
        public void ShowNotifyOther(int index)
        {
            var model = _instance.OpenModal<TitleModal>();
            model.ShowInfo(index, TypeFlag.TitleType.EnterGame0);
            model.Game0ConfirmButton.onClick.AddListener(() =>
            {
                Debug.Log("Game 0");
                GuideModel(index);
            });
        }

        // Show and enter missions (1-2m) mission2-8
        public void ShowNotify(int index)
        {
            var model = _instance.OpenModal<TitleModal>();
            model.ShowInfo(index, TypeFlag.TitleType.EnterTitle);
            model.ConfirmButton.onClick.AddListener(() =>
            {
                EnterModel(index);
                GameMissions.instance.EnterGame();
            });
        }

        private void EnterModel(int index)
        {
            var model = _instance.OpenModal<DialogModal>();
            model.ShowInfo(index, TypeFlag.DialogType.EnterDialog);
            model.NextButton.onClick.AddListener(() => StartDialogModel(index));
        }

        private void StartDialogModel(int index)
        {
            var model = _instance.OpenModal<TitleModal>();
            model.ShowInfo(index, TypeFlag.TitleType.GameTitle);
            model.ConfirmButton.onClick.AddListener(() => GuideModel(index));
        }

        private void GuideModel(int index)
        {
            var model = _instance.OpenModal<PictureModal>();
            model.ShowInfo(index, TypeFlag.PictureType.GuideType);

            model.GuideConfirmButtonOne.onClick.AddListener(()=>
            {
                CloseModal();
                GameMissions.instance.StartGame();
            });
        }
    }
}

