using UnityEngine;

public class ShopButton : MonoBehaviour
{
    private void OnMouseDown() {
        GameStateManager.sharedInstance.OpenShop();
    }
}
