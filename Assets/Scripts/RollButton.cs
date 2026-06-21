using System;
using UnityEngine;

public class RollButton : MonoBehaviour
{
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private MonsterBoxes monsterBox;
    [SerializeField] private RevealController revealController;
    [SerializeField] private RarityConfig[] rarityConfigs;

    public MonsterBoxes GetBox() => monsterBox;

    public void SetBox(MonsterBoxes box)
    {
        monsterBox = box;
    }

    public void Click()
    {
        if(!CurrencyManager.Instance.TrySpend(monsterBox.cost)) return;

        var result = gachaManager.RollMonster(monsterBox);

        RarityConfig config = Array.Find(rarityConfigs, r => r.rarity == result.monster.rarity); 

        revealController.PlayReveal(result.monster, result.isShiny, config);
    }
}
