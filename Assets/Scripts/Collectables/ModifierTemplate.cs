using UnityEngine;
using System.Collections;

public abstract class ModifierTemplate : MonoBehaviour
{
    //Configuration Parameters
    [Header("Modifier Parameters")]
    [SerializeField] protected ParticleSystem modifierCollectVFX = null;
    [SerializeField] protected float modifierDuration = 15f;

    //Internal Methods
    protected void ModifierCollected() {
        StatsManager.sharedInstance.AddModifier();
        DisableModifierSprite();
        SpawnCollectVFX();
        ShakeCamera();
        StartCoroutine(ModifierEffect());
    }

    protected void DisableModifierSprite() {
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
    }

    protected void SpawnCollectVFX() {
        GameObject modifierVFX = Instantiate(modifierCollectVFX, transform.position, transform.rotation).gameObject;
        Destroy(modifierVFX.gameObject, 1f);
    }

    protected void ShakeCamera() {
        CameraShaker.sharedInstance.AddCameraShake(0.25f);
    }

    protected abstract IEnumerator ModifierEffect();

    protected void ExpireModifier() {
        Destroy(gameObject);
    }
}
