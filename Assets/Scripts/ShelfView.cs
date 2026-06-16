using UnityEngine;
using UnityEngine.UI;

public class ShelfView : MonoBehaviour
{
    public Image[] slotImages;
    public GameObject[] emptyOverlays;

    public void RefreshView()
    {
        for(int i = 0; i < ShelfManager.Instance.SlotCount; i++)
        {
            ShelfSlot slot = ShelfManager.Instance.GetSlot(i);
            if(slot == null) continue;

            if (!slot.IsEmpty)
            {
                Monsters data = LookupMonstersData(slot.displayedMonster.monsterName);
                if(data == null) continue;

                slotImages[i].sprite = (slot.displayedMonster.isShiny && data.shinyIcon != null)
                    ? data.shinyIcon
                    : data.baseIcon;
                slotImages[i].enabled = true;  

                if (emptyOverlays.Length > i) emptyOverlays[i].SetActive(false);
            }
            else
            {
                slotImages[i].enabled = false;
                if (emptyOverlays.Length > i) emptyOverlays[i].SetActive(true);
            }
        }
    }

    private Monsters LookupMonstersData(string monsterName)
    {
        return CollectionManager.Instance.allMonsterData.Find(m => m.monsterName == monsterName);
    }
}
