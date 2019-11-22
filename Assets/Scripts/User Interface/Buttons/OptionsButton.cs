using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    private void OnMouseUp() {
        GameStateManager.sharedInstance.OpenOptions();
    }
}
