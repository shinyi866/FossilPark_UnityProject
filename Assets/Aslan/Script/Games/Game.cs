using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMission
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        protected GameObject Object;

        public bool isGameStart;

        public virtual void OpenGame(bool isOpen)
        {
            if (Object != null)
            {
                Object.SetActive(isOpen);
            }
        }

        public GameObject GetGameObject()
        {
            return Object;
        }

        public void Init()
        {
            Object.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

}
