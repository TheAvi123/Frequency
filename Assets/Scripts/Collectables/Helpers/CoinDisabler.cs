using UnityEngine;

namespace Collectables.Helpers {
    public class CoinDisabler : MonoBehaviour
    {
        //Internal Methods
        private void OnTriggerEnter2D(Collider2D otherCollider) {
            if (otherCollider.CompareTag("Coin")) {
                DisableCoin(otherCollider.gameObject);
            }
        }

        private void DisableCoin(GameObject coinObject) {
            coinObject.GetComponent<Coin>().DisableCoin();
        }
    }
}
