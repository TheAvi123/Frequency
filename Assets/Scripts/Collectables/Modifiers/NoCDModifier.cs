using System.Collections;

using Player;

using UnityEngine;

namespace Collectables.Modifiers {
    public class NoCDModifier : Modifier
    {
        //Reference Variables
        private PlayerAbilityManager player = null;

        //Internal Methods
        protected override IEnumerator ModifierEffect() {
            player = FindObjectOfType<PlayerAbilityManager>();
            player.SetRemovedCooldown(true);
            float timer = 0f;
            while (timer <= modifierDuration) {
                timer += Time.deltaTime / Time.timeScale;
                yield return null;
            }
            player.SetRemovedCooldown(false);
            ExpireModifier();
        }

        public override void EndModifierEffects() {
            StopAllCoroutines();
            player.SetDoubleCooldown(false);
            ExpireModifier();
        }
    }
}
