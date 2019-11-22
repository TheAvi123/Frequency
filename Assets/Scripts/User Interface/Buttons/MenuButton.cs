using UnityEngine;

public class MenuButton : MonoBehaviour
{
    private void OnMouseUp() {
        GameStateManager.sharedInstance.LoadMenu();
    }
}
