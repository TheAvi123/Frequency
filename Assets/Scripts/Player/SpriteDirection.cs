using UnityEngine;

public class SpriteDirection : MonoBehaviour
{
    //Reference Variables
    private SpriteRenderer playerSprite = null;

    //Configuration Parameters
    [SerializeField] float rotationOffset = -90f;

    //State Variables
    private Vector2 currentPosition;
    private Vector2 previousPosition;
    private Vector2 currentDirection;
    private float directionAngle;

    //Internal Methods
    private void Awake() {
        FindPlayerSprite();
    }

    private void FindPlayerSprite() {
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        if (!playerSprite) {
            Debug.LogError("No Sprite Renderer Found on The Player");
            enabled = false;        //Disable This Component
        }
    }

    private void Update() {
        UpdatePositionAndDirection();
        SetSpriteRotation();
    }

    private void UpdatePositionAndDirection() {
        previousPosition = currentPosition;
        currentPosition = gameObject.transform.position;
        currentDirection = (currentPosition - previousPosition).normalized;
        directionAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
    }

    private void SetSpriteRotation() {
        playerSprite.transform.eulerAngles = new Vector3(0, 0, directionAngle + rotationOffset);
    }
}
