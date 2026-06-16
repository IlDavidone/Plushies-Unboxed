using TMPro;
using UnityEngine;

public class CurrencyText : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyText;

    void Start()
    {
        CurrencyManager.Instance.OnCurrencyChanged += ChangeText;
    }

    void ChangeText(double amount)
    {
        currencyText.text = "Money: " + amount.ToString("0.0");
    }
}
