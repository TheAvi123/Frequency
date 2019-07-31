using UnityEngine;

public class RestartButton : MonoBehaviour
{
    private void OnMouseDown() {
        GameStateManager.sharedInstance.RestartGame();
    }
}
