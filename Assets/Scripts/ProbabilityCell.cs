using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProbabilityCell : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI chanceText;
    [SerializeField] private Image rarityBorder;

    public void Setup(Monsters monster, float chancePercent)
    {
        if(CollectionManager.Instance.GetMonsterCount(monster.monsterName) == 0)
            icon.color = Color.black;
        else
            icon.color = Color.white;

        icon.sprite = monster.baseIcon;

        if(monster.rarity == Rarity.Secret)
            chanceText.text = "???";
        else
            chanceText.text = $"{chancePercent:F2}%";

        if (rarityBorder != null)
        {
            rarityBorder.color = GetRarityColor(monster.rarity);
        }
    }

    private Color GetRarityColor(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Color.white;
            case Rarity.Rare: return new Color(0.3f, 0.6f, 1f);
            case Rarity.Epic: return new Color(0.7f, 0.3f, 1f);
            case Rarity.Legendary: return new Color(1f, 0.7f, 0.1f);
            case Rarity.Secret: return new Color(1f, 0.2f, 0.2f);
            default: return Color.gray;
        }
    }
}
