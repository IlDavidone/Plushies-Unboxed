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

public enum DetailTab
{
    Info,
    Perks
}

public class MonsterDetailPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private GameObject collectionTab;
    [SerializeField] private Image monsterIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI monsterId;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI incomeText;
    [SerializeField] private TextMeshProUGUI sellValueText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image perk1Icon;
    [SerializeField] private TextMeshProUGUI perk1Title;
    [SerializeField] private TextMeshProUGUI perk1Description;
    [SerializeField] private Image perk2Icon;
    [SerializeField] private TextMeshProUGUI perk2Title;
    [SerializeField] private TextMeshProUGUI perk2Description;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TextMeshProUGUI abilityTitle;
    [SerializeField] private TextMeshProUGUI abilityDescription;
    [SerializeField] private TextMeshProUGUI uniquePowerText;
    [SerializeField] private Button equipCounterButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private TextMeshProUGUI equipButtonLabel;
    [SerializeField] private TextMeshProUGUI equipCounterButtonLabel;
    [SerializeField] private Button closeButton;

    [Space] 
    [SerializeField] private GameObject perkTab;
    [SerializeField] private GameObject descriptionTab;
    [SerializeField] private Button nextTabButton;

    [Space]
    public ShelfView shelfView;
    public PerkDatabase perkDatabase;
    public AbilityDatabase abilityDatabase;
    public CounterButton counterButton1;
    public CounterButton counterButton2;

    private OwnedMonster currentInstance;
    private Monsters currentMonsterData;
    private DetailTab currentTab;

    void Awake()
    {
        panelRoot.SetActive(false);
        equipCounterButton.onClick.AddListener(OnEquipToCounterPressed);
        equipButton.onClick.AddListener(OnEquipPressed);
        closeButton.onClick.AddListener(Close);
        nextTabButton.onClick.AddListener(CycleTab);
    }

    private void CycleTab()
    {
        currentTab = (DetailTab)(((int)currentTab + 1) % 2);
        ApplyTab();
    }

    private void ApplyTab()
    {
        descriptionTab.SetActive(currentTab == DetailTab.Info);
        perkTab.SetActive(currentTab == DetailTab.Perks);
    }

    public void ShowDetails(OwnedMonster monsterInstance, Monsters monster)
    {
        currentInstance = monsterInstance;
        currentMonsterData = monster;

        PerkDefinition perk1 = perkDatabase.Get(monsterInstance.perk1);
        PerkDefinition perk2 = perkDatabase.Get(monsterInstance.perk2);

        AbilityDefinition ability = abilityDatabase.Get(monster.uniquePowerId);

        monsterIcon.sprite = monsterInstance.isShiny ? monster.shinyIcon : monster.baseIcon;
        nameText.text = monster.monsterName;
        rarityText.text = monster.rarity.ToString();
        incomeText.text = $"Income: ${monster.baseIncome:F1}/sec";
        sellValueText.text = $"Sell Value: ${monster.sellValue:F0}";
        descriptionText.text = monster.flavorText;
        uniquePowerText.text = monster.uniquePowerId != AbiltyID.None
            ? monster.uniquePowerDescription
            : "No unique power";

        abilityTitle.text = "Innate Ability: " + ability.title;
        abilityDescription.text = ability.description;
        abilityIcon.sprite = ability.icon;
        
        perk1Title.text = "Perk 1: " + perk1.title;
        perk1Description.text = perk1.description;
        perk1Icon.sprite = perk1.icon;

        perk2Title.text = "Perk 2: " + perk2.title;
        perk2Description.text = perk2.description;
        perk2Icon.sprite = perk2.icon;

        monsterId.text = monsterInstance.instanceId;

        RefreshEquipButtons();

        panelRoot.SetActive(true);
        collectionTab.SetActive(false);

        currentTab = DetailTab.Perks;
        ApplyTab();
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
            ToastMessage.Instance.Show("Not Equipped - No empty shelf slot available");

        shelfView.RefreshView();
        RefreshEquipButtons();
    }

    public void OnEquipToCounterPressed()
    {
        CounterSlot slot = CounterManager.Instance.GetFirstFreeSlot();
        if (slot == null)
        {
            ToastMessage.Instance.Show("Cannot be equipped - Counter slots full");
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