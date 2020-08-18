using Systems;

using UnityEngine;

namespace UserInterface.Buttons {
    public class QuitButton : MonoBehaviour
    {
        private void OnMouseUp() {
            GameStateManager.sharedInstance.QuitGame();
        }
    }
}
