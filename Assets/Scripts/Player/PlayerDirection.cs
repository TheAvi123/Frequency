using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
    //Configuration Parameters
    [SerializeField] float rotationOffset = -90f;

    //State Variables
    private Vector2 currentPosition;
    private Vector2 previousPosition;
    private Vector2 currentDirection;
    private float directionAngle;

    //Internal Methods
    private void Update() {
        UpdatePositionAndDirection();
        SetSpriteRotation();
    }

    private void UpdatePositionAndDirection() {
        previousPosition = currentPosition;
        currentPosition = gameObject.transform.position;
        if (currentPosition != previousPosition) {
            currentDirection = (currentPosition - previousPosition).normalized;
            directionAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        }
    }

    private void SetSpriteRotation() {
        gameObject.transform.eulerAngles = new Vector3(0, 0, directionAngle + rotationOffset);
    }
}
