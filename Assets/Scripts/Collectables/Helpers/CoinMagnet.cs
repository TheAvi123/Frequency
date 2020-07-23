using UnityEngine;
using System.Collections;

public class CoinMagnet : MonoBehaviour
{
    //Configuration Parameters
    [SerializeField] float magnetMultiplier = 5f;

    //State Variables
    private int coinCount = 0;
    private bool keepAttracting = true;

    //Internal Methods
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Coin") {
            if (keepAttracting) {
                StartCoroutine(AttractCoin(otherCollider.gameObject));
            }
        }
    }

    private IEnumerator AttractCoin(GameObject coinObject) {
        coinCount++;
        coinObject.GetComponent<Animator>().enabled = false;
        Transform coin = coinObject.gameObject.transform;
        while (coinObject.activeInHierarchy) {
            coin.position = Vector3.Lerp(coin.position, transform.position, magnetMultiplier * Time.deltaTime);
            yield return null;
        }
        coinCount--;
    }

    private IEnumerator DestroyMagnet() {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        while (coinCount > 0) {
            yield return null;
        }
        Destroy(gameObject);
    }

    //Public Methods
    public void StopAttracting() {
        keepAttracting = false;
        StartCoroutine(DestroyMagnet());
    }
}
