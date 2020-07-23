using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModifierManager : MonoBehaviour
{
    //Reference Variables
    public static ModifierManager sharedInstance;

    //Configuration Parameters
    [SerializeField] Modifier[] modifierArray = null;
    [SerializeField] float modifierSpawnChance = 0.03f;
    [SerializeField] float modifierSpawnDelay = 10f;

    //Display Variables
    private TextMeshProUGUI modifierDisplay = null;
    private Image modifierCooldownBar = null;

    //State Variales
    private Modifier activeModifier = null;
    private float spawnTimer = 0f;

    //Internal Methods
    private void Awake() {
        SetSharedInstance();
        CheckModifierArray();
        FindModifierDisplay();
        SetSpawnDelay();
    }

    private void SetSharedInstance() {
        sharedInstance = this;
    }

    private void CheckModifierArray() {
        if (modifierArray == null || modifierArray.Length == 0) {
            enabled = false;
            Debug.LogError("No Modifiers Specified in Manager Array");
        }
    }

    private void FindModifierDisplay() {
        GameObject gameOverlay = GameObject.Find("GameOverlay");
        modifierDisplay = gameOverlay.transform.Find("ModifierDisplay").GetComponent<TextMeshProUGUI>();
        modifierCooldownBar = modifierDisplay.transform.Find("ModifierCooldown").GetComponent<Image>();
    }

    private void SetSpawnDelay() {
        spawnTimer = modifierSpawnDelay;
    }

    private void Start() {
        InitializeDisplay();
    }

    private void InitializeDisplay() {
        modifierDisplay.gameObject.SetActive(false);
        modifierCooldownBar.fillAmount = 1;
    }

    private void Update() {
        TickSpawnCooldown();
    }

    private void TickSpawnCooldown() {
        if (spawnTimer > 0f) {
            spawnTimer -= Time.deltaTime;
        }
    }

    //Helper Methods
    private void SpawnRandomModifier(Vector3 spawnPosition) {
        spawnTimer = modifierSpawnDelay;
        Modifier modifierToSpawn = modifierArray[Random.Range(0, modifierArray.Length)];
        Modifier modifier = Instantiate(modifierToSpawn, spawnPosition, Quaternion.identity);
        modifier.gameObject.transform.SetParent(gameObject.transform);
    }

    private Vector3 CalculateSpawnPosition(Transform obstacle, Vector3 spawn) {
        Vector3 spawnPosition = obstacle.position;
        spawnPosition += spawn * Camera.main.aspect * 16 / 9;
        return spawnPosition;
    }

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
    public void SetAvailableSpawn(Transform obstacle, Vector3 spawn) {
        if (activeModifier == null && spawnTimer <= 0f) {
            float roll = Random.value;
            if (roll < modifierSpawnChance) {
                SpawnRandomModifier(CalculateSpawnPosition(obstacle, spawn));
            }
        }
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
