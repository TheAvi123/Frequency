using UnityEngine;
using System.Collections;
using UnityEngine.PlayerLoop;

public class TutorialManager : MonoBehaviour
{
    //Reference Variables
    public static TutorialManager sharedInstance;
    private PlayerWave player = null;

    //Configuration Parameters
    [SerializeField] GameObject gameOverlay = null;
    [SerializeField] GameObject pauseOverlay = null;

    //State Variales
    private int currentStageIndex = 0;

    //Blocking Variables
    [Header("Abilities")]
    private bool flipEnabled = false;
    private bool dashEnabled = false;
    private bool delayEnabled = false;
    //[Header("User Interface")]
    //private bool scoreDisplayEnabled = false;
    //private bool dashDisplayEnabled = false;
    //private bool delayDisplayEnabled = false;

    //Internal Methods
    private void Awake() {
        SetSharedInstance();
        FindPlayer();
    }

    private void SetSharedInstance() {
        sharedInstance = this;
    }

    private void FindPlayer() {
        player = FindObjectOfType<PlayerWave>();
        if (!player) {
            Debug.LogError("No Player Found In Tutorial Scene");
            enabled = false;
        }
    }

    private void Start() {
        SetInputControllerActions();
        StartCoroutine(DisableInterfaceElements());
        SetPlayerDirection();
    }

    private void SetInputControllerActions() {
        InputController inputController = null;
        inputController = FindObjectOfType<InputController>();
        if (!inputController) {
            gameObject.SetActive(false);
        } else {
            inputController.SetTapAction(AttemptPlayerFlip);
            inputController.SetUpSwipeAction(AttemptPlayerDash);
            inputController.SetDownSwipeAction(AttemptPlayerDelay);
        }
    }

    private IEnumerator DisableInterfaceElements() {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < gameOverlay.transform.childCount; i++) {
            gameOverlay.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SetPlayerDirection() {
        player.SetFrequencyToOne();
    }

    //Public Methods
    private void AttemptPlayerFlip() {
        if (flipEnabled) {
            player.Flip();
        }
    }

    private void AttemptPlayerDash() {
        if (dashEnabled) {
            player.Dash();
        }
    }

    private void AttemptPlayerDelay() {
        if (delayEnabled) {
            player.Delay();
        }
    }
}
