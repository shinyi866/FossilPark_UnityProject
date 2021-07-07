using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using View;

namespace GameMission
{
    public class Games : MonoBehaviour
    {
        private Game[] games;

        private static Games _instance;

        public static Games instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<Games>();
                    _instance.SetUp();
                }

                return _instance;
            }
        }

        private Game currentGame;

        public void SetUp()
        {
            games = GetComponentsInChildren<Game>();
        }

        public T GetGame<T>() where T : Game
        {
            return games.First(x => typeof(T) == x.GetType()) as T;
        }

        public T OpenGame<T>() where T : Game
        {
            if (games == null) return null;

            Game targetModal = null;

            foreach (Game modal in games)
            {

                if (typeof(T) == modal.GetType())
                {
                    targetModal = modal;
                    targetModal.OpenGame(true);
                }

            }

            currentGame = targetModal;

            return targetModal as T;
        }

        public void ClosGame()
        {
            if (currentGame != null)
            {
                currentGame.Init();
                currentGame.OpenGame(false);
            }

            GameModals.instance.CloseAllModal();
            GameModals.instance.CloseAR();
        }
    }
}

