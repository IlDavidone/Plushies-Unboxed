using System;
using System.Collections.Generic;
using System.Linq;
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

        CheckCompletionAchievement();

        OnMonsterAdded?.Invoke(monster);
    }

    public int GetMonsterCount(string monsterName)
    {
        return ownedMonsters.FindAll(m => m.monsterName == monsterName).Count;
    }

    public int GetTotalMonsterCount()
    {
        return ownedMonsters.Count;
    }

    private void CheckCompletionAchievement() //doesn't account for secret rarity
    {
        int distinctOwnedSpecies = ownedMonsters
            .Select(m => m.monsterName)
            .Distinct()
            .Count();

        int totalNonMythicSpecies = allMonsterData.Count(m => m.rarity != Rarity.Secret);

        int ownedNonMythicSpecies = ownedMonsters
            .Select(m => m.monsterName)
            .Distinct()
            .Count(name => allMonsterData.Find(m => m.monsterName == name)?.rarity != Rarity.Secret);

        if (ownedNonMythicSpecies >= totalNonMythicSpecies)
            AchivementManager.Instance.TryUnlock("all_discovered");
    }

    public List<OwnedMonster> GetOwnedMonstersSnapshot() => new List<OwnedMonster>(ownedMonsters);

    public void LoadFromSnapshot(List<OwnedMonster> normalSnapshot)
    {
        ownedMonsters = normalSnapshot;
    }
}
