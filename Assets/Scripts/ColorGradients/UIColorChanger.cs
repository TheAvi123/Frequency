using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ColorGradients {
    public class UIColorChanger : MonoBehaviour
    {
        //Reference Variables
        private TextMeshProUGUI text = null;
        private Image image = null;

        //Configuration Parameters
        [SerializeField] Material baseColor = null;

        //Internal Methods
        private void Awake() {
            FindUIComponent();
        }

        private void FindUIComponent() {
            text = GetComponent<TextMeshProUGUI>();
            image = GetComponent<Image>();
        }

        private void Update() {
            UpdateColor();
        }

        private void UpdateColor() {
            if (text) {
                text.color = baseColor.color;
            }
            if (image) {
                image.color = baseColor.color;
            }
        }
    }
}
