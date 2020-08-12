using Systems;

using UnityEngine;

namespace UserInterface.Buttons {
    public class ShopButton : MonoBehaviour
    {
        private void OnMouseUp() {
            GameStateManager.sharedInstance.OpenShop();
        }
    }
}
