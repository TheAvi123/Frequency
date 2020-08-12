using Systems;

using UnityEngine;

namespace UserInterface.Buttons {
    public class TutorialButton : MonoBehaviour
    {
        private void OnMouseUp() {
            GameStateManager.sharedInstance.LoadTutorial();
        }
    }
}
