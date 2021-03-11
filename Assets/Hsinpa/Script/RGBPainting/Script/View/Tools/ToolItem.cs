using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Expect.View
{
    public class ToolItem : MonoBehaviour
    {
        [SerializeField]
        private Text tipText;

        [SerializeField]
        private ParticleSystem _particleSystem;

        [SerializeField]
        private Vector3 _rotationOffset;

        [SerializeField]
        private Vector3 _positionOffset;

        private Vector3 _originalPos;
        private Quaternion _originalRot;
        private Transform _originalParent;

        private float recordTime;
        private int delayTime = 5;

        private void Start()
        {
            _originalPos = transform.position;
            _originalRot = transform.rotation;
            _originalParent = transform.parent;
        }

        public void Return() {
            transform.SetParent(_originalParent);
            transform.position = _originalPos;
            transform.rotation = _originalRot;
            EnableParticles(false);
        }

        public void PairToParent(Transform parentObject) {
            this.transform.SetParent(parentObject);
            this.transform.localPosition = _positionOffset;
            this.transform.localRotation = Quaternion.Euler(parentObject.transform.forward + _rotationOffset);
        }

        public void ShowTipIntruction(string message) {
            if (tipText == null) return;

            if (string.IsNullOrEmpty(message)) {
                tipText.enabled = false;
                return;
            }

            tipText.enabled = true;
            tipText.text = message;

            recordTime = Time.time + delayTime - 0.1f;

            CloseSiblingMsg();

            _ = Utility.UtilityMethod.DoDelayWork(delayTime, () =>
            {
                if (Time.time > recordTime)
                    tipText.enabled = false;
            });
        }

        public void EnableParticles(bool isEnable) {
            if (_particleSystem == null) return;
            
            if (isEnable && _particleSystem.isStopped)
                _particleSystem.Play();
            
            if (!isEnable && _particleSystem.isPlaying)
                _particleSystem.Stop();
        }

        private void CloseSiblingMsg() {
            foreach (Transform child in transform.parent) {
                var toolItem = child.GetComponent<ToolItem>();
                if (toolItem != null && toolItem.name != this.name)
                    toolItem.ShowTipIntruction("");
            }
        }

    }
}