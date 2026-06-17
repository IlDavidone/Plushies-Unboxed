using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Button closeButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        panelRoot.SetActive(false);
        closeButton.onClick.AddListener(Close);
    }

    public void ShowDetails(OwnedMonster monsterInstance, Monsters monster)
    {
        monsterIcon.sprite = monster.baseIcon;
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

        panelRoot.SetActive(true);
        collectionTab.SetActive(false);
    }

    void Close()
    {
        panelRoot.SetActive(false);
        collectionTab.SetActive(true);
    }
}
