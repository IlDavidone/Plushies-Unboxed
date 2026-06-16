using UnityEngine;

public class RollButton : MonoBehaviour
{
    [SerializeField] private GachaManager gachaManager;
    [SerializeField] private MonsterBoxes monsterBox;

    public void Click()
    {
        if(!CurrencyManager.Instance.TrySpend(monsterBox.cost)) return;

        var result = gachaManager.RollMonster(monsterBox);
        CollectionManager.Instance.AddMonster(result.monster, result.isShiny);

        string shinyDescriptor = result.isShiny ? "Shiny" : "Normal";

        Debug.Log($"Pulled: {result.monster.monsterName} ({result.monster.rarity}, {shinyDescriptor}) from box: {monsterBox.name}");
    }
}
