using Systems;

using UnityEngine;

namespace UserInterface.Buttons {
    public class ScoresButton : MonoBehaviour
    {
        private void OnMouseUp() {
            GameStateManager.sharedInstance.LoadStats();
        }
    }
}
