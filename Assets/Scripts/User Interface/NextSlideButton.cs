using UnityEngine;

public class NextSlideButton : MonoBehaviour
{
    private void OnMouseDown() {
        InstructionSlideController.sharedInstance.NextSlide();
    }
}
