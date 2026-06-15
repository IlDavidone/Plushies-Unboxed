using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;

    private Dictionary<string, int> ownedMonsters = new Dictionary<string, int>();
    public List<Monsters> allMonsterData; //reference for lookups by name

    public event Action<Monsters, int> OnMonsterCollected;

    void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;
    }

    public void AddMonster(Monsters monsterData)
    {
        if (!ownedMonsters.ContainsKey(monsterData.monsterName))
        {
            ownedMonsters[monsterData.monsterName] = 0;
        }

        ownedMonsters[monsterData.monsterName]++;
        OnMonsterCollected?.Invoke(monsterData, ownedMonsters[monsterData.monsterName]);
    }

    public int GetMonsterCount(Monsters monsterData)
    {
        return ownedMonsters.TryGetValue(monsterData.name, out int count) ? count : 0;
    }

    public float GetTotalIncomePerSecond()
    {
        float total = 0f;
        foreach(var kvp in ownedMonsters)
        {
            Monsters monsterData = allMonsterData.Find(m => m.monsterName == kvp.Key);
            if(monsterData != null)
            {
                total += monsterData.baseIncome * kvp.Value;
            } 
        }

        return total;
    }

    public Dictionary<string, int> GetOwnedMonstersSnapshot() => new Dictionary<string, int>(ownedMonsters);

    public void LoadFromSnapshot(Dictionary<string, int> snapshot) => ownedMonsters = snapshot;
}
