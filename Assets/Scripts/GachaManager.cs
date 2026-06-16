using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GachaManager : MonoBehaviour
{
    [SerializeField] private float shinyChance = 0.5f;

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

        List<Monsters> pool = monsterBox.monstersPool;

        if(pityCounters[monsterBox] >= monsterBox.pityThreshold)
        {
            pool = monsterBox.monstersPool.FindAll(m => m.rarity >= monsterBox.guaranteedPityRarity);
            pityCounters[monsterBox] = 0;
        }

        Monsters rollResult = PickWeighted(pool);

        if(rollResult.rarity >= monsterBox.guaranteedPityRarity)   //reset pity counter even if pulled rarity exceeds pity one naturally
            pityCounters[monsterBox] = 0;

        bool isShiny = Random.value <= shinyChance;

        return (rollResult, isShiny);
    }

    private Monsters PickWeighted(List<Monsters> pool)
    {
        float totalWeight = 0f;
        foreach(var m in pool)
        {
            totalWeight += m.rollWeight;
        }

        float roll = Random.Range(0, totalWeight);
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
