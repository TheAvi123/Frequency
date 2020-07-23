using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager sharedInstance;

    private enum GameState {Null, Splash, Start, Play, Over, Stats, Shop, Tutorial, Options};

    [Header("Scene Names")]
    [SerializeField] string splashSceneName   = null;
    [SerializeField] string startSceneName    = null;
    [SerializeField] string playSceneName     = null;
    [SerializeField] string gameOverSceneName = null;
    [SerializeField] string statsSceneName    = null;
    [SerializeField] string shopSceneName     = null;
    [SerializeField] string tutorialSceneName = null;
    [SerializeField] string optionsSceneName  = null;


    //Configuration Parameters
    [Header("Configuration")]
    [SerializeField] bool startFromInitialState = true;
    [SerializeField] GameState initialState = GameState.Splash;

    //State Variables
    private GameState currentState;
    private Coroutine waitLoader = null;
    private Coroutine fadeLoader = null;
    private bool fadeComplete = false;

    //Internal Methods
    private void Awake() {
        SetSharedInstance();
        SetupPersistenceEnvironment();
        SetCurrentState();
        LoadInitialState();
    }

    private void OnSceneChange() {
        SetCurrentState();
    }   //Called from Singleton

    private void SetSharedInstance() {
        sharedInstance = this;
    }

    public void SetupPersistenceEnvironment() {
        //This code is required for the persistence system's BinaryFormatter to work on iOS devices
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
    }

    private void SetCurrentState() {
        currentState = SceneNameToState(SceneManager.GetActiveScene());
    }

    public void LoadInitialState() {
        if (startFromInitialState && currentState != initialState) {
            currentState = initialState;
            SceneManager.LoadScene(StateToName(initialState));
        }
    }

    private void Start() {
        LoadSaveData();
    }

    private void LoadSaveData() {
        PersistenceManager.LoadGame();
    }

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
            case GameState.Stats:
                return statsSceneName;
            case GameState.Shop:
                return shopSceneName;
            case GameState.Tutorial:
                return tutorialSceneName;
            case GameState.Options:
                return optionsSceneName;
            case GameState.Null:
            default:
                Debug.LogError("Game State " + state.ToString() + " Does Not Exist");
                return null;
        }
    }

    private GameState SceneNameToState(Scene scene) {
        string sceneName = scene.name;
        if (sceneName == splashSceneName) {
            return GameState.Splash;
        } else if (sceneName == startSceneName) {
            return GameState.Start;
        } else if (sceneName == playSceneName) {
            return GameState.Play;
        } else if (sceneName == gameOverSceneName) {
            return GameState.Over;
        } else if (sceneName == statsSceneName) {
            return GameState.Stats;
        } else if (sceneName == shopSceneName) {
            return GameState.Shop;
        } else if (sceneName == tutorialSceneName) {
            return GameState.Tutorial;
        } else if (sceneName == optionsSceneName) {
            return GameState.Options;
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
        LoadState(state);
        waitLoader = null;
    }

    //Load Methods
    public void LoadStartMenu() {
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

    public void LoadStats() {
        LoadState(GameState.Stats);
    }

    public void OpenShop() {
        LoadState(GameState.Shop);
    }

    public void LoadTutorial() {
        LoadState(GameState.Tutorial);
    }

    public void OpenOptions() {
        LoadState(GameState.Options);
    }

    //Public Methods
    public string GetCurrentScene() {
        return SceneManager.GetActiveScene().name;
    }

    public void SetFadeComplete() {
        fadeComplete = true;
    }
}