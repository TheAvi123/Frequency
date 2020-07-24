using UnityEngine;
using System.Collections;

public class PlayerInteractions : MonoBehaviour
{
    //Reference Variables
    [Header("Visual Effects")]
    [SerializeField] GameObject plusOneVFX = null;
    [SerializeField] ParticleSystem deathVFX = null;

    //Configuration Parameters
    [SerializeField] float deathFreezeTime = 0.5f;
    [SerializeField] float playerDeathDelay = 2.5f;

    //State Variables
    private bool playerAlive = true;

    //Modifier Variables
    private bool ghostMode = false;

    //Internal Methods
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
            StartCoroutine(PlayerDied());
        }
    }

    private void NearMissTrigger() {
        if (ghostMode) {
            //Do Nothing
        } else {
            NearMiss();
        }
    }

    private IEnumerator PlayerDied() {
        playerAlive = false;
        DisablePlayer();
        DisableInputControllers();
        StopIncreasingScore();
        ClearInfoDisplays();
        RemoveModifiers();
        DisablePause();
        yield return StartCoroutine(WaitForDeathFreeze());
        SpawnDeathVFX();
        ShakeScreen();
        LoadGameOver();
    }
    #region PlayerDeath Helper Functions
    private void DisablePlayer() {
        gameObject.GetComponent<PlayerWave>().enabled = false;
        gameObject.GetComponent<PlayerAbilityManager>().enabled = false;
        gameObject.GetComponent<PlayerDirection>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    private void DisableInputControllers() {
        TouchController touchController = FindObjectOfType<TouchController>();
        if (touchController) {
            touchController.gameObject.SetActive(false);
            return;
        }
        MouseController mouseController = FindObjectOfType<MouseController>();
        if (mouseController) {
            mouseController.gameObject.SetActive(false);
            return;
        }
    }

    private void StopIncreasingScore() {
        ScoreManager.sharedInstance.StopIncreasingScore();
    }

    private void ClearInfoDisplays() {
        InfoDisplayer.sharedInstance.ClearDisplays();
    }

    private void RemoveModifiers() {
        ModifierManager.sharedInstance.EndModifierEffects();
    }

    private void DisablePause() {
        RectTransform[] interfaceElements = Resources.FindObjectsOfTypeAll<RectTransform>();
        foreach (RectTransform element in interfaceElements) {
            if (element.gameObject.name == "PauseCollider") {
                element.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator WaitForDeathFreeze() {
        Time.timeScale = 0f;
        Camera.main.gameObject.GetComponent<PlayerFollow>().DeathZoomAnimation(deathFreezeTime, transform.position);
        yield return new WaitForSecondsRealtime(deathFreezeTime);
        Time.timeScale = 1f;
    }

    private void SpawnDeathVFX() {
        Instantiate(deathVFX, transform.position, transform.rotation);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    private void ShakeScreen() {
        CameraShaker.sharedInstance.AddCameraShake(1f);
    }

    private void LoadGameOver() {
        GameStateManager.sharedInstance.GameOver(playerDeathDelay);
    }
    #endregion

    private void NearMiss() {
        if (playerAlive) {
            ScoreManager.sharedInstance.AddScore(1);
            StatsManager.sharedInstance.AddNearMiss();
            InfoDisplayer.sharedInstance.DisplayInfo("NEAR MISS");
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
