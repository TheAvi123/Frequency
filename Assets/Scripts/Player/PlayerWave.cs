using System.Collections;
using UnityEngine;

public class PlayerWave : MonoBehaviour
{
    //Reference Variables
    private PlayerAbilityManager abilityManager;

    //Movement Configuration Parameters
    [Header("Movement Parameters")]
    [SerializeField] float verticalSpeed = 10f;
    [SerializeField] [Range(-1, 1)] int frequencyDirection;
    [SerializeField] [Range(0, 10f)] float frequencyMultiplier = 4f;
    [SerializeField] [Range(0, 15f)] float amplitudeMultiplier = 7.5f;

    //Movement State Variables
    [Header("Set Variables")]
    private float setVerticalSpeed;
    private float setFrequencyMultiplier;
    private float setAmplitudeMultiplier;

    [Header("Position")]
    private float xPosition, yPosition;
    private float initialOffsetX;
    [HideInInspector] public float initialOffsetY;      //Made Public to Allow Access from Camera

    [Header("Angle and Direction")]
    private float currentAngle = 0;
    private const float resetAngle = 2 * Mathf.PI;

    //Ability Configuration Parameters
    [Header("Dash Ability")]
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashSpeedMultiplier = 4f;

    [Header("Delay Ability")]
    [SerializeField] int delayWavelengths = 1;
    [SerializeField] float delayVerticalSpeed = 0f;

    //Ability State Variables
    private Coroutine dashCoroutine = null;
    private Coroutine delayCoroutine = null;

    //Internal Movement Methods
    private void Awake() {
        FindAbilityManager();
    }

    private void FindAbilityManager() {
        abilityManager = GetComponent<PlayerAbilityManager>();
        if (!abilityManager) {
            Debug.LogError("No Player Ability Manager Found!");
            enabled = false;
        }
    }

    private void Start() {
        AspectRatioReconfigurations();
        SetupInitialOffsets();
        CalculateAmplitudeMultiplier();
        SaveSetVariables();
        SetRandomFrequencyDirection();
    }

    private void AspectRatioReconfigurations() {
        float aspectMultiplier = Camera.main.aspect * 16 / 9;
        transform.localScale *= aspectMultiplier;
        verticalSpeed *= aspectMultiplier;
    }

    private void SetupInitialOffsets() {
        Vector2 offsetVector = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.4f));    //Percentage Coordinates
        initialOffsetX = offsetVector.x;
        initialOffsetY = offsetVector.y;
        transform.position = new Vector2(initialOffsetX, -16.5f);
    }

    private void CalculateAmplitudeMultiplier() {
        amplitudeMultiplier = Camera.main.ViewportToWorldPoint(new Vector3(0.92f, 0)).x;
    }

    private void SaveSetVariables() {
        //Saving These Variables For Reset Purpose
        setVerticalSpeed = verticalSpeed;
        setFrequencyMultiplier = frequencyMultiplier;
        setAmplitudeMultiplier = amplitudeMultiplier;
    }

    private void SetRandomFrequencyDirection() {
        frequencyDirection = Random.Range(0, 2) * 2 - 1;    //Randomly Returns Either +1 or -1
    }

    private void Update() {
        UpdateAngle();
        SetPosition();
    }

    private void UpdateAngle() {
        currentAngle += Time.deltaTime * frequencyMultiplier * frequencyDirection;
        if (currentAngle >= resetAngle) {
            currentAngle -= resetAngle;
        }
    }

    private void SetPosition() {
        xPosition = Mathf.Sin(currentAngle) * amplitudeMultiplier + initialOffsetX;
        yPosition = transform.position.y + verticalSpeed * Time.deltaTime;
        transform.position = new Vector3(xPosition, yPosition);
    }

    //Ability Methods
    public void Flip() {
        StopAllCoroutines();                            //Cancel Dash and Delay Abilities
        ResetWaveParameters();                          //Reset Parameters to Presets
        frequencyDirection = -frequencyDirection;       //Flip Frequency Direction

        abilityManager.FlipUsed();

        if (dashCoroutine != null) {
            frequencyDirection = -frequencyDirection;       //Undo Flip to Only Cancel Dash
            dashCoroutine = null;
        } else if (delayCoroutine != null) {
            frequencyDirection = -frequencyDirection;       //Undo Flip to Only Cancel Delay
            delayCoroutine = null;
        }
    }

    public void Dash() {
        if (abilityManager.GetDashStatus()) {
            if (delayCoroutine != null) {           //Delay Active
                StopCoroutine(delayCoroutine);      //Stop Delay Coroutine
                ResetWaveParameters();              //Reset Parameters to Presets
                delayCoroutine = null;
            }
            dashCoroutine = StartCoroutine(DashCoroutine());
        }
    }

    public void Delay() {
        if (abilityManager.GetDelayStatus()) {
            delayCoroutine = StartCoroutine(DelayCoroutine());
        }
    }

    private IEnumerator DashCoroutine() {
        abilityManager.DashUsed(dashDuration);
        frequencyMultiplier = 0;
        verticalSpeed *= dashSpeedMultiplier;               //Multiply Vertical Speed by Multiplier
        yield return new WaitForSeconds(dashDuration);
        verticalSpeed = setVerticalSpeed;                   //Reset Vertical Speed to Set Multiplier
        frequencyMultiplier = setFrequencyMultiplier;       //Reset Frequency to Set Frequency
        dashCoroutine = null;
    }

    private IEnumerator DelayCoroutine() {
        abilityManager.DelayUsed(frequencyMultiplier, delayWavelengths);
        verticalSpeed = delayVerticalSpeed;                     //Set Vertical Speed to 0 = Pause Movement
        yield return new WaitWhile(() => abilityManager.GetDelayProgress());
        verticalSpeed = setVerticalSpeed;                       //Reset Vertical Speed to Set Vertical Speed
        delayCoroutine = null;
    }

    private void ResetWaveParameters() {
        verticalSpeed = setVerticalSpeed;
        frequencyMultiplier = setFrequencyMultiplier;
        amplitudeMultiplier = setAmplitudeMultiplier;
    }
}