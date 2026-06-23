using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PityData
{
    public string boxId;
    public int pity;
    public int exclusivePity;
}

public class GachaManager : MonoBehaviour
{
    [SerializeField] private float shinyChance = 0.01f;

    public List<Monsters> allMonsters;

    private Dictionary<string, PityData> pity = new();
    
    public (Monsters monster, bool isShiny) RollMonster(MonsterBoxes monsterBox)
    {
        /*pityCounter++;

        List<Monsters> pool = allMonsters;

        if(pityCounter >= pityThreshold)
        {
            pool.FindAll(m => m.rarity >= Rarity.Rare);
            pityCounter = 0;
        }
        */

        if(!pity.ContainsKey(monsterBox.boxName))
            pity[monsterBox.boxName] = new PityData
            {
                boxId = monsterBox.boxName,
                pity = 0,
                exclusivePity = 0
            };

        var data = pity[monsterBox.boxName];

        data.pity++;
        data.exclusivePity++;

        if(data.exclusivePity >= monsterBox.exclusiveMonsterPity)
        {
            data.exclusivePity = 0;
            Monsters exclusiveMonster = allMonsters.Find(m => m.rarity == Rarity.Secret);
            return (exclusiveMonster, UnityEngine.Random.value <= shinyChance + (ShelfManager.Instance.GetGlobalShinyBonus() / 100) + (CounterManager.Instance.GetShinyChanceCounterBonus() / 100));
        }

        List<Monsters> pool = monsterBox.monstersPool;

        if(data.pity >= monsterBox.pityThreshold - (ShelfManager.Instance.GetGlobalPityReduction() + CounterManager.Instance.GetBoxPityReduction()))
        {
            pool = monsterBox.monstersPool.FindAll(m => m.rarity >= monsterBox.guaranteedPityRarity);
            data.pity = 0;
        }

        Monsters rollResult = PickWeighted(pool);

        if(rollResult.rarity >= monsterBox.guaranteedPityRarity)   //reset pity counter even if pulled rarity exceeds pity rarity naturally
            data.pity = 0;

        bool isShiny = UnityEngine.Random.value <= shinyChance + (ShelfManager.Instance.GetGlobalShinyBonus() / 100) + (CounterManager.Instance.GetShinyChanceCounterBonus() / 100);

        return (rollResult, isShiny);
    }

    private Monsters PickWeighted(List<Monsters> pool)
    {
        float totalWeight = 0f;
        foreach(var m in pool)
        {
            totalWeight += m.rollWeight;
        }

        float roll = UnityEngine.Random.Range(0, totalWeight);
        float cumulative = 0f;
        foreach(var m in pool)
        {
            cumulative += m.rollWeight;
            if(roll <= cumulative)
            {
                return m;
            }
        }

        return pool[pool.Count - 1];
    }

    public int GetPityCount(MonsterBoxes box)
    {
        return pity.TryGetValue(box.boxName, out var data)
            ? data.pity
            : 0;
    }

    public int GetExclusivePityCount(MonsterBoxes box)
    {
        return pity.TryGetValue(box.boxName, out var data)
            ? data.exclusivePity
            : 0;
    }

    public int GetEffectivePityThreshold(MonsterBoxes box)
    {
        return Mathf.Max(
            1,
            box.pityThreshold -
            (ShelfManager.Instance.GetGlobalPityReduction() +
            CounterManager.Instance.GetBoxPityReduction())
        );
    }

    public List<PityData> GetPitySnapshot()
    {
        return new List<PityData>(pity.Values);
    }

    public void LoadPitySnapshot(List<PityData> data)
    {
        pity.Clear();

        if (data == null)
            return;

        foreach (var entry in data)
        {
            pity[entry.boxId] = entry;
        }
    }
}
