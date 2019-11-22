using UnityEngine;

public class QuitButton : MonoBehaviour
{
    private void OnMouseUp() {
        GameStateManager.sharedInstance.QuitGame();
    }
}
