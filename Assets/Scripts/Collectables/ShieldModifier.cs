using UnityEngine;
using System.Collections;

public class ShieldModifier : ModifierTemplate
{
    //Reference Variables
    [SerializeField] GameObject shieldPrefab = null;

    //Internal Methods
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            ModifierCollected();
        }
    }

    protected override IEnumerator ModifierEffect() {
        float modifierTimer = 0f;
        Transform player = FindObjectOfType<PlayerWave>().transform;
        GameObject shield = Instantiate(shieldPrefab, player.position, player.rotation).gameObject;
        shield.transform.position = player.position;
        shield.transform.SetParent(player);
        while (modifierTimer <= modifierDuration) {
            modifierTimer += Time.deltaTime;
            yield return null;
        }
        if (shield) {
            shield.GetComponent<Shield>().DestroyShield();
        }
        ExpireModifier();
    }
}
