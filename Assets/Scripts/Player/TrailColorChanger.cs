using UnityEngine;

public class TrailColorChanger : MonoBehaviour
{
    ///Reference Variables
    private TrailRenderer playerTrail = null;
    private SpriteRenderer playerSprite = null;
 
    //Serialized Parameters
    [Header("Gradient Parameters")]
    [SerializeField] Gradient colorGradient = null;
    [SerializeField] float gradientSpeed = 0.025f;

    //State Variables
    private float gradientTicker = 0f;
    private Color currentColor;

    private void Awake() {
        FindPlayerSprite();
        FindPlayerTrail();
    }

    private void FindPlayerSprite() {
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        if (!playerSprite) {
            Debug.LogError("No Sprite Renderer Found on The Player");
            enabled = false;        //Disable This Component
        }
    }

    private void FindPlayerTrail() {
        playerTrail = GetComponent<TrailRenderer>();
        if (!playerTrail) {
            Debug.LogError("No Trail Renderer Found On The Player");
            enabled = false;        //Disable This Component
        }
    }

    private void Start() {
        SetInitialColor();
        GradientCheck();
    }

    private void SetInitialColor() {
        GradientMesh background = FindObjectOfType<GradientMesh>();
        if (background) {   //Sync Color Gradient And Progress With Background
            colorGradient = background.GetColorGradient();
            gradientSpeed = background.GetGradientSpeed();
            gradientTicker = background.GetTickerValue() - 0.25f;
        } else {
            gradientTicker = Random.value;
        }
    }

    private void GradientCheck() {
        if (colorGradient == null) {
            Debug.LogWarning("No Color Gradient Set for Player Trail");
            enabled = false;        //Disable This Component
        }
    }

    private void Update() {
        UpdateTicker();
        UpdateColor();
    }

    private void UpdateTicker() {
        gradientTicker += Time.deltaTime * gradientSpeed;   //Increase Ticker per Frame
        if (gradientTicker > 1) {
            gradientTicker -= 1;    //Reset to 0 to Start Back at Beginning of Gradient
        }
    }

    private void UpdateColor() {
        currentColor = colorGradient.Evaluate(gradientTicker);
        playerTrail.startColor = currentColor;
        playerSprite.color = currentColor;
    }

    //Public Methods
    public Color GetTrailColor() {
        return currentColor;
    }
}
