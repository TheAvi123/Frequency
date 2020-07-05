using UnityEngine;

[CreateAssetMenu]
public class Achievement : ScriptableObject
{
    private enum Stat {Runs, Score, Coins, Modifiers, Flips, Dashes, Delays, NearMisses, Time};

    private string achievementName;
    private Stat statistic;
    private int threshold;
}
