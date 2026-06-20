using UnityEngine;
using UnityEngine.UI;

public class CounterButton : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject emptyOverlay;
    [SerializeField] private Monsters[] allMonsterData;

    public void RefreshView()
    {
        CounterSlot slot = CounterManager.Instance.GetSlot(slotIndex);
        if (slot == null) return;

        if (!slot.IsEmpty)
        {
            Monsters data = LookupMonsterData(slot.displayedMonster.monsterName);
            if (data != null)
            {
                iconImage.sprite = (slot.displayedMonster.isShiny && data.shinyIcon != null)
                    ? data.shinyIcon
                    : data.baseIcon;
            }

            iconImage.enabled = true;
            emptyOverlay.SetActive(false);
        }
        else
        {
            iconImage.enabled = false;
            emptyOverlay.SetActive(true);
        }
    }

    public void OnRemovePressed()
    {
        CounterManager.Instance.RemoveFromSlot(slotIndex);
        RefreshView();
    }

    private Monsters LookupMonsterData(string monsterName)
    {
        for (int i = 0; i < allMonsterData.Length; i++)
            if (allMonsterData[i].monsterName == monsterName)
                return allMonsterData[i];
        return null;
    }
}