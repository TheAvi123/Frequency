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
    private float ticker = 0f;
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
        ticker = Random.value;    //Set Ticker to Random Color on Gradient
    }

    private void Update() {
        UpdateTicker();
        UpdateColor();
    }

    private void UpdateTicker() {
        ticker += Time.deltaTime * gradientSpeed;   //Increase Ticker per Frame
        if (ticker > 1) {
            ticker -= 1;    //Reset to 0 to Start Back at Beginning of Gradient
        }
    }

    private void UpdateColor() {
        currentColor = colorGradient.Evaluate(ticker);
        playerTrail.startColor = currentColor;
        playerSprite.color = currentColor;
    }
}
