using UnityEngine;

public class ScoresButton : MonoBehaviour
{
    private void OnMouseUp() {
        GameStateManager.sharedInstance.ShowScores();
    }
}
