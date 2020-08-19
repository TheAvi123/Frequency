using UnityEngine;

namespace CameraFeatures {
    public class CameraShaker : MonoBehaviour
    {
        public static CameraShaker sharedInstance;
        
        //Reference Variables
        private Transform cameraTransform = null;

        //Configuration Parameters
        [Header("Shake Parameters")]
        [SerializeField][Range(0, 1)] float traumaLevel = 0f;
        [SerializeField] float roughnessMultiplier = 15f;           //Multiplier for Trauma Level
        [SerializeField] float distanceMultiplier = 10f;            //Multiplier for Displacement
        [SerializeField] float rotationMultiplier = 1f;             //Multiplier for Rotation
        [SerializeField] float shakeDecay = 1f;                     //Rate at which Shake Fades Out

        //State Variables
        private bool shakeActive = true;
        private float shakeTicker = 0f;

        //Internal Methods
        private void Awake() {
            SetSharedInstance();
            GetCameraTransform();
        }

        private void SetSharedInstance() {
            sharedInstance = this;
        }

        private void GetCameraTransform() {
            cameraTransform = gameObject.transform;
        }

        private void Update() {
            if (shakeActive) {
                Shake();
            }
        }

        private void Shake() {
            UpdateTimer();
            MoveAndRotate();
            DecayTrauma();
        }

        private void UpdateTimer() {
            shakeTicker += Time.deltaTime * Mathf.Pow(traumaLevel, 0.3f) * roughnessMultiplier;
        }

        private void MoveAndRotate() {
            Vector3 newPos = GeneratePerlinVector() * (distanceMultiplier * traumaLevel);
            cameraTransform.localPosition = newPos;
            cameraTransform.localRotation = Quaternion.Euler(newPos * rotationMultiplier);
        }

        private void DecayTrauma() {
            traumaLevel -= Mathf.Clamp01(Time.deltaTime * shakeDecay * traumaLevel);
            if (traumaLevel <= 0.001) {
                traumaLevel = 0;
                shakeActive = false;
                ResetPositionRotation();
            }
        }

        //Helper Methods
        private float GeneratePerlinFloat(float seed) {
            return ((Mathf.PerlinNoise(seed, shakeTicker) - 0.5f) * 2f);
        }

        private Vector3 GeneratePerlinVector() {
            return new Vector3(GeneratePerlinFloat(0), GeneratePerlinFloat(50), 0);
        }

        private void ResetPositionRotation() {
            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localEulerAngles = Vector3.zero;
        }

        //Public Methods
        public void AddCameraShake(float traumaToAdd) {
            shakeActive = true;
            traumaLevel = Mathf.Clamp01(traumaToAdd);
        }
    }
}