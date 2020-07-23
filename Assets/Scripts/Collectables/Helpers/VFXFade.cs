using System.Collections;
using UnityEngine;

public class VFXFade : MonoBehaviour
{
    //Reference Variables
    private ParticleSystem spawnedVFX = null;

    //Internal Methods
    private IEnumerator FadeIn(float fadeTime) {
        float timer = 0f;
        spawnedVFX = GetComponent<ParticleSystem>();
        var mainModule = spawnedVFX.main;
        mainModule.startColor = Color.clear;
        while (timer <= fadeTime) {
            mainModule.startColor = Color.Lerp(mainModule.startColor.color, Color.white, Time.deltaTime / 2);
            timer += Time.deltaTime / Time.timeScale;
            yield return null;
        }
        mainModule.startColor = Color.white;
    }

    private IEnumerator FadeOut(float fadeTime) {
        float timer = 0f;
        var mainModule = spawnedVFX.main;
        while (timer <= fadeTime) {
            mainModule.startColor = Color.Lerp(mainModule.startColor.color, Color.clear, Time.deltaTime);
            timer += Time.deltaTime / Time.timeScale;
            yield return null;
        }
        Destroy(gameObject);
    }

    //Public Methods
    public void FadeInParticles(float fadeTime) {
        StartCoroutine(FadeIn(fadeTime));
    }

    public void FadeOutParticles(float fadeTime) {
        StartCoroutine(FadeOut(fadeTime));
    }
}
