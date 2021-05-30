using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace View
{
    public class Modals : MonoBehaviour
    {

        private Modal[] modals;

        private static Modals _instance;

        public static Modals instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<Modals>();
                    _instance.SetUp();
                }

                return _instance;
            }
        }

        private Modal currentModal;

        public void SetUp()
        {
            modals = GetComponentsInChildren<Modal>();
        }

        public T GetModel<T>() where T : Modal
        {
            return modals.First(x => typeof(T) == x.GetType()) as T;
        }

        public T OpenModal<T>() where T : Modal
        {
            if (modals == null) return null;

            Modal targetModal = null;

            foreach(Modal modal in modals)
            {

                if (typeof(T) == modal.GetType())
                {
                    targetModal = modal;
                    targetModal.Show(true);
                }
                
            }

            currentModal = targetModal;

            return targetModal as T;
        }

        public void CloseModal()
        {
            if(currentModal != null)
                currentModal.Show(false);
        }

        public void CloseAllModal()
        {
            foreach (Modal modal in modals) { modal.Show(false); }
        }

        public void OpenAR(int index, TypeFlag.ARObjectType type)
        {
            foreach (Modal modal in modals) { modal.Show(false); }

            CameraCtrl.instance.SwitchCamera(true);

            ARModal arModal = _instance.GetModel<ARModal>(); // call ARModal direct, will not change currentModal(last modal)
            arModal.ShowView(true);
            arModal.Show(true);

            if (type == TypeFlag.ARObjectType.DinosaurlBaby)
                arModal.feedButtonGameObject.SetActive(true);

            PlaceARObject.instance.EnterAR(index, type);
        }

        public void OpenNotSupportAR(int index, TypeFlag.ARObjectType type)
        {
            foreach (Modal modal in modals) { modal.Show(false); }

            CameraCtrl.instance.SwitchCamera(true);

            if (index != 6)
                MediaPlayerController.instance.LoadAndPlayVideo("Video/scence_360.mp4");
            else
                MediaPlayerController.instance.LoadVideo("Video/dolphin360.mp4");

            ARModal arModal = _instance.GetModel<ARModal>(); // call ARModal direct, will not change currentModal(last modal)
            arModal.ShowView(true);
            arModal.Show(true);

            if (type == TypeFlag.ARObjectType.DinosaurlBaby)
                arModal.feedButtonGameObject.SetActive(true);

            PlaceARObject.instance.EnterNoAR(index, type);
        }

        public void CloseARInMain()
        {
            foreach (Modal modal in modals) { modal.Show(false); }

            if (currentModal == null) return;
            currentModal.Show(true);

            CameraCtrl.instance.SwitchCamera(false);
            MediaPlayerController.instance.CloseVideo();
            MainModal mainModal = _instance.GetModel<MainModal>();
            mainModal.Show(true);

            PlaceARObject.instance.CloseAR();
        }

        public void CloseARInGame()
        {
            foreach (Modal modal in modals) { modal.Show(false); }

            CameraCtrl.instance.SwitchCamera(false);
            MediaPlayerController.instance.CloseVideo();
            MainModal mainModal = _instance.GetModel<MainModal>();
            mainModal.Show(true);

            PlaceARObject.instance.CloseAR();
        }
    }
}
