using UnityEngine;

public class PlayButton : MonoBehaviour
{
    private void OnMouseUp() {
        GameStateManager.sharedInstance.PlayGame();
    }
}
