using UnityEngine;
using System.Collections;

public class CoinMagnetModifier : ModifierTemplate
{
    //Reference Variables
    [SerializeField] GameObject coinMagnetPrefab = null;

    //Internal Methods
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            ModifierCollected();
        }
    }

    protected override IEnumerator ModifierEffect() {
        float modifierTimer = 0f;
        GameObject magnet = Instantiate(coinMagnetPrefab).gameObject;
        Transform playerTransform = FindObjectOfType<PlayerWave>().transform;
        while (modifierTimer <= modifierDuration) {
            magnet.transform.position = playerTransform.position;
            modifierTimer += Time.deltaTime;
            yield return null;
        }
        magnet.GetComponent<CoinMagnet>().StopAttracting();
        ExpireModifier();
    }
}
