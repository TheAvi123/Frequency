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
        GradientCheck();
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

    private void GradientCheck() {
        if (colorGradient == null) {
            Debug.LogWarning("No Color Gradient Set for Player Trail");
            enabled = false;        //Disable This Component
        }
    }

    private void Start() {
        SetRandomColor();
    }

    private void SetRandomColor() {
        gradientTicker = Random.value;    //Set Ticker to Random Color on Gradient
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

    //Public Getter Methods
    public Gradient GetColorGradient() {
        return colorGradient;
    }

    public float GetGradientSpeed() {
        return gradientSpeed;
    }

    public float GetTickerValue() {
        return gradientTicker;
    }
}
