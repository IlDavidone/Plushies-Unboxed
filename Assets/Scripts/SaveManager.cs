using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
 
[Serializable]
public class SaveData
{
    public double currency;
    public float idleIncomeMultiplier;
    public List<OwnedMonster> ownedMonsters = new List<OwnedMonster>();
    public List<ShelfSlotSaveData> shelfSlots = new List<ShelfSlotSaveData>();
    public string lastSaveTimeUTC;
}
 
public class SaveManager : MonoBehaviour
{
    public string SavePath => Application.persistentDataPath + "/save.json";
 
    public void Save()
    {
        var data = new SaveData
        {
            currency = CurrencyManager.Instance.currency,
            idleIncomeMultiplier = CurrencyManager.Instance.idleIncomeMultiplier,
            lastSaveTimeUTC = DateTime.UtcNow.ToString("o"),
            ownedMonsters = CollectionManager.Instance.GetOwnedMonstersSnapshot(),
            shelfSlots = ShelfManager.Instance.GetSaveSnapshot()
        };
 
        File.WriteAllText(SavePath, JsonUtility.ToJson(data));
    }
 
    public void Load()
    {
        if (!File.Exists(SavePath)) return;
 
        SaveData save;
        try
        {
            save = JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed, save file may be corrupted: {e}");
            return;
        }
 
        CurrencyManager.Instance.currency = save.currency;
        CurrencyManager.Instance.idleIncomeMultiplier = save.idleIncomeMultiplier;
 
        CollectionManager.Instance.LoadFromSnapshot(new List<OwnedMonster>(save.ownedMonsters));
 
        // ShelfManager.Awake() must have already run and created fresh slots
        // before this Load() call -- it only mutates existing slots, never recreates the array.
        ShelfManager.Instance.LoadFromSnapshot(save.shelfSlots);
 
        if (DateTime.TryParse(save.lastSaveTimeUTC, null,
            System.Globalization.DateTimeStyles.RoundtripKind, out DateTime lastTime))
        {
            double elapsedSeconds = (DateTime.UtcNow - lastTime).TotalSeconds;
            elapsedSeconds = Math.Min(elapsedSeconds, 86400 * 2);
 
            float income = ShelfManager.Instance.GetTotalIncome() * CurrencyManager.Instance.idleIncomeMultiplier;
            double offlineEarnings = income * elapsedSeconds;
 
            if (offlineEarnings > 0)
                ShowOfflineEarningsPopup(offlineEarnings);
        }
    }
 
    private void ShowOfflineEarningsPopup(double amount)
    {
        Debug.Log($"Welcome back! You earned {amount:F0} while offline!");
        CurrencyManager.Instance.AddCurrency(amount);
    }
 
    void OnApplicationQuit() => Save();
 
    void OnApplicationPause(bool pause)
    {
        if (pause) Save();
    }
}
