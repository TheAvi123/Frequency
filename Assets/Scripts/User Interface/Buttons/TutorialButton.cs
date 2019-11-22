using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    private void OnMouseUp() {
        GameStateManager.sharedInstance.LoadTutorial();
    }
}
