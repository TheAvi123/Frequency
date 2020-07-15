using UnityEngine;

public class StatsButton : MonoBehaviour
{
    //Reference Variables
    private Animator canvasSwitcher = null;

    private void Awake() {
        FindCanvasSwitcher();
    }

    private void FindCanvasSwitcher() {
        canvasSwitcher = gameObject.GetComponentInParent<Animator>();
    }

    private void OnMouseUp() {
        canvasSwitcher.SetTrigger("Switch");
    }
}