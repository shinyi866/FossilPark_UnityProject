using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMission
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        protected GameObject Object;

        public virtual void OpenGame(bool isOpen)
        {
            if (Object != null)
            {
                Object.SetActive(isOpen);
            }
        }
    }

}
