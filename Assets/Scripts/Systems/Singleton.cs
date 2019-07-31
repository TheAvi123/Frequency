using UnityEngine;

public class Singleton : MonoBehaviour
{
    [SerializeField] Component component = null;

    private void Awake() {
        var objects = FindObjectsOfType(component.GetType());
        if (objects.Length > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }
}
