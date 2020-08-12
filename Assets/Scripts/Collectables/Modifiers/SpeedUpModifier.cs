using System.Collections;

using Collectables.Helpers;

using UnityEngine;

// ReSharper disable CompareOfFloatsByEqualityOperator
namespace Collectables.Modifiers {
    public class SpeedUpModifier : Modifier
    {
        //Configuration Parameters
        [Header("Custom Modifier Parameters")]
        [SerializeField] float speedMultiplier = 1.3f;

        [Header("Transition Parameters")]
        [SerializeField] float speedUpMultiplier = 5f;
        [SerializeField] float slowDownMultiplier = 1f;

        [Header("Particle Effects")]
        [SerializeField] float particleFadeTime = 5f;
        [SerializeField] VFXFade speedUpVFX = null;

        //State Variable
        private VFXFade spawnedVFX = null;

        //Internal Methods
        protected override IEnumerator ModifierEffect() {
            float timer = 0f;
            spawnedVFX = Instantiate(speedUpVFX);
            while (Time.timeScale < speedMultiplier - 0.01f) {
                if (Time.timeScale != 0) {
                    Time.timeScale = Mathf.Lerp(Time.timeScale, speedMultiplier, Time.deltaTime * speedUpMultiplier);
                    timer += Time.deltaTime / Time.timeScale;
                }
                yield return null;
            }
            spawnedVFX.FadeInParticles(particleFadeTime);
            while (timer <= modifierDuration) {
                if (Time.timeScale != 0) {
                    timer += Time.deltaTime / Time.timeScale;
                }
                yield return null;
            }
            timer = 0f;
            spawnedVFX.FadeOutParticles(particleFadeTime);
            while (Time.timeScale > 1.01f) {
                if (Time.timeScale != 0) {
                    Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.deltaTime * slowDownMultiplier);
                    timer += Time.deltaTime / Time.timeScale;
                }
                yield return null;
            }
            Time.timeScale = 1f;
            ExpireModifier();
        }

        public override void EndModifierEffects() {
            StopAllCoroutines();
            Time.timeScale = 1;
            Destroy(spawnedVFX.gameObject);
            ExpireModifier();
        }
    }
}
