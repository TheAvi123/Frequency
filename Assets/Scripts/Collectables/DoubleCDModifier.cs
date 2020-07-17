using UnityEngine;
using System.Collections;

public class DoubleCDModifier : ModifierTemplate
{
    //Internal Methods
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            ModifierCollected();
        }
    }

    protected override IEnumerator ModifierEffect() {
        PlayerAbilityManager player = FindObjectOfType<PlayerAbilityManager>();
        float modifierTimer = 0f;
        while (modifierTimer <= modifierDuration) {
            player.SetDoubleCooldown(true);
            modifierTimer += Time.deltaTime;
            yield return null;
        }
        player.SetDoubleCooldown(false);
        ExpireModifier();
    }
}
