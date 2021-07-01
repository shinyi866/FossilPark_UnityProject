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
        public GameObject alertObject;

        public System.Action<bool> gameOverEvent;
        private int missionIndex = 3;

        private Camera _camera;
        private ARGameModal gameModal;
        private PlayableDirector playableDirector;

        private bool isGameStart;
        private bool isARStart;
        private bool isThrowing;
        private bool isCreateHandBall;
        private int fruit = 3;
        private int count;
        private int currentFruitIndex;
        private float time = 3;
        private float alertTime = 1;
        private GameObject _ball;
        private GameObject currentBall;

        // set thorw ball parameter
        private float Xmin = -0.2f;
        private float Xmax = 0.1f;
        private int speed = 185;
        private int passCount = 1;

        private string videoPath = "Video/monkey.mp4";

        // unsupport AR
        public GameObject tools;

        public void Init()
        {
            gameModal = GameModals.instance.GetModal<ARGameModal>();
            _camera = CameraCtrl.instance.GetCurrentCamera();
            playableDirector = monkey.GetComponent<PlayableDirector>();

            if(!MainApp.Instance.isARsupport)
                MediaPlayerController.instance.LoadVideo(videoPath);

            count = fruit;
            //leftButton = gameModal.game3Panel.leftButton;
            //rightButton = gameModal.game3Panel.rightButton;
        }

        public void GameStart()
        {
            isGameStart = true;
            monkey.SetActive(true);
            GameModals.instance.OpenModal<ARGameModal>();
            gameModal.ShowModal(missionIndex, TypeFlag.ARGameType.Game3);

            if (MainApp.Instance.isARsupport)
            {
                isARStart = true;
                SetBasketPosition();
            }
            else
            {
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

            if(time < 0 && alertTime > 0)
            {
                alertTime -= Time.deltaTime;
                alertObject.transform.position = new Vector3(monkey.transform.position.x, alertObject.transform.position.y, monkey.transform.position.z);
                alertObject.SetActive(true);
            }
        }

        private void SetBasketPosition()
        {
            var _frontPos = _camera.transform.forward * 1.9f;
            var _upPos = _camera.transform.up * -1.1f;

            basket.transform.position = _camera.transform.position + _frontPos + _upPos;
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
            else
            {
                _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0);

                if (_camera.transform.position.x < -12)
                {
                    _camera.transform.position = new Vector3(-12f, _camera.transform.position.y, _camera.transform.position.z);
                }

                if (_camera.transform.position.x > 5)
                {
                    _camera.transform.position = new Vector3(5f, _camera.transform.position.y, _camera.transform.position.z);
                }
            }

            gameModal.game3Panel.text.text = CatchFruit.fruitCount.ToString();// "接到果子數： " + CatchFruit.fruitCount.ToString();

            if (TriggerFruitPlane.fruitTouchPlane == fruit)
            {
                if (!MainApp.Instance.isARsupport)
                {
                    _camera.transform.position = new Vector3(0, 0, 0);
                    tools.SetActive(false);
                }

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
            speed = 180;

            var _frontPos = _camera.transform.forward;
            var _upPos = _camera.transform.up;

            monkeyScene.transform.position = _camera.transform.position + _frontPos + _upPos * 1.2f;
            basket.transform.position = _camera.transform.position + _frontPos * 1.9f + _upPos * -1.1f;
            tools.SetActive(true);
        }
    }
}
