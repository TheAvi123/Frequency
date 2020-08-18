using InputControllers;

using UnityEngine;

namespace UserInterface {
    public class OverlaySwitcher : MonoBehaviour
    {
        //Configuration Parameters
        [SerializeField] Canvas[] overlays = null;

        //State Variables
        private int currentOverlay = 0;

        //Internal Methods
        private void Awake() {
            ActivateFirstOverlay();
        }

        private void ActivateFirstOverlay() {
            if (overlays != null) {
                overlays[0].gameObject.SetActive(true);
                for (int i = 1; i < overlays.Length; i++) {
                    overlays[i].gameObject.SetActive(false);
                }
            }
        }

        private void Start() {
            SetInputControllerActions();
        }

        private void SetInputControllerActions() {
            InputController inputController = FindObjectOfType<InputController>();
            if (!inputController) {
                gameObject.SetActive(false);
            }
            inputController.SetLeftSwipeAction(SwitchToNextOverlay);
            inputController.SetRightSwipeAction(SwitchToPrevOverlay);
        }

        //Input Action Methods
        private void SwitchToPrevOverlay() {
            if (overlays == null || overlays.Length < 2) {
                Debug.LogWarning("No Overlays To Switch Between");
            } else {
                overlays[currentOverlay].gameObject.SetActive(false);
                if (currentOverlay == 0) {
                    currentOverlay = overlays.Length;
                }
                currentOverlay = (--currentOverlay) % overlays.Length;
                overlays[currentOverlay].gameObject.SetActive(true);
            }
        }

        private void SwitchToNextOverlay() {
            if (overlays == null || overlays.Length < 2) {
                Debug.LogWarning("No Overlays To Switch Between");
            } else {
                overlays[currentOverlay].gameObject.SetActive(false);
                currentOverlay = (++currentOverlay) % overlays.Length;
                overlays[currentOverlay].gameObject.SetActive(true);
            }
        }
    }
}
