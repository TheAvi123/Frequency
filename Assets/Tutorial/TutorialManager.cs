using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private enum Stage {Intro, Basics, Flip, Dash, Delay, Combo, Score, End}

    //Reference Variables
    private GameStateManager gameManager;
    private UIInputController inputController;
    private PlayerAbilityManager abilityManager;

    //Configuration Parameters
    [SerializeField] GameObject tutorialOverlay = null;

    //State Variales
    private Stage currentStage = 0;

    //Internal Methods
    private void Awake() {
        FindGameManager();
        FindInputController();
        FindAbiltiyManager();
    }

    private void FindGameManager() {
        gameManager = GameStateManager.sharedInstance;
    }

    private void FindInputController() {
        inputController = FindObjectOfType<UIInputController>();
    }

    private void FindAbiltiyManager() {
        abilityManager = FindObjectOfType<PlayerAbilityManager>();
    }

    private void Start() {
        DisableRegularSystems();
    }

    private void DisableRegularSystems() {
        gameManager.gameObject.SetActive(false);
        inputController.gameObject.SetActive(false);
        abilityManager.enabled = false;
        tutorialOverlay.SetActive(false);
    }

    private void Update() {
        FollowCurrentStage();
    }

    private void FollowCurrentStage() {
        switch (currentStage) {
            case Stage.Intro:
                break;
            case Stage.Basics:
                break;
            case Stage.Flip:
                break;
            case Stage.Dash:
                break;
            case Stage.Delay:
                break;
            case Stage.Combo:
                break;
            case Stage.Score:
                break;
            case Stage.End:
                break;
        }
    }
}
