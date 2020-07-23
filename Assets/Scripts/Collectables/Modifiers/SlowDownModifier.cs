using UnityEngine;
using System.Collections;

public class SlowDownModifier : Modifier
{
    //Configuration Parameters
    [Header("Custom Modifier Parameters")]
    [SerializeField] float speedMultiplier = 0.7f;

    [Header("Transition Parameters")]
    [SerializeField] float slowDownMultiplier = 5f;
    [SerializeField] float speedUpMultiplier = 1f;

    //Internal Methods
    protected override IEnumerator ModifierEffect() {
        float timer = 0f;
        while (Time.timeScale > speedMultiplier + 0.01f) {
            if (Time.timeScale != 0) {
                Time.timeScale = Mathf.Lerp(Time.timeScale, speedMultiplier, Time.deltaTime * slowDownMultiplier);
                timer += Time.deltaTime / Time.timeScale;
            }
            yield return null;
        }
        while (timer <= modifierDuration) {
            if (Time.timeScale != 0) {
                timer += Time.deltaTime / Time.timeScale;
            }
            yield return null;
        }
        timer = 0f;
        while (Time.timeScale < 0.99f) {
            if (Time.timeScale != 0) {
                Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.deltaTime * speedUpMultiplier);
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
        ExpireModifier();
    }
}
