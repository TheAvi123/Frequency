using TMPro;

using UnityEngine;

namespace ColorGradients {
    public class BaseColorChanger : MonoBehaviour
    {
        //Reference Variables
        private GradientMesh gradientMesh;

        //Configuration Parameters
        [Header("Gradient Parameters")]
        [SerializeField] float gradientSpeed = 0;
        [SerializeField] Gradient colorGradient = null;

        //Modified Variables
        [Header("Materials")]
        [SerializeField] Material baseMaterial = null;
        [SerializeField] TMP_FontAsset textAsset = null;
        [SerializeField] TMP_FontAsset numberAsset = null;

        //State Variables
        private float gradientTicker = 0f;
        private Color currentColor;

        private void Awake() {
            FindGradientMesh();
            SyncWithGradientMesh();
        }

        private void FindGradientMesh() {
            gradientMesh = GetComponent<GradientMesh>();
            if (!gradientMesh) {
                Debug.LogError("No Gradient Mesh Component Found");
                enabled = false;
            }
        }

        private void SyncWithGradientMesh() {
            colorGradient = gradientMesh.GetColorGradient();
            gradientSpeed = gradientMesh.GetGradientSpeed();
            gradientTicker = gradientMesh.GetTickerValue() - 0.40f;
            if (gradientTicker < 0) {
                gradientTicker += 1;
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void OnSceneChange() {
            //Do Nothing... Function Required For Singleton
        }

        private void Update() {
            UpdateTicker();
            UpdateMaterials();
        }

        private void UpdateTicker() {
            gradientTicker += Time.deltaTime * gradientSpeed;   //Increase Ticker per Frame
            if (gradientTicker > 1) {
                gradientTicker -= 1;    //Reset to 0 to Start Back at Beginning of Gradient
            }
        }

        private void UpdateMaterials() {
            currentColor = colorGradient.Evaluate(gradientTicker);
            baseMaterial.color = currentColor;
            textAsset.material.SetColor(ShaderUtilities.ID_FaceColor, currentColor);
            numberAsset.material.SetColor(ShaderUtilities.ID_FaceColor, currentColor);
        }

        //Public Methods
        public Color GetCurrentColor() {
            return currentColor;
        }
    }
}
