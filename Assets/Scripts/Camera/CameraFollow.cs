using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    //Reference Variables
    private Camera cam = null;

    //State Variables
    private Vector3 currentPosition;
    private float yOffset = 0f;

    //Scene Change Function
    #region SceneChangedEventCalls
    private void OnEnable() {
        SceneManager.activeSceneChanged += OnSceneChange;     //Subscribe to Event Delegate
    }

    private void OnDisable() {
        SceneManager.activeSceneChanged -= OnSceneChange;     //Unsubscribe from Event Delegate
    }
    #endregion
    private void OnSceneChange(Scene oldScene, Scene newScene) {
        FindCameraObject();
    }

    //Internal Methods
    private void Awake() {
        FindCameraObject();
        SetupPositionParameters();
        SetVerticalOffset();
    }

    private void FindCameraObject() {
        cam = Camera.main;
        if (!cam) {
            Debug.LogError("No Main Camera Found!");
            enabled = false;        //Disable This Component
        }
    }

    private void SetupPositionParameters() {
        currentPosition = transform.position;
    }

    private void SetVerticalOffset() {
        yOffset = currentPosition.y;
    }

    private void LateUpdate() {
        UpdatePosition();
    }

    private void UpdatePosition() {
        currentPosition.y = cam.transform.position.y + yOffset;
        transform.position = currentPosition;
    }
}
