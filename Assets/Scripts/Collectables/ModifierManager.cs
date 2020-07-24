using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModifierManager : MonoBehaviour
{
    //Reference Variables
    public static ModifierManager sharedInstance;

    //Display Variables
    private TextMeshProUGUI modifierDisplay = null;
    private Image modifierCooldownBar = null;

    //State Variales
    private Modifier activeModifier = null;

    //Internal Methods
    private void Awake() {
        SetSharedInstance();
        FindModifierDisplay();
    }

    private void SetSharedInstance() {
        sharedInstance = this;
    }

    private void FindModifierDisplay() {
        GameObject gameOverlay = GameObject.Find("GameOverlay");
        modifierDisplay = gameOverlay.transform.Find("ModifierDisplay").GetComponent<TextMeshProUGUI>();
        modifierCooldownBar = modifierDisplay.transform.Find("ModifierCooldown").GetComponent<Image>();
    }

    private void Start() {
        InitializeDisplay();
    }

    private void InitializeDisplay() {
        modifierDisplay.gameObject.SetActive(false);
        modifierCooldownBar.fillAmount = 1;
    }

    //Helper Methods
    private IEnumerator UpdateModifierDisplay(float duration) {
        float timer = 0f;
        modifierDisplay.gameObject.SetActive(true);
        while (timer < duration) {
            modifierCooldownBar.fillAmount = 1 - (timer / duration);
            timer += Time.deltaTime / Time.timeScale;
            yield return null;
        }
        modifierDisplay.gameObject.SetActive(false);
    }

    //Public Methods
    public Modifier GetActiveModifier() {
        return activeModifier;
    }

    public void SetActiveModifier(Modifier modifier, float duration) {
        activeModifier = modifier;
        if (activeModifier) {
            StartCoroutine(UpdateModifierDisplay(duration));
        }
    }

    public void EndModifierEffects() {
        if (activeModifier != null) {
            activeModifier.EndModifierEffects();
        }
        modifierDisplay.gameObject.SetActive(false);
    }
}
