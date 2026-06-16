using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class RevealController : MonoBehaviour
{
    [SerializeField] private GameObject revealPanel;
    [SerializeField] private ShelfView shelfView;
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
 
        monsterIcon.sprite = (isCurrentMonsterShiny && currentMonster.shinyIcon != null)
            ? currentMonster.shinyIcon
            : currentMonster.baseIcon;
        monsterIcon.color = isCurrentMonsterShiny ? new Color(1f, 0.95f, 0.4f) : Color.white;
 
        nameText.text = isCurrentMonsterShiny ? $"* {currentMonster.monsterName} *" : currentMonster.monsterName;
        rarityText.text = currentMonster.rarity.ToString();
        rarityText.color = rarity.color;
 
        double sellPrice = isCurrentMonsterShiny
            ? currentMonster.sellValue * currentMonster.shinySellValueMultiplier
            : currentMonster.sellValue;
        sellValueText.text = $"Sell: ${sellPrice:F0}";
 
        if (shinyVFX != null) shinyVFX.SetActive(isCurrentMonsterShiny);
 
        revealPanel.SetActive(true);
 
        keepButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
        keepButton.onClick.AddListener(OnKeep);
        sellButton.onClick.AddListener(() => OnSell(sellPrice));
    }
 
    private PerkID RollRandomPerk()
    {
        var values = Enum.GetValues(typeof(PerkID));
        return (PerkID)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }
 
    private void OnKeep()
    {
        OwnedMonster newMonster = new OwnedMonster
        {
            instanceId = Guid.NewGuid().ToString(),
            monsterName = currentMonster.monsterName,
            isShiny = isCurrentMonsterShiny,
            perk1 = RollRandomPerk(),
            perk2 = RollRandomPerk()
        };

        CollectionManager.Instance.AddMonsterInstance(newMonster);

        bool placed = ShelfManager.Instance.TryPlace(newMonster);
        if (placed)
            shelfView.RefreshView();
        else
            Debug.Log("Shelf full -- monster kept in collection but not displayed.");

        revealPanel.SetActive(false);
    }
 
    private void OnSell(double amount)
    {
        CurrencyManager.Instance.AddCurrency(amount);
        revealPanel.SetActive(false);
    }
}
