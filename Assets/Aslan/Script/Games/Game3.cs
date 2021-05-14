using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using View;

namespace GameMission
{
    public class Game3 : Game
    {
        public GameObject basket;
        public GameObject[] fruitPrefabs;
        public GameObject throwPosition;
        public GameObject handBall;
        public GameObject monkey;
        public GameObject monkeyScene;

        public System.Action<bool> gameOverEvent;
        private int missionIndex = 3;

        private Camera _camera;
        private ARGameModal gameModal;
        private Animator monleyAnimator;
        private PlayableDirector playableDirector;

        private bool isGameStart;
        private bool isUnARStart;
        private bool isARStart;
        private bool isThrowing;
        private bool isCreateHandBall;
        private int fruit = 3;
        private int count;
        private int currentFruitIndex;
        private float time = 3;
        private GameObject _ball;
        private GameObject currentBall;

        // set thorw ball parameter
        private float Xmin = -0.2f;
        private float Xmax = 0.1f;
        private int speed = 197;
        private int passCount = 1;

        // unsupport AR
        public GameObject tools;

        public void Init()
        {
            gameModal = GameModals.instance.GetModal<ARGameModal>();
            _camera = CameraCtrl.instance.GetCurrentCamera();
            monleyAnimator = monkey.GetComponent<Animator>();
            playableDirector = monkey.GetComponent<PlayableDirector>();

            count = fruit;
            //leftButton = gameModal.game3Panel.leftButton;
            //rightButton = gameModal.game3Panel.rightButton;
        }

        public void GameStart()
        {
            isGameStart = true;
            monkey.SetActive(true);

            if (MainApp.Instance.isARsupport)
            {
                isARStart = true;
                GameModals.instance.OpenModal<ARGameModal>();
                gameModal.ShowModal(missionIndex, TypeFlag.ARGameType.Game3);
                SetBasketPosition();
            }
            else
            {
                isUnARStart = true;

                UnsupportAR();
            }
        }

        private void ResetDirection()
        {
            if (time > 0)
            {
                Compass.Instance.SetUp(Object, 83);
                time -= Time.deltaTime;
            }
        }

        private void SetBasketPosition()
        {
            var _cameraFront = _camera.transform.forward;

            _cameraFront.y = -1f;

            basket.transform.position = _camera.transform.position + _cameraFront;
        }

        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }

        private void Update()
        {
            if (!isGameStart) return;

            SetBasketPosition();

            if (isARStart)
                ResetDirection();

            gameModal.game3Panel.text.text = CatchFruit.fruitCount.ToString();// "接到果子數： " + CatchFruit.fruitCount.ToString();

            if (TriggerFruitPlane.fruitTouchPlane == fruit)
            {
                bool isSuccess = CatchFruit.fruitCount >= passCount;
                GameResult(isSuccess);
                isGameStart = false;
            }
            else
            {
                if (playableDirector.time < 0.5f && !isCreateHandBall)
                {
                    count -= 1;
                    currentFruitIndex = Random.Range(0, fruitPrefabs.Length);
                    currentBall = Instantiate(fruitPrefabs[currentFruitIndex], handBall.transform);
                    currentBall.transform.SetParent(handBall.transform);
                    isThrowing = false;
                    isCreateHandBall = true;
                }

                if (playableDirector.time >= 7.2f && !isThrowing)
                {
                    GameObject ball = Instantiate(fruitPrefabs[currentFruitIndex], throwPosition.transform);
                    ball.AddComponent<Rigidbody>();
                    ball.GetComponent<Rigidbody>().AddForce((throwPosition.transform.forward + new Vector3(Random.Range(Xmin, Xmax), 2, 0)) * speed);
                    Destroy(currentBall);
                    isCreateHandBall = false;
                    isThrowing = true;
                }
            }
        }

        // Unsupport AR Game
        private void UnsupportAR()
        {
            var _cameraFront = _camera.transform.forward;
            _cameraFront.y = -0.8f;
            basket.transform.position = _camera.transform.position + _cameraFront;

            tools.SetActive(true);
        }
    }
}
