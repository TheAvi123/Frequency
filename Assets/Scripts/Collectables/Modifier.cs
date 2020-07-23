using UnityEngine;
using System.Collections;

public abstract class Modifier : MonoBehaviour
{
    //Configuration Parameters
    [Header("Modifier Parameters")]
    [SerializeField] protected ParticleSystem modifierCollectVFX = null;
    [SerializeField] protected float modifierDuration = 15f;
    [SerializeField] protected string modifierName = "!";

    //Internal Methods
    protected void Start() {
        ResizeModifier();
    }

    private void ResizeModifier() {
        float aspectMultiplier = Camera.main.aspect * 16 / 9;
        transform.localScale *= aspectMultiplier;
    }

    protected void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.tag == "Player") {
            ModifierCollected();
        }
        if (otherCollider.tag == "Disabler") {
            Destroy(gameObject);
        }
    }

    protected void ModifierCollected() {
        ModifierManager.sharedInstance.SetActiveModifier(this, modifierDuration);
        InfoDisplayer.sharedInstance.DisplayInfo(modifierName);
        StatsManager.sharedInstance.AddModifier();
        DisableModifierSpriteAndCollider();
        SpawnCollectVFX();
        ShakeCamera();
        StartCoroutine(ModifierEffect());
    }

    protected void DisableModifierSpriteAndCollider() {
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
    }

    protected void SpawnCollectVFX() {
        GameObject modifierVFX = Instantiate(modifierCollectVFX, transform.position, transform.rotation).gameObject;
        Destroy(modifierVFX.gameObject, 1f);
    }

    protected void ShakeCamera() {
        CameraShaker.sharedInstance.AddCameraShake(0.25f);
    }

    protected void ExpireModifier() {
        ModifierManager.sharedInstance.SetActiveModifier(null, 0f);
        Destroy(gameObject);
    }

    protected abstract IEnumerator ModifierEffect();

    public abstract void EndModifierEffects();
}
