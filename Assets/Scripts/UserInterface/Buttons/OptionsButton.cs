using Systems;

using UnityEngine;

namespace UserInterface.Buttons {
    public class OptionsButton : MonoBehaviour
    {
        private void OnMouseUp() {
            GameStateManager.sharedInstance.OpenOptions();
        }
    }
}
