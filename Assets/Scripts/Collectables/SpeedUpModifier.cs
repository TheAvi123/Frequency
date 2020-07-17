using UnityEngine;
using System.Collections;

public class SpeedUpModifier : ModifierTemplate
{
    //Configuration Parameters
    [Header("Custom Modifier Parameters")]
    [SerializeField] float speedMultiplier = 1.3f;

    [Header("Transition Parameters")]
    [SerializeField] float timeToSpeedUp = 0.5f;
    [SerializeField] float timeToSlowDown = 2.0f;

    //Internal Methods
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            ModifierCollected();
        }
    }

    protected override IEnumerator ModifierEffect() {
        float modifierTimer = 0f;
        while (modifierTimer <= modifierDuration) {
            Time.timeScale = Mathf.Lerp(Time.timeScale, speedMultiplier, modifierTimer / timeToSpeedUp);
            modifierTimer += Time.deltaTime;
            yield return null;
        }
        modifierTimer = 0f;
        while (Time.timeScale > 1.01) {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, modifierTimer / timeToSlowDown);
            modifierTimer += Time.deltaTime;
            yield return null;
        }
        Time.timeScale = 1f;
        ExpireModifier();
    }
}
