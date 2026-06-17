using UnityEngine;

[CreateAssetMenu(fileName = "Monsters", menuName = "Game/Monsters")]
public class Monsters : ScriptableObject
{
    [Header("Identity")]
    public string monsterName;
    public Sprite baseIcon;
    public Sprite shinyIcon;          // optional; falls back to tint if null
    [TextArea] public string flavorText;

    [Header("Rarity & RNG")]
    public Rarity rarity;
    public float rollWeight;          // higher = more common within its box pool

    [Header("Economy")]
    public float baseIncome;         // currency/sec when displayed on a shelf slot
    public double sellValue;          // instant currency if sold instead of kept
    public float shinySellValueMultiplier = 3f; // applies to both baseIncome and sellValue when shiny

    [Header("Unique Counter Power")]
    [Tooltip("Only active while this monster occupies one of the two counter slots.")]
    public AbiltyID uniquePowerId;
    [TextArea] public string uniquePowerDescription;
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary,
    Secret
}

public enum AbiltyID
{
    None,          //case for all common, rare aand epic monsters
    GoldenPaw,     //+50% shelves income
    LuckyCoin,     //+5% global shiny chance
    BargainMaster, //-20% box cost
    SpeedyPity,    //-5 pity threshold
    DoubleStuffed, //double own income
    CrowdPleaser,  //+1 box roll every 60s
}
