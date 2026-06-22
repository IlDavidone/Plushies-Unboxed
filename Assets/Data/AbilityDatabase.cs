using System;
using UnityEngine;

[Serializable]
public class AbilityDefinition
{
    public AbiltyID abilityId;
    public string title;
    [TextArea] public string description;
    public Sprite icon;
}

[CreateAssetMenu(fileName = "AbilityDatabase", menuName = "Game/AbilityDatabase")]
public class AbilityDatabase : ScriptableObject
{
    public AbilityDefinition[] abilities;

    public AbilityDefinition Get(AbiltyID id)
    {
        foreach(var a in abilities)
            if(a.abilityId == id) return a;
        
        return null;
    }
}
