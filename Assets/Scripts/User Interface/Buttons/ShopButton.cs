using UnityEngine;

public class ShopButton : MonoBehaviour
{
    private void OnMouseUp() {
        GameStateManager.sharedInstance.OpenShop();
    }
}
