using System.Collections;

using Collectables.Helpers;

using Player;

using UnityEngine;

namespace Collectables.Modifiers {
    public class ShieldModifier : Modifier
    {
        //Reference Variables
        [SerializeField] GameObject shieldPrefab = null;

        //State Variables
        private GameObject shield = null;

        //Internal Methods
        protected override IEnumerator ModifierEffect() {
            Transform player = FindObjectOfType<PlayerWave>().transform;
            shield = Instantiate(shieldPrefab, player.position, player.rotation).gameObject;
            shield.transform.SetParent(player);
            float timer = 0f;
            while (timer <= modifierDuration) {
                timer += Time.deltaTime / Time.timeScale;
                yield return null;
            }
            if (shield) {
                shield.GetComponent<Shield>().DestroyShield();
            }
            ExpireModifier();
        }

        public override void EndModifierEffects() {
            StopAllCoroutines();
            if (shield) {
                shield.GetComponent<Shield>().DestroyShield();
            }
            ExpireModifier();
        }
    }
}
