using UnityEngine;

public class Coin : MonoBehaviour
{
    //Configuration Parameters
    [SerializeField] ParticleSystem coinCollectVFX = null;

    //Internal Methods
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            CoinCollected();
        }
    }

    private void CoinCollected() {
        CoinManager.sharedInstance.CollectCoin();
        SpawnCollectVFX();
        ShakeCamera();
        DisableCoin();
    }

    private void SpawnCollectVFX() {
        GameObject coinVFX = Instantiate(coinCollectVFX, transform.position, transform.rotation).gameObject;
        Destroy(coinVFX, 1f);
    }

    private void ShakeCamera() {
        CameraShaker.sharedInstance.AddCameraShake(0.1f);
    }

    //Public Methods
    public void DisableCoin() {
        gameObject.SetActive(false);
    }
}
