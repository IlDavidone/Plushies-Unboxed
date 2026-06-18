using Unity.VisualScripting;
using UnityEngine;

public class PerkReroller : MonoBehaviour
{
    public double perkRerollCost = 500;

    public bool RerollPerk(OwnedMonster monsterInstance, int perkSlot)
    {
         if(!CurrencyManager.Instance.TrySpend(perkRerollCost)) return false;

         PerkID newPerk = RollRandomPerk();

        switch (perkSlot)
        {
            case 1:
                monsterInstance.perk1 = newPerk;
                break;
            
            case 2:
                monsterInstance.perk2 = newPerk;
                break;
        }

        return true;
    }

    private PerkID RollRandomPerk()
    {
        var values = System.Enum.GetValues(typeof(PerkID));
        return (PerkID)values.GetValue(Random.Range(0, values.Length));
    }
}
