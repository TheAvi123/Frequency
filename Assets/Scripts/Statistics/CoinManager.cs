using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    //Reference Variables
    public static CoinManager sharedInstance;

    //Configuration Parameters
    [SerializeField] float coinDisplayTime = 3f;

    //State Variales
    private int coinsTotal;
    private int coinsCollected;

    private TextMeshProUGUI coinDisplay = null;

    //Internal Methods
    private void Awake() {
        SetSharedInstance();
    }

    private void SetSharedInstance() {
        sharedInstance = this;
    }
    
    private void OnSceneChange() {
        if (SceneManager.GetActiveScene().name == "StartMenu") {
            FindCoinDisplay();
            UpdateTotalCoinDisplay();
        }
        if (SceneManager.GetActiveScene().name == "PlayScene") {
            FindCoinDisplay();
            ResetCollectedCoins();
        }
        if (SceneManager.GetActiveScene().name == "GameOver") {
            TransferCoins();
        }
        if (SceneManager.GetActiveScene().name == "Tutorial") {
            FindCoinDisplay();
            ResetCollectedCoins();
        }
    }   //Called Through Singleton

    private void FindCoinDisplay() {
        if (gameObject.activeInHierarchy) {
            coinDisplay = GameObject.FindGameObjectWithTag("CoinDisplay").GetComponent<TextMeshProUGUI>();
        }
    }

    private void ResetCollectedCoins() {
        coinsCollected = 0;
        coinDisplay.text = coinsCollected.ToString();
        coinDisplay.gameObject.SetActive(false);
    }

    private void TransferCoins() {
        coinsTotal += coinsCollected;
    }

    private IEnumerator UpdateCoinDisplay() {
        coinDisplay.gameObject.SetActive(true);
        coinDisplay.text = coinsCollected.ToString();
        yield return new WaitForSecondsRealtime(coinDisplayTime);
        coinDisplay.gameObject.SetActive(false);
    }

    private void UpdateTotalCoinDisplay() {
        coinDisplay.text = coinsTotal.ToString();
    }

    //Public Methods
    public void CollectCoin() {
        coinsCollected++;
        StopAllCoroutines();
        StartCoroutine(UpdateCoinDisplay());
    }

    public int GetCoinsCollected() {
        return coinsCollected;
    }

    public void AddCoins(int amountToAdd) {
        coinsTotal += amountToAdd;
    }

    public bool SpendCoins(int amountToRemove) {
        if (coinsTotal >= amountToRemove) {
            coinsTotal -= amountToRemove;
            return true;    //Purchase Successful
        } else {
            return false;   //Purchase Failed Due to Insufficient Coins
        }
    }

    public void SetCoinsTotal(int coins) {
        coinsTotal = coins;
    }

    public int GetCoinsTotal() {
        return coinsTotal;
    }
}
