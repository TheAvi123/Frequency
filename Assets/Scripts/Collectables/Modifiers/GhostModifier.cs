using System.Collections;

using Player;

using UnityEngine;

namespace Collectables.Modifiers {
    public class GhostModifier : Modifier
    {
        //Reference Variables
        private PlayerInteractions player = null;

        //Internal Methods
        protected override IEnumerator ModifierEffect() {
            player = FindObjectOfType<PlayerInteractions>();
            player.SetGhostMode(true);
            float timer = 0f;
            while (timer <= modifierDuration) {
                timer += Time.deltaTime / Time.timeScale;
                yield return null;
            }
            player.SetGhostMode(false);
            ExpireModifier();
        }

        public override void EndModifierEffects() {
            StopAllCoroutines();
            player.SetGhostMode(false);
            ExpireModifier();
        }
    }
}
