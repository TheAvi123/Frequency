using System.Collections;

using Collectables.Helpers;

using Player;

using UnityEngine;

namespace Collectables.Modifiers {
    public class CoinMagnetModifier : Modifier
    {
        //Reference Variables
        [SerializeField] GameObject coinMagnetPrefab = null;

        //State Variables
        private Transform player = null;
        private GameObject magnet = null;

        //Internal Methods
        protected override IEnumerator ModifierEffect() {
            magnet = Instantiate(coinMagnetPrefab).gameObject;
            player = FindObjectOfType<PlayerWave>().transform;
            float timer = 0f;
            while (timer <= modifierDuration) {
                magnet.transform.position = player.position;
                timer += Time.deltaTime / Time.timeScale;
                yield return null;
            }
            StartCoroutine(EndMagnetEffects());
        }

        private IEnumerator EndMagnetEffects() {
            magnet.GetComponent<CoinMagnet>().StopAttracting();
            while (magnet) {
                magnet.transform.position = player.position;
                yield return null;
            }
            ExpireModifier();
        }

        public override void EndModifierEffects() {
            StopAllCoroutines();
            StartCoroutine(EndMagnetEffects());
        }
    }
}
