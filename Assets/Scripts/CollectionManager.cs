using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;

    public List<OwnedMonster> ownedMonsters = new List<OwnedMonster>();

    public List<Monsters> allMonsterData; //reference for lookups by name

    public event Action<OwnedMonster> OnMonsterAdded;

    void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;
    }

    public void AddMonsterInstance(OwnedMonster monster)
    {
        ownedMonsters.Add(monster);
        OnMonsterAdded?.Invoke(monster);
    }

    public int GetMonsterCount(string monsterName)
    {
        return ownedMonsters.FindAll(m => m.monsterName == monsterName).Count;
    }

    public List<OwnedMonster> GetOwnedMonstersSnapshot() => new List<OwnedMonster>(ownedMonsters);

    public void LoadFromSnapshot(List<OwnedMonster> normalSnapshot)
    {
        ownedMonsters = normalSnapshot;
    }
}
