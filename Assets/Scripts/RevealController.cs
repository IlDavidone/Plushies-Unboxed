using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RevealController : MonoBehaviour
{
    [SerializeField] private GameObject revealPanel;
    [SerializeField] private TextMeshProUGUI nameText, rarityText, sellValueText;
    [SerializeField] private Image monsterIcon;
    [SerializeField] private Button keepButton, sellButton;
    [SerializeField] private GameObject shinyVFX;

    private Monsters currentMonster;
    private bool isCurrentMonsterShiny;

    void Awake()
    {
        revealPanel.SetActive(false);
    }

    public void PlayReveal(Monsters monsterData, bool isShiny, RarityConfig rarity)
    {
        currentMonster = monsterData;
        isCurrentMonsterShiny = isShiny;

        monsterIcon.sprite = (isCurrentMonsterShiny && currentMonster.shinyIcon != null) ? currentMonster.shinyIcon : currentMonster.baseIcon;

        if(isCurrentMonsterShiny) monsterIcon.color = new Color(1f, 0.95f, 0.4f);
        else monsterIcon.color = Color.white;

        nameText.text = isCurrentMonsterShiny ? $"✨ {currentMonster.monsterName} ✨" : currentMonster.monsterName;
        rarityText.text = currentMonster.rarity.ToString();
        rarityText.color = rarity.color;

        double sellPrice = isCurrentMonsterShiny ? currentMonster.sellValue * currentMonster.shinySellValueMultiplier : currentMonster.sellValue;
        sellValueText.text = $"Sell: ${sellPrice:F0}";

        //shinyVFX.SetActive(isCurrentMonsterShiny);
        revealPanel.SetActive(true);

        keepButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
        keepButton.onClick.AddListener(OnKeep);
        sellButton.onClick.AddListener(() => OnSell(sellPrice));
    }

    private void OnKeep()
    {
        CollectionManager.Instance.AddMonster(currentMonster, isCurrentMonsterShiny);
        string ShelfSlotID = ShelfManager.Instance.GetFirstEmptySlotID();
        if(ShelfSlotID != null)
        {
            ShelfManager.Instance.PlaceOnShelf(ShelfSlotID, currentMonster, isCurrentMonsterShiny);
        }
        revealPanel.SetActive(false);
    }

    private void OnSell(double amount)
    {
        CurrencyManager.Instance.AddCurrency(amount);
        revealPanel.SetActive(false);
    }
}
