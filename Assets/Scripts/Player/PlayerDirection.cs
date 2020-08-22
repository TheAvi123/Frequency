using UnityEngine;

namespace Player {
    public class PlayerDirection : MonoBehaviour
    {
        //Reference Variables
        private Transform playerTransform;
        
        //Configuration Parameters
        [SerializeField] float rotationOffset = -90f;

        //State Variables
        private Vector2 currentPosition;
        private Vector2 previousPosition;
        private Vector2 currentDirection;
        private float directionAngle;

        //Internal Methods
        private void Awake() {
            GetPlayerTransform();
        }

        private void GetPlayerTransform() {
            playerTransform = gameObject.transform;
        }

        private void Update() {
            UpdatePositionAndDirection();
            SetSpriteRotation();
        }

        private void UpdatePositionAndDirection() {
            previousPosition = currentPosition;
            currentPosition = playerTransform.position;
            if (currentPosition != previousPosition) {
                currentDirection = (currentPosition - previousPosition).normalized;
                directionAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            }
        }

        private void SetSpriteRotation() {
            playerTransform.eulerAngles = new Vector3(0, 0, directionAngle + rotationOffset);
        }
    }
}
