using UnityEngine;

[CreateAssetMenu(fileName = "Monsters", menuName = "Game/Monsters")]
public class Monsters : ScriptableObject
{
    public string monsterName;
    public Sprite icon;
    public Rarity rarity;
    public float baseIncome;
    public float rollWeight;
    [TextArea] public string flavorText;
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary 
}
