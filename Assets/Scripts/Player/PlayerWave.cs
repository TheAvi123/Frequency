using System.Collections;
using UnityEngine;

public class PlayerWave : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] float verticalSpeed = 10f;
    [SerializeField] [Range(-1, 1)] int frequencyDirection;
    [SerializeField] [Range(0, 10f)] float frequencyMultiplier = 4f;
    [SerializeField] [Range(0, 10f)] float amplitudeMultiplier = 7.5f;

    [Header("Set Variables")]
    //Saved For Reset Purpose
    private float setVerticalSpeed;
    private float setFrequencyMultiplier;
    private float setAmplitudeMultiplier;

    [Header("Position")]
    private float xPosition, yPosition;
    private float initialOffsetX;
    public float initialOffsetY;            //Made Public to Allow Access from Camera

    [Header("Angle and Direction")]
    private float currentAngle = 0;
    private const float resetAngle = 2 * Mathf.PI;

    [Header("Dash Ability")]
    [SerializeField] float dashDuration = 0.25f;
    [SerializeField] float dashSpeedMultiplier = 4f;
    private Coroutine dashCoroutine = null;

    [Header("Delay Ability")]
    [SerializeField] int delayWavelengthDuration = 1;
    [SerializeField] float delayVerticalSpeed = 0f;
    private Coroutine delayCoroutine = null;
    private bool delayInProgress = false;
    private float delayAngle;

    //Regular Movement Methods
    private void Start() {
        SetupInitialOffsets();
        SaveSetVariables();
        SetRandomFrequencyDirection();
    }

    private void SetupInitialOffsets() {
        Vector2 offsetVector = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.3f, 0));    //Percentage Coordinates
        initialOffsetX = offsetVector.x;
        initialOffsetY = offsetVector.y;
        transform.position = new Vector2(initialOffsetX, 0);
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

        if (delayInProgress) {         //For Exiting Delay Ability - See Delay Coroutine
            delayAngle += Time.deltaTime * frequencyMultiplier;
            if (delayAngle >= resetAngle * delayWavelengthDuration) {
                delayInProgress = false;
            }
        }
    }

    private void SetPosition() {
        xPosition = Mathf.Sin(currentAngle) * amplitudeMultiplier + initialOffsetX;
        yPosition = transform.position.y + verticalSpeed * Time.deltaTime;
        transform.position = new Vector2(xPosition, yPosition);
    }

    //Input Dependent Methods
    public void Flip() {
        StopAllCoroutines();                            //Cancel Dash and Delay Abilities
        ResetWaveParameters();                          //Reset Parameters to Presets
        frequencyDirection = -frequencyDirection;       //Flip Frequency Direction
    }

    public void Dash() {
        if (delayCoroutine != null) {           //Delay Active
            StopCoroutine(delayCoroutine);      //Stop Delay Coroutine
            ResetWaveParameters();              //Reset Parameters to Presets
            delayCoroutine = null;
        }
        dashCoroutine = StartCoroutine(DashCoroutine());
    }

    public void Delay() {
        delayCoroutine = StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DashCoroutine() {
        frequencyMultiplier = 0;
        verticalSpeed *= dashSpeedMultiplier;               //Multiply Vertical Speed by Multiplier
        yield return new WaitForSeconds(dashDuration);
        verticalSpeed = setVerticalSpeed;                   //Reset Vertical Speed to Set Multiplier
        frequencyMultiplier = setFrequencyMultiplier;       //Reset Frequency to Set Frequency
        dashCoroutine = null;
    }

    private IEnumerator DelayCoroutine() {
        delayAngle = 0;                                         //Mark Initial Angle for Duration Check
        delayInProgress = true;
        verticalSpeed = delayVerticalSpeed;                     //Set Vertical Speed to 0 = Pause Movement
        yield return new WaitWhile(() => delayInProgress);      //Boolean Checked in UpdateAngle Method
        verticalSpeed = setVerticalSpeed;                       //Reset Vertical Speed to Set Vertical Speed
        delayCoroutine = null;
    }

    private void ResetWaveParameters() {
        verticalSpeed = setVerticalSpeed;
        frequencyMultiplier = setFrequencyMultiplier;
        amplitudeMultiplier = setAmplitudeMultiplier;
    }
}
