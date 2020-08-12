using UnityEngine;

namespace UserInterface.Buttons {
    public class StatsButton : MonoBehaviour
    {
        //Reference Variables
        private Animator canvasSwitcher = null;

        //Configuration Parameters
        private static readonly int Switch = Animator.StringToHash("Switch");

        private void Awake() {
            FindCanvasSwitcher();
        }

        private void FindCanvasSwitcher() {
            canvasSwitcher = gameObject.GetComponentInParent<Animator>();
        }

        private void OnMouseUp() {
            canvasSwitcher.SetTrigger(Switch);
        }
    }
}