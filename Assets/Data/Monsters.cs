using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "Monsters", menuName = "Game/Monsters")]
public class Monsters : ScriptableObject
{
    public string monsterName;
    public Sprite baseIcon;
    public Sprite shinyIcon;
    public Rarity rarity;
    public float baseIncome;
    public float rollWeight;
    public double sellValue;
    public double shinySellValueMultiplier = 3f;
    [TextArea] public string flavorText;
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary,
    Secret
}
