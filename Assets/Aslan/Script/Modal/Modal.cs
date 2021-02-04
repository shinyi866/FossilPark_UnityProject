using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View
{
    public class Modal : MonoBehaviour
    {
        [SerializeField]
        protected CanvasGroup canvasGroup;

        public virtual void Show(bool isShow)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = (isShow) ? 1 : 0;
                canvasGroup.interactable = isShow;
                canvasGroup.blocksRaycasts = isShow;
            }
        }
    }
}

