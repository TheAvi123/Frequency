using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    private void OnMouseDown() {
        GameStateManager.sharedInstance.OpenOptions();
    }
}
