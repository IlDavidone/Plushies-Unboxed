using System;
using UnityEngine;

[Serializable]
public class PerkDefinition
{
    public PerkID perkId;
    public string title;
    [TextArea] public string description;
    public Sprite icon;
}

[CreateAssetMenu(fileName = "PerkDatabase", menuName = "Game/PerkDatabase")]
public class PerkDatabase : ScriptableObject
{
    public PerkDefinition[] perks;

    public PerkDefinition Get(PerkID id)
    {
        foreach(var p in perks)
            if(p.perkId == id) return p;
        
        return null;
    }
}
