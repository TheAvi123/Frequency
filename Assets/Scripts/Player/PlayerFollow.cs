using System.Collections;
using System.Diagnostics.CodeAnalysis;

using Statistics;

using UnityEngine;

namespace Player {
    public class PlayerFollow : MonoBehaviour
    {
        //Reference Variables
        private PlayerWave player = null;

        //Configuration Parameters
        [SerializeField] float lerpConstant = 0.12f;
        [SerializeField] float zoomFactor = 0.25f;

        //State Variables
        private bool followingPlayer = false;
        private bool playerAlive = true;
        private Vector3 currentPosition;
        private float cameraOffset;

        //Internal Methods
        private void OnEnable() {
            FindPlayerObject();
        }

        private void FindPlayerObject() {
            player = FindObjectOfType<PlayerWave>();
            if (!player) {
                Debug.LogError("No Player Object Found To Follow");
                enabled = false;        //Disable This Component
            }
        }

        private void Start() {
            playerAlive = true;
            followingPlayer = false;
            SetupPositionParameters();
        }

        private void SetupPositionParameters() {
            currentPosition = new Vector3(transform.position.x, 0, transform.position.z);
        }

        private void Update() {
            UpdatePosition();
        }

        private void UpdatePosition() {
            if (playerAlive) {
                if (followingPlayer) {
                    currentPosition.y = player.transform.position.y + cameraOffset;
                    transform.localPosition = currentPosition;
                } else {    //Create Initial Delay Before Following Player
                    if (player.transform.position.y > player.initialOffsetY) {
                        SetupCameraOffset();
                        followingPlayer = true;
                        ScoreManager.sharedInstance.StartIncreasingScore();
                    }
                }
            }
        }

        private void SetupCameraOffset() {
            cameraOffset = transform.position.y - player.transform.position.y;
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeCameraMain")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private IEnumerator ZoomAnimation(float freezeTime, Vector3 deathPosition) {
            Camera mainCam = Camera.main;
            float originalSize = mainCam.orthographicSize;
            Vector3 originalPosition = transform.localPosition;
            deathPosition.z = originalPosition.z;
            float initialTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - initialTime < freezeTime) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, deathPosition, lerpConstant / 3);
                mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, originalSize * zoomFactor, lerpConstant / 3);
                yield return null;
            }
            while (Mathf.Abs(originalSize - mainCam.orthographicSize) > 0.01f) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, lerpConstant);
                mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, originalSize, lerpConstant);
                yield return null;
            }
        }

        //Public Methods
        public void DeathZoomAnimation(float freezeTime, Vector3 deathPosition) {
            playerAlive = false;
            StartCoroutine(ZoomAnimation(freezeTime, deathPosition));
        }
    }
}
