using UnityEngine;

public class PreviousSlideButton : MonoBehaviour
{
    private void OnMouseDown() {
        InstructionSlideController.sharedInstance.PreviousSlide();
    }
}
