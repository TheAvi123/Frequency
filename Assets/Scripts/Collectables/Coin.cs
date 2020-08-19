using CameraFeatures;

using Statistics;

using UnityEngine;

using UserInterface;

namespace Collectables {
    public class Coin : MonoBehaviour
    {
        //Configuration Parameters
        [SerializeField] ParticleSystem coinCollectVFX = null;

        //State Variables
        private Transform coinTransform;
        private Vector3 spawnPosition;

        //Internal Methods
        private void OnEnable() {
            GetCoinTransform();
            GetSpawnPosition();
        }

        private void GetCoinTransform() {
            coinTransform = gameObject.transform;
        }

        private void GetSpawnPosition() {
            spawnPosition = coinTransform.localPosition;
        }

        private void OnTriggerEnter2D(Collider2D otherCollider) {
            if (otherCollider.CompareTag("Player")) {
                CoinCollected();
            }
        }

        private void CoinCollected() {
            InfoDisplayer.sharedInstance.DisplayInfo("COIN COLLECTED");
            CoinManager.sharedInstance.CollectCoin();
            SpawnCollectVFX();
            ShakeCamera();
            DisableCoin();
            ReturnToSpawnPosition();
        }

        private void SpawnCollectVFX() {
            GameObject coinVFX = Instantiate(coinCollectVFX, coinTransform.position, coinTransform.rotation).gameObject;
            Destroy(coinVFX, 1f);
        }

        private void ShakeCamera() {
            CameraShaker.sharedInstance.AddCameraShake(0.1f);
        }

        private void ReturnToSpawnPosition() {
            coinTransform.localPosition = spawnPosition;
        }

        //Public Methods
        public void DisableCoin() {
            gameObject.SetActive(false);
        }
    }
}
