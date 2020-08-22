﻿using System.Collections;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Player {
    public class PlayerWave : MonoBehaviour
    {
        //Reference Variables
        private PlayerAbilityManager abilityManager;
        private Transform playerTransform;

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

        [Header("Angle and Direction")] 
        private bool trueAngle = true;
        private float idealAngle = 0;
        private float currentAngle = 0;
        private const float ResetAngle = 2 * Mathf.PI;

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
            GetPlayerTransform();
        }

        private void FindAbilityManager() {
            abilityManager = GetComponent<PlayerAbilityManager>();
            if (!abilityManager) {
                Debug.LogError("No Player Ability Manager Found!");
                enabled = false;
            }
        }

        private void GetPlayerTransform() {
            playerTransform = gameObject.transform;
        }


        private void Start() {
            AspectRatioReconfigurations();
            CalculateAmplitudeMultiplier();
            SaveCalculatedMovementVariables();
            SetRandomFrequencyDirection();
            MoveToStartPosition();
        }

        private void AspectRatioReconfigurations() {
            float aspectMultiplier = Camera.main.aspect * 16 / 9;
            playerTransform.localScale *= aspectMultiplier;
            verticalSpeed *= aspectMultiplier;
        }

        private void CalculateAmplitudeMultiplier() {
            amplitudeMultiplier = Camera.main.ViewportToWorldPoint(new Vector3(0.92f, 0)).x;
        }

        private void SaveCalculatedMovementVariables() {
            //Saving These Variables For Reset Purpose
            setVerticalSpeed = verticalSpeed;
            setFrequencyMultiplier = frequencyMultiplier;
            setAmplitudeMultiplier = amplitudeMultiplier;
        }

        private void SetRandomFrequencyDirection() {
            frequencyDirection = Random.Range(0, 2) * 2 - 1;    //Randomly Returns Either +1 or -1
        }
        
        private void MoveToStartPosition() {
            playerTransform.position = new Vector2(0f, -16.5f);
        }

        private void Update() {    
            UpdateAngle();
            SetPosition();
        }

        private void UpdateAngle() {
            float tickAmount = Time.deltaTime * frequencyMultiplier * frequencyDirection;
            currentAngle += tickAmount;
            if (!trueAngle) {
                idealAngle += tickAmount;
                if (Mathf.Abs(currentAngle - ResetAngle) < 0.5f && idealAngle < 0.5f) {
                    currentAngle -= ResetAngle;
                }
                currentAngle = Mathf.Lerp(currentAngle, idealAngle, 2.5f * Time.deltaTime);
                if (Mathf.Abs(currentAngle - idealAngle) <= 0.01f) {
                    trueAngle = true;
                }
            }
            if (frequencyDirection == 1 && currentAngle >= ResetAngle) {
                currentAngle -= ResetAngle;
                if (!trueAngle) {
                    idealAngle -= ResetAngle;
                }
            } else if (frequencyDirection == -1 && currentAngle <= 0) {
                currentAngle += ResetAngle;
                if (!trueAngle) {
                    idealAngle += ResetAngle;
                }
            }
        }

        private void SetPosition() {
            xPosition = Mathf.Sin(currentAngle) * amplitudeMultiplier;
            yPosition = playerTransform.position.y + verticalSpeed * Time.deltaTime;
            playerTransform.position = new Vector3(xPosition, yPosition);
        }

        //Ability Methods
        public void Flip() {
            StopAllCoroutines();                            //Cancel Dash and Delay Abilities
            ResetWaveParameters();                          //Reset Parameters to Presets
            frequencyDirection = -frequencyDirection;       //Flip Frequency Direction

            abilityManager.FlipUsed();

            if (abilityManager.GetDashProgress() && dashCoroutine != null) {
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

        //Public Methods
        public void SetFrequencyDirection(bool positive) {
            if (positive) {
                frequencyDirection = 1;
            } else {
                frequencyDirection = -1;
            }
        }

        public void SetIdealAngle(float radianAngle) {
            trueAngle = false;
            idealAngle = radianAngle * Mathf.PI;
        }

        public float GetVerticalSpeed() {
            return verticalSpeed;
        }
    }
}