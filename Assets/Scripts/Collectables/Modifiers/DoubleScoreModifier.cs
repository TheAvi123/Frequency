using UnityEngine;
using System.Collections;

public class DoubleScoreModifier : Modifier
{
    //Internal Methods
    protected override IEnumerator ModifierEffect() {
        ScoreManager.sharedInstance.SetDoubleScore(true);
        float timer = 0f;
        while (timer <= modifierDuration) {
            timer += Time.deltaTime / Time.timeScale;
            yield return null;
        }
        ScoreManager.sharedInstance.SetDoubleScore(false);
        ExpireModifier();
    }

    public override void EndModifierEffects() {
        StopAllCoroutines();
        ScoreManager.sharedInstance.SetDoubleScore(false);
        ExpireModifier();
    }
}
