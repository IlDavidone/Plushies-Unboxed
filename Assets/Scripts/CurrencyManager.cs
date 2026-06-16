using System;
using UnityEngine;
using UnityEngine.Rendering;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public double currency;
    public float clickPower = 1f;
    public float idleIncomeMultiplier = 1f;


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
        OnCurrencyChanged?.Invoke(currency);
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
        AddCurrency(income * Time.deltaTime);
    }
}
