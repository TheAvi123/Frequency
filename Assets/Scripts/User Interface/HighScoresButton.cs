using UnityEngine;

public class HighScoresButton : MonoBehaviour
{
    private void OnMouseDown() {
        GameStateManager.sharedInstance.ShowHighScores();
    }
}
