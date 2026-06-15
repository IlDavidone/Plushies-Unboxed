using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    public List<Monsters> allMonsters;

    [SerializeField] private float pityCounter = 0;
    [SerializeField] private float pityThreshold = 20;
    
    public Monsters RollMonster()
    {
        pityCounter++;

        List<Monsters> pool = allMonsters;

        if(pityCounter >= pityThreshold)
        {
            pool.FindAll(m => m.rarity >= Rarity.Rare);
            pityCounter = 0;
        }

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
