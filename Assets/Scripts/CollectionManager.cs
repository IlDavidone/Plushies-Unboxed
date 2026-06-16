using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;

    private Dictionary<string, int> ownedMonsters = new Dictionary<string, int>();
    private Dictionary<string, int> ownedShinies = new Dictionary<string, int>();

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

    public void AddMonster(Monsters monsterData, bool isShiny)      //dict holds a reference to currently used dictionary
    {
        var dict = isShiny ? ownedShinies : ownedMonsters;

        if (!dict.ContainsKey(monsterData.monsterName))
        {
            dict[monsterData.monsterName] = 0;
        }

        dict[monsterData.monsterName]++;
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
    public Dictionary<string, int> GetOwnedShiniesSnapshot() => new Dictionary<string, int>(ownedShinies);

    public void LoadFromSnapshot(Dictionary<string, int> normalSnapshot, Dictionary<string, int> shiniesSnapshot)
    {
        ownedMonsters = normalSnapshot;
        ownedShinies = shiniesSnapshot;
    }
}
