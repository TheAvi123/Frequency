using UnityEngine;

public class OverlaySwitcher : MonoBehaviour
{
    //Reference Variables
    UIInputController inputController = null;

    //Configuration Parameters
    [SerializeField] Canvas[] overlays = null;

    //State Variales
    private int currentOverlay = 0;

    //Internal Methods
    private void Awake() {
        FindUIInputController();
        ActivateFirstOverlay();
        SetInputActions();
    }

    private void FindUIInputController() {
        foreach (UIInputController controller in GameObject.FindObjectsOfType<UIInputController>()) {
            if (controller.enabled) {
                inputController = controller;
            }
        }
        if (!inputController) {
            gameObject.SetActive(false);
        }
    }

    private void ActivateFirstOverlay() {
        if (overlays != null) {
            overlays[0].gameObject.SetActive(true);
            for (int i = 1; i < overlays.Length; i++) {
                overlays[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetInputActions() {
        inputController.SetTapAction(DoNothing);
        inputController.SetUpSwipeAction(DoNothing);
        inputController.SetDownSwipeAction(DoNothing);
        inputController.SetLeftSwipeAction(SwitchToNextOverlay);
        inputController.SetRightSwipeAction(SwitchToPrevOverlay);
    }

    private void DoNothing() {
       //Do Nothing
    }

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
