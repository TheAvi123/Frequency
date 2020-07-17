using UnityEngine;
using System.Collections;

public class FreezeScoreModifier : ModifierTemplate
{
    //Internal Methods
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            ModifierCollected();
        }
    }

    protected override IEnumerator ModifierEffect() {
        float modifierTimer = 0f;
        while (modifierTimer <= modifierDuration) {
            ScoreManager.sharedInstance.SetFreezeScore(true);
            modifierTimer += Time.deltaTime;
            yield return null;
        }
        ScoreManager.sharedInstance.SetFreezeScore(false);
        ExpireModifier();
    }
}
