using System.Collections;

using Player;

using UnityEngine;

namespace Collectables.Modifiers {
    public class NoCoinsModifier : Modifier
    {
        //Reference Variables
        [SerializeField] GameObject coinDisablerPrefab = null;

        //State Variables
        private Transform disabler = null;

        //Internal Methods
        protected override IEnumerator ModifierEffect() {
            gameObject.GetComponentInChildren<Coin>().gameObject.SetActive(false);
            Transform player = FindObjectOfType<PlayerWave>().transform;
            disabler = Instantiate(coinDisablerPrefab).gameObject.transform;
            float timer = 0f;
            while (timer <= modifierDuration) {
                disabler.position = new Vector2(0, player.position.y);
                timer += Time.deltaTime / Time.timeScale;
                yield return null;
            }
            Destroy(disabler);
            ExpireModifier();
        }

        public override void EndModifierEffects() {
            StopAllCoroutines();
            Destroy(disabler);
            ExpireModifier();
        }
    }
}
