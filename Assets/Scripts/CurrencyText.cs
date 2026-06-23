using TMPro;
using UnityEngine;

public class CurrencyText : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text currencyPerSecond;

    void Start()
    {
        CurrencyManager.Instance.OnCurrencyChanged += ChangeText;
    }

    void ChangeText(double amount)
    {
        currencyText.text = amount.ToString("0");
        currencyPerSecond.text = $"{CurrencyManager.Instance.totalIncomePerSecond:F0}/s";
    }
}
