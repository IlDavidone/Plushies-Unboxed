using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class EquipmentChecker
{
    public static bool IsEquippedAnywhere(string instanceId)
    {
        return ShelfManager.Instance.IsInstanceOnShelf(instanceId)
            || CounterManager.Instance.IsInstanceOnCounter(instanceId);
    }
}

public class MonsterDetailPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private GameObject collectionTab;
    [SerializeField] private Image monsterIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI monsterId;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI flavorText;
    [SerializeField] private TextMeshProUGUI incomeText;
    [SerializeField] private TextMeshProUGUI sellValueText;
    [SerializeField] private TextMeshProUGUI perk1;
    [SerializeField] private TextMeshProUGUI perk2;
    [SerializeField] private TextMeshProUGUI uniquePowerText;
    [SerializeField] private Button equipCounterButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private TextMeshProUGUI equipButtonLabel;
    [SerializeField] private TextMeshProUGUI equipCounterButtonLabel;
    [SerializeField] private Button closeButton;

    [Space]
    public ShelfView shelfView;
    public CounterButton counterButton1;
    public CounterButton counterButton2;

    private OwnedMonster currentInstance;
    private Monsters currentMonsterData;

    void Awake()
    {
        panelRoot.SetActive(false);
        equipCounterButton.onClick.AddListener(OnEquipToCounterPressed);
        equipButton.onClick.AddListener(OnEquipPressed);
        closeButton.onClick.AddListener(Close);
    }

    public void ShowDetails(OwnedMonster monsterInstance, Monsters monster)
    {
        currentInstance = monsterInstance;
        currentMonsterData = monster;

        monsterIcon.sprite = monsterInstance.isShiny ? monster.shinyIcon : monster.baseIcon;
        nameText.text = monster.monsterName;
        rarityText.text = monster.rarity.ToString();
        flavorText.text = monster.flavorText;
        incomeText.text = $"Income: ${monster.baseIncome:F1}/sec";
        sellValueText.text = $"Sell Value: ${monster.sellValue:F0}";
        uniquePowerText.text = monster.uniquePowerId != AbiltyID.None
            ? monster.uniquePowerDescription
            : "No unique power";
        perk1.text = "Perk 1: " + monsterInstance.perk1;
        perk2.text = "Perk 2: " + monsterInstance.perk2;
        monsterId.text = monsterInstance.instanceId;

        RefreshEquipButtons();

        panelRoot.SetActive(true);
        collectionTab.SetActive(false);
    }

    private void RefreshEquipButtons()
    {
        bool equippedAnywhere = EquipmentChecker.IsEquippedAnywhere(currentInstance.instanceId);

        // Shelf button: disabled if equipped anywhere
        equipButton.interactable = !equippedAnywhere;
        equipButtonLabel.text = equippedAnywhere ? "Equipped" : "Equip";

        // Counter button: disabled if equipped anywhere, OR if rarity too low for counter
        bool meetsRarity = currentMonsterData.rarity >= Rarity.Legendary; // match CounterManager's minimumRarityAllowed
        equipCounterButton.interactable = !equippedAnywhere && meetsRarity;
        equipCounterButtonLabel.text = equippedAnywhere ? "Equipped" : (meetsRarity ? "Equip to Counter" : "Too Common");
    }

    public void OnEquipPressed()
    {
        bool placed = ShelfManager.Instance.TryPlace(currentInstance);
        if (!placed)
            Debug.Log("Shelf full or already equipped, cannot equip.");

        shelfView.RefreshView();
        RefreshEquipButtons();
    }

    public void OnEquipToCounterPressed()
    {
        CounterSlot slot = CounterManager.Instance.GetFirstFreeSlot();
        if (slot == null)
        {
            Debug.Log("Cannot place on counter — slots full.");
            return;
        }

        int slotIndex = CounterManager.Instance.GetIndexFromId(slot);
        if (slotIndex == -1) return;

        bool placed = CounterManager.Instance.TryPlace(slotIndex, currentInstance);
        if (!placed)
            Debug.Log("Cannot place on counter — already equipped or rarity too low.");

        counterButton1.RefreshView();
        counterButton2.RefreshView();
        RefreshEquipButtons();
    }

    void Close()
    {
        panelRoot.SetActive(false);
        collectionTab.SetActive(true);
    }
}