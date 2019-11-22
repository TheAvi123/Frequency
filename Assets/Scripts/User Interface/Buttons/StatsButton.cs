using UnityEngine;

public class StatsButton : MonoBehaviour
{
    //Reference Variables
    [SerializeField] Animator canvasSwitcher = null;

    private void OnMouseUp() {
        canvasSwitcher.SetTrigger("Switch");
    }
}
