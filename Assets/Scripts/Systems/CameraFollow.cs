using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    ///Reference Variables
    private Camera cam = null;

    ///State Variables
    private Vector3 position;

    #region OnSceneLoadDelegateCalls   
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    #endregion
    void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        FindCameraObject();
        SetupPositionParameters();
    }

    private void FindCameraObject() {
        cam = Camera.main;
        if (!cam) {
            Debug.LogError("No Main Camera Found!");
            enabled = false;        //Disable This Component
        }
    }

    private void SetupPositionParameters() {
        position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void Update() {
        UpdatePosition();
    }

    private void UpdatePosition() {
        position.y = cam.transform.position.y;
        transform.position = position;
    }
}
