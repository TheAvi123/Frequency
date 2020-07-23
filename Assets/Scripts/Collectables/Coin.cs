using UnityEngine;

public class Coin : MonoBehaviour
{
    //Configuration Parameters
    [SerializeField] ParticleSystem coinCollectVFX = null;

    //State Variables
    private Vector3 spawnPosition;

    //Internal Methods
    private void OnEnable() {
        GetSpawnPosition();
    }

    private void GetSpawnPosition() {
        spawnPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
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
        GameObject coinVFX = Instantiate(coinCollectVFX, transform.position, transform.rotation).gameObject;
        Destroy(coinVFX, 1f);
    }

    private void ShakeCamera() {
        CameraShaker.sharedInstance.AddCameraShake(0.1f);
    }

    private void ReturnToSpawnPosition() {
        transform.position = spawnPosition;
    }

    //Public Methods
    public void DisableCoin() {
        gameObject.SetActive(false);
    }
}
