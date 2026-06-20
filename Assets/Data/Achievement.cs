using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Game/Achievement")]
public class Achievement : ScriptableObject
{
    public string achievementId;
    public string title;
    public string description;
    public Sprite icon;
    public double rewardCurrency; // 0 if no reward
}