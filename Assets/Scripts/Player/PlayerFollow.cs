using System.Collections;
using System.Diagnostics.CodeAnalysis;

using Statistics;

using UnityEngine;

namespace Player {
    public class PlayerFollow : MonoBehaviour
    {
        //Reference Variables
        private PlayerWave player = null;
        private Transform playerTransform;
        private Transform follower;

        //Configuration Parameters
        [Header("Player")]
        [SerializeField] float followSpeed = 10f;
        [SerializeField] Vector2 playerOffset = new Vector2(0.5f, 0.25f);
        
        [Header("Zoom")]
        [SerializeField] float zoomLerpConstant = 0.12f;
        [SerializeField] float zoomFactor = 0.25f;

        //State Variables
        private bool followingPlayer = false;
        private bool playerAlive = true;
        private Vector3 targetPosition;
        
        //Offset Variables
        private float yOffset;
        private float cameraOffset;
        private Vector2 currentOffset;

        //Internal Methods
        private void OnEnable() {
            FindPlayerObject();
        }

        private void FindPlayerObject() {
            player = FindObjectOfType<PlayerWave>();
            if (player is null) {
                Debug.LogError("No Player Object Found To Follow");
                enabled = false;        //Disable This Component
            } else {
                playerTransform = player.transform;
            }
        }
        
        private void Awake() {
            GetPlayerTransform();
        }

        private void GetPlayerTransform() {
            follower = gameObject.transform;
        }
        
        private void Start() {
            playerAlive = true;
            followingPlayer = false;
            SetupPositionParameters();
            CalculatePlayerOffsets(playerOffset);
        }

        private void SetupPositionParameters() {
            targetPosition = new Vector3(follower.position.x, 0, follower.position.z);
        }

        private void CalculatePlayerOffsets(Vector2 percentCoordinates) {
            currentOffset = percentCoordinates;
            Vector2 offsetVector = Camera.main.ViewportToWorldPoint(percentCoordinates);
            yOffset = offsetVector.y;
            cameraOffset = follower.position.y - yOffset;
        }

        private IEnumerator ShiftPlayerOffset(Vector2 targetOffset, float duration) {
            Vector2 oldOffset = currentOffset;
            float timer = 0f;
            while (timer < duration) {
                Vector2 newOffset = Vector2.Lerp(oldOffset, targetOffset, timer / duration);
                timer += Time.deltaTime / Time.timeScale;
                CalculatePlayerOffsets(newOffset);
                yield return null;
            }
            CalculatePlayerOffsets(targetOffset);
        }

        private void Update() {
            UpdatePosition();
        }

        private void UpdatePosition() {
            if (playerAlive) {
                if (followingPlayer) {
                    targetPosition.y = playerTransform.position.y + cameraOffset;
                    follower.position = Vector3.Lerp(follower.position, targetPosition, followSpeed * Time.deltaTime);
                } else {    //Create Initial Delay Before Following Player
                    if (playerTransform.position.y > yOffset) {
                        SetupCameraOffset();
                        followingPlayer = true;
                        ScoreManager.sharedInstance.StartIncreasingScore();
                    }
                }
            }
        }

        private void SetupCameraOffset() {
            cameraOffset = follower.position.y - playerTransform.position.y;
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeCameraMain")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private IEnumerator ZoomAnimation(float freezeTime, Vector3 deathPosition) {
            Camera mainCam = Camera.main;
            float originalSize = mainCam.orthographicSize;
            Vector3 originalPosition = follower.localPosition;
            deathPosition.z = originalPosition.z;
            float initialTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - initialTime < freezeTime) {
                follower.localPosition = Vector3.Lerp(follower.localPosition, deathPosition, zoomLerpConstant / 3);
                mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, originalSize * zoomFactor, zoomLerpConstant / 3);
                yield return null;
            }
            while (Mathf.Abs(originalSize - mainCam.orthographicSize) > 0.01f) {
                follower.localPosition = Vector3.Lerp(follower.localPosition, originalPosition, zoomLerpConstant);
                mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, originalSize, zoomLerpConstant);
                yield return null;
            }
        }
        
        //Public Methods
        public void DeathZoomAnimation(float freezeTime, Vector3 deathPosition) {
            playerAlive = false;
            StartCoroutine(ZoomAnimation(freezeTime, deathPosition));
        }

        public void ChangeCameraOffset(Vector2 offsetCoordinates, float duration) {
            if (duration > 0) {
                StartCoroutine(ShiftPlayerOffset(offsetCoordinates, duration));
            } else {
                CalculatePlayerOffsets(offsetCoordinates);
            }
        }

        public void StopFollowingPlayer() {
            followingPlayer = false;
        }
    }
}
