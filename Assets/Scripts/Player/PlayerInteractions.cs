using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private TrailColorChanger playerTrail = null;

    [Header("Visual Effects")]
    [SerializeField] ParticleSystem deathVFX = null;

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
        if (other.tag == "ObstaclePart") {
            PlayerDied();
        }
    }

    private void PlayerDied() {
        gameObject.SetActive(false);
        DisableInputControllers();
        StopIncreasingScore();
        ShakeScreen();
        SpawnDeathVFX();
        LoadGameOver();
    }

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
}
