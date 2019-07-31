using UnityEngine;

public class StartButton : MonoBehaviour
{
    private void OnMouseDown() {
        GameStateManager.sharedInstance.StartGame();
    }
}
