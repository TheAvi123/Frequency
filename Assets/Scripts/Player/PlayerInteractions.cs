using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    //Reference Variables
    private RectTransform pauseOverlay = null;

    [Header("Visual Effects")]
    [SerializeField] GameObject plusOneVFX = null;
    [SerializeField] ParticleSystem deathVFX = null;

    //Configuration Parameters
    [SerializeField] float playerDeathDelay = 2.5f;

    //State Variables
    private bool playerAlive = true;

    //Modifier Variables
    private bool ghostMode = false;

    //Internal Methods
    private void Awake() {
        FindPauseOverlay();
    }

    private void FindPauseOverlay() {
        RectTransform[] interfaceElements = Resources.FindObjectsOfTypeAll<RectTransform>();
        foreach (RectTransform element in interfaceElements) {
            if (element.tag == "PauseOverlay") {
                pauseOverlay = element;
            }
        }
        if (!pauseOverlay) {
            Debug.LogError("No Pause Overlay Found In Scene");
            enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch (other.tag) {
            case "ObstaclePart":
                ObstacleCollision();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        switch (other.tag) {
            case "NMCollider":
                NearMissTrigger();
                break;
        }
    }

    private void ObstacleCollision() {
        if (ghostMode) {
            //Do Nothing
        } else {
            PlayerDied();
        }
    }

    private void NearMissTrigger() {
        if (ghostMode) {
            //Do Nothing
        } else {
            NearMiss();
        }
    }

    private void PlayerDied() {
        playerAlive = false;
        gameObject.SetActive(false);
        DisableInputControllers();
        StopIncreasingScore();
        DisablePause();
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

    private void DisablePause() {
        pauseOverlay.gameObject.SetActive(false);
    }

    private void ShakeScreen() {
        CameraShaker.sharedInstance.AddCameraShake(1f);
    }

    private void SpawnDeathVFX() {
        Instantiate(deathVFX, transform.position, transform.rotation);
    }

    private void LoadGameOver() {
        GameStateManager.sharedInstance.GameOver(playerDeathDelay);
    }
    #endregion

    private void NearMiss() {
        if (playerAlive) {
            ScoreManager.sharedInstance.AddScore(1);
            StatsManager.sharedInstance.AddNearMiss();
            SpawnPlusOneSprite();
        }
    }
    #region NearMiss Helper Functions
    private void SpawnPlusOneSprite() {
        GameObject plusOne = Instantiate(plusOneVFX, transform.position, Quaternion.identity) as GameObject;
        Destroy(plusOne, 2f);
    }
    #endregion

    //Public Methods
    public void SetGhostMode(bool status) {
        ghostMode = status;
    }
}
