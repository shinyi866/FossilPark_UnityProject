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
        private int fruit = 5;
        private int count;
        private int currentFruitIndex;
        private float time = 3;
        private GameObject _ball;
        private GameObject currentBall;

        // set thorw ball parameter
        private float Xmin = -0.4f;
        private float Xmax = 0.2f;
        private int speed = 174;
        private int passCount = 1;

        // unsupport AR
        private Button leftButton;
        private Button rightButton;
        public GameObject walls;

        public void Init()
        {
            gameModal = GameModals.instance.GetModal<ARGameModal>();
            _camera = CameraCtrl.instance.GetCurrentCamera();
            monleyAnimator = monkey.GetComponent<Animator>();
            playableDirector = monkey.GetComponent<PlayableDirector>();
            monkeyScene.transform.rotation = Compass.Instance.transform.rotation;

            count = fruit;
            leftButton = gameModal.game3Panel.leftButton;
            rightButton = gameModal.game3Panel.rightButton;

            _camera = CameraCtrl.instance.GetCurrentCamera();
            _camera.transform.position = Compass.Instance.transform.position;

            /*
            var _cameraFront = _camera.transform.forward;
            _cameraFront.y = -0.8f;
            _cameraFront.z = 1f;
            basket.transform.position = _camera.transform.position + _cameraFront;
            */
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
                Object.transform.rotation = Compass.Instance.transform.rotation;
                //basket.transform.rotation = Compass.Instance.transform.rotation;
                time -= Time.deltaTime;
            }
        }

        private void SetBasketPosition()
        {
            var _cameraFront = _camera.transform.forward;

            _cameraFront.y = -1f;
            //_cameraFront *= 1f;

            basket.transform.position = _camera.transform.position + _cameraFront;
            basket.transform.rotation = _camera.transform.rotation;
        }

        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }

        private void Update()
        {
            if (!isGameStart) return;

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

            if (isARStart)
            {
                SetBasketPosition();
            }
        }

        // Unsupport AR Game
        private void UnsupportAR()
        {
            var left = 0f;
            var right = 0f;
            // set basket position
            var _cameraFront = _camera.transform.forward;
            _cameraFront.y = -0.8f;
            basket.transform.position = _camera.transform.position + _cameraFront;

            walls.SetActive(true);

            leftButton.onClick.AddListener(() =>
            {
                var moviePosition = basket.transform.position;
                left -= 0.3f;
                Debug.Log("left " + left);
                moviePosition.x = left;
                basket.transform.position = Vector3.Lerp(basket.transform.position, moviePosition, 3 * Time.deltaTime);
            });

            rightButton.onClick.AddListener(() =>
            {
                var moviePosition = basket.transform.position;
                right += 0.3f;
                Debug.Log("right " + right);
                moviePosition.x = right;
                basket.transform.position = Vector3.Lerp(basket.transform.position, moviePosition, 3 * Time.deltaTime);
            });
        }
    }
}
