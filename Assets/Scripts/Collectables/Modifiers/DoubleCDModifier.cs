using System.Collections;

using Player;

using UnityEngine;

namespace Collectables.Modifiers {
    public class DoubleCDModifier : Modifier
    {
        //Reference Variables
        private PlayerAbilityManager player = null;

        //Internal Methods
        protected override IEnumerator ModifierEffect() {
            player = FindObjectOfType<PlayerAbilityManager>();
            player.SetDoubleCooldown(true);
            float timer = 0f;
            while (timer <= modifierDuration) {
                timer += Time.deltaTime / Time.timeScale;
                yield return null;
            }
            player.SetDoubleCooldown(false);
            ExpireModifier();
        }

        public override void EndModifierEffects() {
            StopAllCoroutines();
            player.SetDoubleCooldown(false);
            ExpireModifier();
        }
    }
}
