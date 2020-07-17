using UnityEngine;

public class CoinDisabler : MonoBehaviour
{
    //Internal Methods
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Coin") {
            DisableCoin(otherCollider.gameObject);
        }
    }

    private void DisableCoin(GameObject coinObject) {
        coinObject.GetComponent<Coin>().DisableCoin();
    }
}
