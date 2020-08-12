using UnityEngine;

namespace UserInterface {
    public class OverlayScaler : MonoBehaviour
    {
        //Reference Variables
        RectTransform overlay;

        private void Awake() {
            FindOverlayTransform();
            ScaleOverlayToAspectRatio();
        }
    
        private void FindOverlayTransform() {
            overlay = GetComponent<RectTransform>();
            if (!overlay) {
                gameObject.SetActive(false);
            }
        }

        private void ScaleOverlayToAspectRatio() {
            if (gameObject.activeInHierarchy) {
                float aspectMultiplier = Camera.main.aspect * 16 / 9;
                overlay.sizeDelta = new Vector2(overlay.rect.width * aspectMultiplier, overlay.rect.height);
            }
        }
    }
}
