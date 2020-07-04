using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager sharedInstance;

    private enum GameState {Null, Splash, Start, Play, Over, Tutorial, Options, Shop, Stats}

    [Header("Scene Names")]
    [SerializeField] string splashSceneName     = null;
    [SerializeField] string startSceneName      = null;
    [SerializeField] string playSceneName       = null;
    [SerializeField] string gameOverSceneName   = null;
    [SerializeField] string tutorialSceneName   = null;
    [SerializeField] string optionsSceneName    = null;
    [SerializeField] string shopSceneName       = null;
    [SerializeField] string statisticsSceneName = null;

    //Configuration Parameters
    [Header("Configuration")]
    [SerializeField] bool startFromInitialState = true;
    [SerializeField] GameState initialState = GameState.Splash;

    //State Variables
    private GameState currentState;
    private Coroutine waitLoader = null;
    private Coroutine fadeLoader = null;
    private bool fadeComplete = false;

    //Helper Methods
    private string StateToName(GameState state) {
        switch (state) {
            case GameState.Splash:
                return splashSceneName;
            case GameState.Start:
                return startSceneName;
            case GameState.Play:
                return playSceneName;
            case GameState.Over:
                return gameOverSceneName;
            case GameState.Tutorial:
                return tutorialSceneName;
            case GameState.Options:
                return optionsSceneName;
            case GameState.Shop:
                return shopSceneName;
            case GameState.Stats:
                return statisticsSceneName;
            case GameState.Null:
            default:
                Debug.LogError("Game State " + state.ToString() + " Does Not Exist"); 
                return null;
        }
    }

    private GameState SceneToState(Scene scene) {
        if (scene.name == splashSceneName) {
            return GameState.Splash;
        } else if (scene.name == startSceneName) {
            return GameState.Start;
        } else if (scene.name == playSceneName) {
            return GameState.Play;
        } else if (scene.name == gameOverSceneName) {
            return GameState.Over;
        } else if (scene.name == tutorialSceneName) {
            return GameState.Tutorial;
        } else if (scene.name == optionsSceneName) {
            return GameState.Options;
        } else if (scene.name == shopSceneName) {
            return GameState.Shop;
        } else if (scene.name == statisticsSceneName) {
            return GameState.Stats;
        } else {
            Debug.LogError("Scene " + scene.name + " Does Not Exist in GameState Enumeration");
            return GameState.Null;
        }
    }

    private void LoadState(GameState state) {
        fadeLoader = StartCoroutine(FadeAndLoad(state));
    }

    private IEnumerator FadeAndLoad(GameState state) {
        if (fadeLoader != null) {
            Debug.LogWarning("FadeLoader Already in Progress");
            yield break;
        }
        SceneChangeController.sharedInstance.TriggerFadeAnimation();
        yield return new WaitUntil(() => fadeComplete);
        SceneManager.LoadScene(StateToName(state));
        fadeComplete = false;
        fadeLoader = null;
    }

    private IEnumerator WaitAndLoad(GameState state, float delayInSeconds) {
        yield return new WaitForSecondsRealtime(delayInSeconds);
        SceneManager.LoadScene(StateToName(state));
        waitLoader = null;
    }

    //Internal Methods
    private void Awake() {
        SetupPersistenceEnvironment();
        SetSharedInstance();
        SetCurrentState();
        LoadInitialState();
    }

    private void SetSharedInstance() {
        sharedInstance = this;
    }

    private void SetCurrentState() {
        currentState = SceneToState(SceneManager.GetActiveScene());
    }

    public void LoadInitialState() {
        if (startFromInitialState && currentState != initialState) {
            currentState = initialState;
            SceneManager.LoadScene(StateToName(initialState));
        }
    }

    public void SetupPersistenceEnvironment() {
        //This code is required for the persistence system's BinaryFormatter to work on iOS devices
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
    }

    private void Start() {
        LoadSaveData();
    }

    private void LoadSaveData() {
        PersistenceManager.LoadGame();
    }

    private void OnSceneChange() {      
        SetCurrentState();
    }   //Called from Singleton

    //Public Call Methods
    public void LoadMenu() {
        LoadState(GameState.Start);
    }

    public void PlayGame() {
        LoadState(GameState.Play);
    }

    public void QuitGame() {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    public void GameOver(float delayInSeconds) {
        if (waitLoader != null) {
            Debug.LogError("WaitAndLoad Already In Progress!");
            return;
        }
        waitLoader = StartCoroutine(WaitAndLoad(GameState.Over, delayInSeconds));
    }

    public void LoadTutorial() {
        LoadState(GameState.Tutorial);
    }

    public void LoadScores() {
        LoadState(GameState.Stats);
    }

    public void OpenOptions() {
        LoadState(GameState.Options);
    }

    public void OpenShop() {
        LoadState(GameState.Shop);
    }

    //Public Return Methods
    public string GetCurrentScene() {
        return SceneManager.GetActiveScene().name;
    }

    public void FadeComplete() {
        fadeComplete = true;
    }
}