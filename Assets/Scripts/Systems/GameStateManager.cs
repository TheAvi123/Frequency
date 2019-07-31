using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager sharedInstance;

    private enum GameState {Splash, Start, Playing, Over, Options, Shop, HighScores}

    [Header("Scene Names")]
    [SerializeField] string splashSceneName = null;
    [SerializeField] string startSceneName = null;
    [SerializeField] string playSceneName = null;
    [SerializeField] string gameOverSceneName = null;
    [SerializeField] string optionsSceneName = null;
    [SerializeField] string shopSceneName = null;
    [SerializeField] string highScoreSceneName = null;

    //Parameters
    [SerializeField] bool startFromInitialState = true;
    [SerializeField] GameState initialState = GameState.Splash;

    //State Variables
    private GameState currentState;

    private string StateToName(GameState state) {
        switch (state) {
            case GameState.Splash:
                return splashSceneName;
            case GameState.Start:
                return startSceneName;
            case GameState.Playing:
                return playSceneName;
            case GameState.Over:
                return gameOverSceneName;
            case GameState.Options:
                return optionsSceneName;
            case GameState.Shop:
                return shopSceneName;
            case GameState.HighScores:
                return highScoreSceneName;
            default:
                Debug.LogError("Game State " + state.ToString() + " does not Exist"); 
                return null;
        }
    }

    private void OnEnable() {
        sharedInstance = this;
        if (startFromInitialState) {
            LoadInitialState();
        }
    }

    public void LoadInitialState() {
        currentState = initialState;
        SceneManager.LoadScene(StateToName(initialState));
    }

    private void LoadState(GameState state) {
        SceneManager.LoadScene(StateToName(state));
    }

    private IEnumerator WaitAndLoad(GameState state, float delayInSeconds) {
        yield return new WaitForSecondsRealtime(delayInSeconds);
        SceneManager.LoadScene(StateToName(state));
    }

    //Public Methods
    public void StartGame() {
        LoadState(GameState.Playing);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void RestartGame() {
        LoadState(GameState.Start);
    }

    public void GameOver(float delayInSeconds) {
        StartCoroutine(WaitAndLoad(GameState.Over, delayInSeconds));
    }

    public void OpenOptions() {
        LoadState(GameState.Options);
    }

    public void OpenShop() {
        LoadState(GameState.Shop);
    }

    public void ShowHighScores() {
        LoadState(GameState.HighScores);
    }
}
