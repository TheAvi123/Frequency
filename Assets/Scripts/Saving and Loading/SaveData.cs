[System.Serializable]
public class SaveData
{
    public int highscore;
    public int coins;

    public SaveData() {
        highscore = ScoreManager.sharedInstance.GetHighScore();
        coins = CoinManager.sharedInstance.GetCoinsTotal(); 
    }
}
