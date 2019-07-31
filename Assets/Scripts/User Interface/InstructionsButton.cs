using UnityEngine;

public class InstructionsButton : MonoBehaviour
{
    private void OnMouseDown() {
        GameStateManager.sharedInstance.ShowInstructions();
    }
}
