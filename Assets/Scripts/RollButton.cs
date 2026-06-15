using UnityEngine;

public class RollButton : MonoBehaviour
{
    public GachaManager gachaManager;

    public void Click()
    {
        Monsters result = gachaManager.RollMonster();

        CollectionManager.Instance.AddMonster(result);

        Debug.Log($"Pulled: {result.monsterName} ({result.rarity})");
    }
}
