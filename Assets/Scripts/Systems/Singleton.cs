using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    //Reference Variables
    [SerializeField] MonoBehaviour[] scripts = null;

    //Internal Methods
    private void Awake() {
        ScriptCheck();
        SingletonCheck();
    }

    private void ScriptCheck() {
        if (scripts.Length == 0) {
            Debug.LogWarning("No Scripts Attached to Singleton... Disabling Singleton");
            enabled = false;
        }
    }

    private void SingletonCheck() {
        foreach (MonoBehaviour script in scripts) {
            var objects = FindObjectsOfType(script.GetType());
            if (objects.Length > 1) {
                gameObject.SetActive(false);
                Destroy(gameObject);
            } else {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    //Scene Change Function
    #region SceneChangedEventCalls
    private void OnEnable() {
        SceneManager.activeSceneChanged += SceneChanged;     //Subscribe to Event Delegate
    }

    private void OnDisable() {
        SceneManager.activeSceneChanged -= SceneChanged;     //Unsubscribe from Event Delegate
    }
    #endregion
    private void SceneChanged(Scene oldScene, Scene newScene) {
        foreach (MonoBehaviour script in scripts) {
            script.Invoke("OnSceneChange", 0);
        }
    }
}
