using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerAbilityManager : MonoBehaviour
{
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

    //Modifier Varibles
    private bool doubleCooldowns = false;
    private bool removedCooldowns = false;

    //Internal Methods
    private void Start() {
        FindCooldownDisplays();
        InitializeCooldowns();
    }

    private void FindCooldownDisplays() {
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

        SetDashTicker();
        dashReady = false;
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

        SetDelayTicker();
        delayReady = false;

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

    private void SetDashTicker() {
        if (doubleCooldowns) {
            dashTicker = dashCooldown * 3;
        } else if (removedCooldowns) {
            dashTicker = 0;
        } else {
            dashTicker = dashCooldown;
        }
    }

    private void SetDelayTicker() {
        if (doubleCooldowns) {
            delayTicker = delayCooldown * 2.5f;
        } else if (removedCooldowns) {
            delayTicker = 0;
        } else {
            delayTicker = delayCooldown;
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

    public void SetDoubleCooldown(bool status) {
        doubleCooldowns = status;
    }

    public void SetRemovedCooldowns(bool status) {
        removedCooldowns = status;
    }
}
