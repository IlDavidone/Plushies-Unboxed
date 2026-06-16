using System;

[Serializable]
public class OwnedMonster
{
    public string instanceId;
    public string monsterName;
    public bool isShiny;
    public PerkID perk1;
    public PerkID perk2;
}

public enum PerkID
{
    SparkleDust,        //+5% income
    QuickPaws,          //+2% shiny chance
    CozyCorner,         //+10% income
    LuckySwing,         //-1 pity globally
    ProductivePal,      //+3% income
    GiftPaper           //-1% box cost
}
