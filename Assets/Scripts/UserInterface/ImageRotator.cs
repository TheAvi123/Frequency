using UnityEngine;

namespace UserInterface {
    public class ImageRotator : MonoBehaviour
    {
        //Reference Variables
        private RectTransform image;

        //Configuration Parameters
        [SerializeField][Range(0f, 720f)] float rotationSpeed = 360f;
        [SerializeField] bool reverseDirection = false;

        //State Variables
        private Vector3 rotationVector;
        private float currentRotation = 0f;

        //Internal Methods
        private void Awake() {
            FindImage();
        }

        private void FindImage() {
            image = GetComponent<RectTransform>();
        }
    
        private void Start() {
            InitializeRotation();
            CheckDirection();
        }

        private void InitializeRotation() {
            rotationVector = image.eulerAngles;
            SetImageRotation();
        }

        private void CheckDirection() {
            if (reverseDirection) {
                rotationSpeed = -rotationSpeed;
            }
        }
    
        private void Update() {
            UpdateRotation();
        }

        private void UpdateRotation() {
            currentRotation += Time.deltaTime * rotationSpeed;
            SetImageRotation();
        }

        private void SetImageRotation() {
            rotationVector.z = currentRotation;
            image.eulerAngles = rotationVector;
        }
    }
}
