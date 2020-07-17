using UnityEngine;
using System.Collections;

public class GhostModifier : ModifierTemplate
{
    //Internal Methods
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            ModifierCollected();
        }
    }

    protected override IEnumerator ModifierEffect() {
        PlayerInteractions player = FindObjectOfType<PlayerInteractions>();
        float modifierTimer = 0f;
        while (modifierTimer <= modifierDuration) {
            player.SetGhostMode(true);
            modifierTimer += Time.deltaTime;
            yield return null;
        }
        player.SetGhostMode(false);
        ExpireModifier();
    }
}
