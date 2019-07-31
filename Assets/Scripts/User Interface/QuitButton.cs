using UnityEngine;

public class QuitButton : MonoBehaviour
{
    private void OnMouseDown() {
        GameStateManager.sharedInstance.QuitGame();
    }
}
