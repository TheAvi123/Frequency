using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private TrailColorChanger playerTrail = null;

    [Header("Visual Effects")]
    [SerializeField] ParticleSystem deathVFX = null;
    [SerializeField] GameObject plusOneVFX = null;

    private bool playerAlive = true;
 
    private void Awake() {
        FindPlayerTrail();
    }

    private void FindPlayerTrail() {
        playerTrail = GetComponent<TrailColorChanger>();
        if (!playerTrail) {
            Debug.LogError("No Player Trail Component Found On Player");
            enabled = false;    //Disables this Component
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.tag) {
            case "ObstaclePart":
                PlayerDied();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        switch (other.tag) {
            case "NMCollider":
                NearMiss();
                break;
        }
    }

    private void NearMiss() {
        if (playerAlive) {
            ScoreManager.sharedInstance.AddScore(1);
            SpawnPlusOneSprite();
        }
    }

    private void SpawnPlusOneSprite() {
        GameObject plusOne = Instantiate(plusOneVFX, transform.position, Quaternion.identity) as GameObject;
        Destroy(plusOne, 2f);
    }

    private void PlayerDied() {
        playerAlive = false;
        gameObject.SetActive(false);
        DisableInputControllers();
        StopIncreasingScore();
        ShakeScreen();
        SpawnDeathVFX();
        LoadGameOver();
    }
    #region PlayerDeath Helper Functions
    private void DisableInputControllers() {
        TouchInputController touchController = FindObjectOfType<TouchInputController>();
        if (touchController) {
            touchController.gameObject.SetActive(false);
            return;
        }
        MouseInputController mouseController = FindObjectOfType<MouseInputController>();
        if (mouseController) {
            mouseController.gameObject.SetActive(false);
            return;
        }
    }

    private void StopIncreasingScore() {
        ScoreManager.sharedInstance.StopIncreasingScore();
    }

    private void ShakeScreen() {
        CameraShaker.sharedInstance.ShakeCamera();
    }

    private void SpawnDeathVFX() {
        ParticleSystem deathParticles = Instantiate(deathVFX, transform.position, transform.rotation) as ParticleSystem;
        ParticleSystem.MainModule psMain = deathParticles.main;
        psMain.startColor = playerTrail.GetTrailColor();
    }

    private void LoadGameOver() {
        GameStateManager.sharedInstance.GameOver(2.5f);
    }
    #endregion
}
