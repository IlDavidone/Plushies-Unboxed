using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShelfSlot
{
    public string slotId;
    public Monsters displayedMonster;
    public bool isShinyDisplayed;
    public float slotMultiplier = 1f;
}

public class ShelfManager : MonoBehaviour
{
    public static ShelfManager Instance;

    public List<ShelfSlot> slots = new List<ShelfSlot>();

    void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;
    }

    public bool PlaceOnShelf(string slotId, Monsters monsterData, bool isShiny)
    {
        ShelfSlot shelfSlot = slots.Find(s => s.slotId == slotId);
        if(shelfSlot == null || shelfSlot.displayedMonster != null) return false;

        shelfSlot.displayedMonster = monsterData;
        shelfSlot.isShinyDisplayed = isShiny;

        return true;
    }

    public void RemoveFromShelf(string slotId)
    {
        ShelfSlot shelfSlot = slots.Find(s => s.slotId == slotId);
        if(shelfSlot != null)
        {
            shelfSlot.displayedMonster = null;
            shelfSlot.isShinyDisplayed = false;
        }
    }

    public string GetFirstEmptySlotID()
    {
        ShelfSlot empty = slots.Find(s => s.displayedMonster == null);
        return empty?.slotId;
    }

    public float GetTotalShelvesIncome()
    {
        float total = 0f;
        foreach(var slot in slots)
        {
            if(slot.displayedMonster == null) continue;

            float baseIncome = slot.displayedMonster.baseIncome;
            if(slot.isShinyDisplayed) baseIncome *= (float)slot.displayedMonster.shinySellValueMultiplier;

            total += baseIncome * slot.slotMultiplier;
        }

        return total;
    }
}
