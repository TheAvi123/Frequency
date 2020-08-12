using System.Collections;

using Statistics;

using UnityEngine;

namespace Collectables.Modifiers {
    public class FreezeScoreModifier : Modifier
    {
        //Internal Methods
        protected override IEnumerator ModifierEffect() {
            ScoreManager.sharedInstance.SetFreezeScore(true);
            float timer = 0f;
            while (timer <= modifierDuration) {
                timer += Time.deltaTime / Time.timeScale;
                yield return null;
            }
            ScoreManager.sharedInstance.SetFreezeScore(false);
            ExpireModifier();
        }

        public override void EndModifierEffects() {
            StopAllCoroutines();
            ScoreManager.sharedInstance.SetFreezeScore(false);
            ExpireModifier();
        }
    }
}
