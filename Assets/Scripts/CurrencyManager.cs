using System;
using UnityEngine;
using UnityEngine.Rendering;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public double currency;
    public double totalCurrencyEarned;
    public float clickPower = 1f;
    public float idleIncomeMultiplier = 1f;

    public double totalIncomePerSecond;


    public event Action<double> OnCurrencyChanged;

    void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;
    }

    public void AddCurrency(double amount)
    {
        currency += amount;
        totalCurrencyEarned = currency;
        OnCurrencyChanged?.Invoke(currency);

        CheckCurrencyAchievements();
    }

    public bool TrySpend(double amount)
    {
        if(currency < amount) return false;
        currency -= amount;
        OnCurrencyChanged?.Invoke(currency);
        return true;
    }

    void Update() //passive income calculator
    {
        float income = ShelfManager.Instance.GetTotalIncome() * idleIncomeMultiplier;
        totalIncomePerSecond = income;
        AddCurrency(income * Time.deltaTime);
    }

    private void CheckCurrencyAchievements()
    {
        if (totalCurrencyEarned >= 100) AchivementManager.Instance.TryUnlock("100_currency");
        if (totalCurrencyEarned >= 1000) AchivementManager.Instance.TryUnlock("1000_currency");
        if (totalCurrencyEarned >= 10000) AchivementManager.Instance.TryUnlock("10000_currency");
        if (totalCurrencyEarned >= 100000) AchivementManager.Instance.TryUnlock("100000_currency");
        if (totalCurrencyEarned >= 1000000) AchivementManager.Instance.TryUnlock("1000000_currency");
    }
}
