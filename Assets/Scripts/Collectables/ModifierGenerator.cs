using UnityEngine;

namespace Collectables {
    public class ModifierGenerator : MonoBehaviour
    {
        public static ModifierGenerator sharedInstance;
        
        //Reference Variables
        private Transform generatorTransform;

        //Configuration Parameters
        [SerializeField] Modifier[] modifierArray = null;
        [SerializeField] float modifierSpawnChance = 0.5f;
        [SerializeField] float modifierSpawnDelay = 10f;

        //State Variables
        private float spawnTimer = 0f;

        //Internal Methods
        private void Awake() {
            SetSharedInstance();
            CheckModifierArray();
            GetGeneratorTransform();
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
        
        private void GetGeneratorTransform() {
            generatorTransform = gameObject.transform;
        }

        private void SetSpawnDelay() {
            spawnTimer = modifierSpawnDelay;
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
            modifier.transform.SetParent(generatorTransform);
        }

        private Vector3 CalculateSpawnPosition(Transform obstacle, Vector3 spawn) {
            Vector3 spawnPosition = obstacle.position;
            spawnPosition += spawn * Camera.main.aspect * 16 / 9;
            return spawnPosition;
        }

        //Public Methods
        public void SetAvailableSpawn(Transform obstacle, Vector3 spawn) {
            if (ModifierManager.sharedInstance.GetActiveModifier() == null && spawnTimer <= 0f) {
                float roll = Random.value;
                if (roll < modifierSpawnChance) {
                    SpawnRandomModifier(CalculateSpawnPosition(obstacle, spawn));
                }
            }
        }
    }
}
