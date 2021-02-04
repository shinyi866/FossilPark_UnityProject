using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace GameMission
{
    public class Game3 : Game
    {
        public GameObject basket;
        public GameObject fruitPrefab;
        public GameObject throwPosition;
        public GameObject handBall;
        public Animator monleyAnimator;

        public System.Action<bool> gameOverEvent;

        private Camera _camera;
        private ARGameModal gameModal;
        private bool isGameStart;
        private bool isThrowing;
        private int fruit = 8;
        private int count;
        private GameObject _ball;

        // set thorw ball parameter
        private float Xmin = -0.4f;
        private float Xmax = 0.2f;
        private int speed = 135;
        private int passCount = 4;

        // unsupport AR
        private Button leftButton;
        private Button rightButton;
        public GameObject walls;

        public void Init()
        {
            gameModal = GameModals.instance.GetModal<ARGameModal>();
            _camera = CameraCtrl.instance.GetCurrentCamera();
            handBall.SetActive(false);
            count = fruit;
            leftButton = gameModal.leftButton;
            rightButton = gameModal.rightButton;

            var _cameraFront = _camera.transform.forward;
            _cameraFront.y = -0.8f;
            _cameraFront.z = 1f;
            basket.transform.position = _camera.transform.position + _cameraFront;

            
            if (MainApp.Instance.isARsupport)
            {
                SetBasketPosition();
            }
            else
            {
                UnsupportAR();
            }
        }

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

        private void SetBasketPosition()
        {
            var _cameraFront = _camera.transform.forward;
            var isSupportAR = MainApp.Instance.isARsupport;

            _cameraFront.y = -0.8f;
            _cameraFront.z = 1f;

            basket.transform.position = _camera.transform.position + _cameraFront;
        }

        public void GameStart()
        {
            isGameStart = true;            
            StartCoroutine(StartThrowBall());
        }

        private IEnumerator StartThrowBall()
        {           
            yield return new WaitForSeconds(1);

            for (int i = 0; i < fruit; i++)
            {
                isThrowing = true;
                monleyAnimator.SetBool("throw", true);
                GameObject ball = Instantiate(fruitPrefab, throwPosition.transform.position, throwPosition.transform.rotation);
                _ball = ball;

                yield return new WaitForSeconds(8f);

                count -= 1;
                Destroy(ball);
            }

            StopAllCoroutines();
        }

        public void ThrowOut()
        {
            //GameObject ball = Instantiate(fruitPrefab, throwPosition.transform.position, throwPosition.transform.rotation);
            _ball.SetActive(true);
            _ball.GetComponent<Rigidbody>().AddForce((throwPosition.transform.forward + new Vector3(Random.Range(Xmin, Xmax), 2, 0)) * speed); // Random.Range(Xmin, Xmax)
            _ball = null;
            
            
            handBall.SetActive(false);
            gameModal.countText.text = "剩餘果子數： " + count.ToString();
            Debug.Log("2剩餘果子數： " + count);
        }

        private void GameResult(bool isSuccess)
        {
            if (gameOverEvent != null)
                gameOverEvent(isSuccess);
        }

        private bool AnimatorIsEnd()
        {
            return monleyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Throw") && monleyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        }

        private void Update()
        {
            if (!isGameStart) return;

            gameModal.getFruitText.text = "接到果子數： " + CatchFruit.fruitCount.ToString();

            if (MainApp.Instance.isARsupport) { SetBasketPosition(); }

            if (AnimatorIsEnd() && isThrowing)
            {
                monleyAnimator.SetBool("throw", false);
                isThrowing = false;
                handBall.SetActive(true);
                Debug.Log("end");
            }

            if (count == 0)
            {
                bool isSuccess = CatchFruit.fruitCount > passCount;
                GameResult(isSuccess);
                isGameStart = false;
            }
        }

    }
}
