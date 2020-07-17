using UnityEngine;
using System.Collections;

public class NoCoinsModifier : ModifierTemplate
{
    //Reference Variables
    [SerializeField] GameObject coinDisablerPrefab = null;

    //Internal Methods
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            ModifierCollected();
        }
    }

    protected override IEnumerator ModifierEffect() {
        float modifierTimer = 0f;
        GameObject disabler = Instantiate(coinDisablerPrefab).gameObject;
        Transform playerTransform = FindObjectOfType<PlayerWave>().transform;
        DisableCoinSprite();
        while (modifierTimer <= modifierDuration) {
            disabler.transform.position = new Vector2(0, playerTransform.position.y);
            modifierTimer += Time.deltaTime;
            yield return null;
        }
        Destroy(disabler);
        ExpireModifier();
    }

    private void DisableCoinSprite() {
        gameObject.GetComponentInChildren<Coin>().gameObject.SetActive(false);
    }
}
