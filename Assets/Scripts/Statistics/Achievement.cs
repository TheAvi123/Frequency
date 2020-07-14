using UnityEngine;

[CreateAssetMenu(fileName = "NewAcheievement", menuName = "Custom Assets/Achievement")]
public class Achievement : ScriptableObject
{
    public new string name;
    public string description;
    public StatsManager.Stat goalStatistic;
    public int goalThreshold;
    public StatsManager.Stat conditionStatistic = StatsManager.Stat.Null;
    public int conditionThreshold = 0;
    public int coinReward;
}
    