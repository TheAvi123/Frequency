using Systems;

using UnityEngine;

namespace UserInterface.Buttons {
    public class PlayButton : MonoBehaviour
    {
        private void OnMouseUp() {
            GameStateManager.sharedInstance.PlayGame();
        }
    }
}
