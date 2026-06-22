using System;
using System.Collections.Generic;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    [SerializeField] private float shinyChance = 0.01f;

    public List<Monsters> allMonsters;

    private Dictionary<MonsterBoxes, int> pityCounters = new Dictionary<MonsterBoxes, int>(); 
    private Dictionary<MonsterBoxes, int> exclusiveMonsterPityCounter = new Dictionary<MonsterBoxes, int>();
    
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

        if(!pityCounters.ContainsKey(monsterBox))
            pityCounters[monsterBox] = 0;

        pityCounters[monsterBox]++;

        if(!exclusiveMonsterPityCounter.ContainsKey(monsterBox))
            exclusiveMonsterPityCounter[monsterBox] = 0;

        exclusiveMonsterPityCounter[monsterBox]++;              //shuffled functions checkers - need to order them

        if(exclusiveMonsterPityCounter[monsterBox] >= monsterBox.exclusiveMonsterPity)
        {
            exclusiveMonsterPityCounter[monsterBox] = 0;
            Monsters exclusiveMonster = allMonsters.Find(m => m.rarity == Rarity.Secret);
            return (exclusiveMonster, UnityEngine.Random.value <= shinyChance + (ShelfManager.Instance.GetGlobalShinyBonus() / 100) + (CounterManager.Instance.GetShinyChanceCounterBonus() / 100));
        }

        List<Monsters> pool = monsterBox.monstersPool;

        if(pityCounters[monsterBox] >= monsterBox.pityThreshold - (ShelfManager.Instance.GetGlobalPityReduction() + CounterManager.Instance.GetBoxPityReduction()))
        {
            pool = monsterBox.monstersPool.FindAll(m => m.rarity >= monsterBox.guaranteedPityRarity);
            pityCounters[monsterBox] = 0;
        }

        Monsters rollResult = PickWeighted(pool);

        if(rollResult.rarity >= monsterBox.guaranteedPityRarity)   //reset pity counter even if pulled rarity exceeds pity rarity naturally
            pityCounters[monsterBox] = 0;

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
}
