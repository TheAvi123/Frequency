using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    ///Reference Variables
    private PlayerWave player = null;

    ///State Variables
    private bool followingPlayer = false;
    private float cameraOffset;
    private Vector3 position;

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
        SetupPositionParameters();
        followingPlayer = false;
    }

    private void SetupPositionParameters() {
        position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void Update() {
        UpdatePosition();
    }

    private void UpdatePosition() {
        if (followingPlayer) {     
            position.y = player.transform.position.y + cameraOffset;
            transform.position = position;
        } else {    //Create Initial Delay Before Following Player
            if (player.transform.position.y > player.initialOffsetY) {
                SetupCameraOffset();
                followingPlayer = true;
            }
        }
    }

    private void SetupCameraOffset() {
        cameraOffset = transform.position.y - player.transform.position.y;
    }
}
