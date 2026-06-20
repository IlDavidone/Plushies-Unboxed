using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class AchivementManager : MonoBehaviour
{
    public static AchivementManager Instance {get; private set;}

    [SerializeField] private Achievement[] allAchievements;

    private HashSet<string> unlockedIds = new HashSet<string>();

    public event Action<Achievement> OnAchievementUnlock;

    void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;
    }

    public void TryUnlock(string achievementId)
    {
        if(unlockedIds.Contains(achievementId)) return;

        Achievement achievement = Array.Find(allAchievements, a => achievementId == a.achievementId);
        if(achievement == null)
            return;

        unlockedIds.Add(achievementId);

        if(achievement.rewardCurrency > 0)
        {
            CurrencyManager.Instance.AddCurrency(achievement.rewardCurrency);
        }

        OnAchievementUnlock?.Invoke(achievement);
    }

    public bool IsUnlocked(string achievementId)
    {
        return unlockedIds.Contains(achievementId);
    }

    public IReadOnlyCollection<string> GetUnlockedSnapshot() => unlockedIds;

    public void LoadFromSnapshot(List<string> savedIds)
    {
        unlockedIds = new HashSet<string>(savedIds);
    }
}
