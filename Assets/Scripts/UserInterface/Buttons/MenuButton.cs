using Systems;

using UnityEngine;

namespace UserInterface.Buttons {
    public class MenuButton : MonoBehaviour
    {
        private void OnMouseUp() {
            GameStateManager.sharedInstance.LoadStartMenu();
        }
    }
}
