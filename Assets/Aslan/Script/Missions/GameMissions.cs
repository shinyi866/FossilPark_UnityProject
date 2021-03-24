using UnityEngine;
using System.Linq;
using View;

namespace GameMission
{
    public class GameMissions : MonoBehaviour
    {
        private Mission[] missions;

        private static GameMissions _instance;

        private Mission currentMission;
        private int currentIndex;

        public static GameMissions instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameMissions>();
                    _instance.SetUp();
                }

                return _instance;
            }
        }

        private void SetUp()
        {
            missions = GetComponentsInChildren<Mission>();
        }

        public T GetMission<T>() where T : Mission
        {
            return missions.First(x => typeof(T) == x.GetType()) as T;
        }

        public void ShowMission(int index)
        {
            if (missions == null) return;

            currentMission = missions[index];
            currentIndex = index;

            if (index == 0 || index == 1)
                GameModals.instance.ShowNotifyOther(index);
            else
                GameModals.instance.ShowNotify(index);
        }

        public void EnterGame()
        {
            currentMission.EnterGame();
        }

        public void StartGame()
        {
            currentMission.StartGame();
        }
    }
}


