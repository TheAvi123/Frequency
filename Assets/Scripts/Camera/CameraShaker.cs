using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker sharedInstance;

    [Header("Shake Parameters")]
    [SerializeField][Range(0, 1)] float traumaLevel = 0f;   //Level Determines Shake Intensity
    [SerializeField] float shakeMultiplier = 15f;           //Multiplier for Trauma Level
    [SerializeField] float roughnessMultiplier = 10f;       //Multiplier for Displacement
    [SerializeField] float rotationalMultiplier = 1f;       //Multiplier for Rotation
    [SerializeField] float shakeDecay = 1f;                 //How Quickly the Shake Fades Out

    //State Variables
    private bool shakeActive = true;    //Boolean for Shake
    private float ticker;               //Time Counter for Decay/Additive Purposes

    private void Awake() {
        SetSharedInstance();
    }

    private void SetSharedInstance() {
        sharedInstance = this;
    }

    private void Update() {
        if (shakeActive) {
            Shake();
        }
    }

    private void Shake() {
        UpdateTimer();
        MoveAndRotate();
        DecayTrauma();
    }

    private void UpdateTimer() {
        ticker += Time.deltaTime * Mathf.Pow(traumaLevel, 0.3f) * shakeMultiplier;
    }

    private void MoveAndRotate() {
        Vector3 newPos = GeneratePerlinVector() * roughnessMultiplier * traumaLevel;
        transform.localPosition = newPos;
        transform.localRotation = Quaternion.Euler(newPos * rotationalMultiplier);
    }

    private void DecayTrauma() {
        traumaLevel -= Mathf.Clamp01(Time.deltaTime * shakeDecay * traumaLevel);
    }

    //Helper Methods
    private float GeneratePerlinFloat(float seed) {
        return ((Mathf.PerlinNoise(seed, ticker) - 0.5f) * 2f);
    }

    private Vector3 GeneratePerlinVector() {
        return new Vector3(GeneratePerlinFloat(0), GeneratePerlinFloat(50), 0);
    }

    private void ResetPositionRotation() {
        print("reset");
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    //Public Methods
    public void ShakeCamera() {
        StartCoroutine(ShakeCameraCoroutine());
    }

    IEnumerator ShakeCameraCoroutine() {
        shakeActive = true;
        traumaLevel = 1;
        yield return new WaitForSecondsRealtime(2f);
        shakeActive = false;
        ResetPositionRotation();
    }
}