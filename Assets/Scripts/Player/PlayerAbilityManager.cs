using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerAbilityManager : MonoBehaviour
{
    //Configuration Parameters
    [Header("Tutorial Override")]
    public bool tutorialActive = false;

    [Header("Ability Cooldowns")]
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] float delayCooldown = 3f;

    [Header("Ability Display Colors")]
    [SerializeField] float readyAlpha = 1f;
    [SerializeField] float cooldownAlpha = 0.25f;

    //Display State Variables
    private Color displayColor;

    //Dash State Variables
    private TextMeshProUGUI dashDisplay = null;
    private bool dashInProgress = false;
    private bool dashReady = false;
    private float dashTicker = 0f;

    //Delay State Variables
    private TextMeshProUGUI delayDisplay = null;
    private bool delayInProgress = false;
    private bool delayReady = false;
    private float delayTicker = 0f;
    private float delayAngle;

    //Internal Methods
    private void Start() {
        FindCooldownDisplays();
        InitializeCooldowns();
    }

    private void FindCooldownDisplays() {
        if (tutorialActive) { return; }
        GameObject gameOverlay = GameObject.Find("GameOverlay");
        if (!gameOverlay) {
            gameOverlay = GameObject.Find("TutorialOverlay");
        }
        dashDisplay = gameOverlay.transform.Find("DashDisplay").GetComponent<TextMeshProUGUI>();
        delayDisplay = gameOverlay.transform.Find("DelayDisplay").GetComponent<TextMeshProUGUI>();
    }

    private void InitializeCooldowns() {
        dashTicker = dashCooldown;
        delayTicker = delayCooldown;
    }

    private void Update() {
        ManageCooldowns();
    }

    private void ManageCooldowns() {
        if (!dashReady) {
            if (dashTicker > 0) {
                dashTicker -= Time.deltaTime;
            } else {
                dashReady = true;
                displayColor = dashDisplay.color;
                displayColor.a = readyAlpha;
                dashDisplay.color = displayColor;
            }
        }
        if (!delayReady) {
            if (delayTicker > 0) {
                delayTicker -= Time.deltaTime;
            } else {
                delayReady = true;
                displayColor = delayDisplay.color;
                displayColor.a = readyAlpha;
                delayDisplay.color = displayColor;
            }
        }
    }

    //Public Call Methods
    public void FlipUsed() {
        StatsManager.sharedInstance.AddFlip();
    }

    public void DashUsed(float duration) {
        StatsManager.sharedInstance.AddDash();

        dashReady = false;
        dashTicker = dashCooldown;
        dashInProgress = true;

        displayColor = dashDisplay.color;
        displayColor.a = cooldownAlpha;
        dashDisplay.color = displayColor;

        TickDashProgress(duration);
    }

    public IEnumerator TickDashProgress(float duration) {
        yield return new WaitForSeconds(duration);
        dashInProgress = false;
    }

    public void DelayUsed(float frequency, float delayWavelengths) {
        StatsManager.sharedInstance.AddDelay();

        delayReady = false;
        delayTicker = delayCooldown;

        displayColor = delayDisplay.color;
        displayColor.a = cooldownAlpha;
        delayDisplay.color = displayColor;

        delayAngle = 0;
        delayInProgress = true;
        StartCoroutine(TickDelayAngle(frequency, delayWavelengths));
    }

    public IEnumerator TickDelayAngle(float frequency, float delayWavelengths) {
        while (true) {
            delayAngle += Time.deltaTime * frequency;
            if (delayAngle >= (2 * Mathf.PI) * delayWavelengths) {
                delayInProgress = false;
                break;
            }
            yield return null;
        }
    }

    //Public Return Methods
    public bool GetDashStatus() {
        return dashReady;
    }

    public bool GetDelayStatus() {
        return delayReady;
    }

    public bool GetDashProgress() {
        return dashInProgress;
    }

    public bool GetDelayProgress() {
        return delayInProgress;
    }
}
