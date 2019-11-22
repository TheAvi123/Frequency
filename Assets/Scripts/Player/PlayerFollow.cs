using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    //Reference Variables
    private PlayerWave player = null;

    //State Variables
    private bool followingPlayer = false;
    private Vector3 currentPosition;
    private float cameraOffset;

    //Internal Methods
    void OnEnable() {
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

    private void SetupCameraOffset() {
        cameraOffset = transform.position.y - player.transform.position.y;
    }
}
