using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public double currency;
    public float idleIncomeMultiplier;
    public List<string> monsterNames = new List<string>();
    public List<int> monsterCount = new List<int>();
    public List<string> shinyNames = new List<string>();
    public List<int> shinyCount = new List<int>();
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
            lastSaveTimeUTC = DateTime.UtcNow.ToString("o")
        };

        foreach(var kvp in CollectionManager.Instance.GetOwnedMonstersSnapshot())
        {
            data.monsterNames.Add(kvp.Key);
            data.monsterCount.Add(kvp.Value);
        }

        foreach (var kvp in CollectionManager.Instance.GetOwnedShiniesSnapshot())
        {
            data.shinyNames.Add(kvp.Key);
            data.shinyCount.Add(kvp.Value);
        }

        File.WriteAllText(SavePath, JsonUtility.ToJson(data));
    }

    public void Load()
    {
        if(!File.Exists(SavePath)) return;

        SaveData save = JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));

        CurrencyManager.Instance.currency = save.currency;
        CurrencyManager.Instance.idleIncomeMultiplier = save.idleIncomeMultiplier;

        var normalSnapshot = new Dictionary<string, int>();
        for(int i = 0; i < save.monsterNames.Count; i++)
        {
            normalSnapshot[save.monsterNames[i]] = save.monsterCount[i];
        }

        var shinySnapshot = new Dictionary<string, int>();
        for (int i = 0; i < save.shinyNames.Count; i++)
            shinySnapshot[save.shinyNames[i]] = save.shinyCount[i];

        CollectionManager.Instance.LoadFromSnapshot(normalSnapshot, shinySnapshot);

        if (DateTime.TryParse(save.lastSaveTimeUTC, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime lastTime))
        {
            double elapsedSeconds = (DateTime.UtcNow - lastTime).TotalSeconds;
            elapsedSeconds = Math.Min(elapsedSeconds, 86400 * 2);

            float income = CollectionManager.Instance.GetTotalIncomePerSecond() * CurrencyManager.Instance.idleIncomeMultiplier;
            double offlineEarnings = income * elapsedSeconds;

            if (offlineEarnings > 0)
            {
                ShowOfflineEarningsPopup(offlineEarnings);
            }
        }
    }

    private void ShowOfflineEarningsPopup(double amount)
    {
        Debug.Log($"Welcome back! you earned {amount} while offline!");
        CurrencyManager.Instance.AddCurrency(amount);
    }

    void OnApplicationQuit() => Save();

    void OnApplicationPause(bool pause)
    {
        if(pause) 
            Save();
    }
}
