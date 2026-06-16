using System;
using System.Collections.Generic;
using UnityEngine;
 
[Serializable]
public class ShelfSlot
{
    public string slotId;
    [NonSerialized] public OwnedMonster displayedMonster; // runtime only, not Inspector-serialized
    public float slotMultiplier = 1f;
 
    public bool IsEmpty => displayedMonster == null;
}
 
[Serializable]
public class ShelfSlotSaveData
{
    public string slotId;
    public float slotMultiplier;
    public bool hasMonster;
    public OwnedMonster monster; // ignored if hasMonster is false
}
 
public class ShelfManager : MonoBehaviour
{
    public static ShelfManager Instance { get; private set; }
 
    [SerializeField] private int slotCount = 4;
    [SerializeField] private Monsters[] allMonsterData;
 
    private ShelfSlot[] slots;
 
    void Awake()
    {
        Instance = this;
        slots = new ShelfSlot[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            slots[i] = new ShelfSlot { slotId = $"Slot{i + 1}" };
        }
        Debug.Log($"[ShelfManager] Initialized {slots.Length} fresh slots.");
    }
 
    public bool TryPlace(OwnedMonster monster)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i].displayedMonster = monster;
                Debug.Log($"[ShelfManager] Placed {monster.monsterName} in {slots[i].slotId}");
                return true;
            }
        }
        Debug.Log("[ShelfManager] No empty slot available, all full.");
        return false;
    }
 
    public void RemoveFromSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return;
        slots[index].displayedMonster = null;
    }
 
    public ShelfSlot GetSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return null;
        return slots[index];
    }
 
    public int SlotCount => slots.Length;
 
    public IReadOnlyList<ShelfSlot> GetSlots() => slots;
 
    public bool UpgradeSlot(int index, float multiplierIncrease)
    {
        ShelfSlot slot = GetSlot(index);
        if (slot == null) return false;
 
        slot.slotMultiplier += multiplierIncrease;
        Debug.Log($"[ShelfManager] {slot.slotId} multiplier upgraded to {slot.slotMultiplier}");
        return true;
    }
 
    public void UpgradeAllSlots(float multiplierIncrease)
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].slotMultiplier += multiplierIncrease;
    }
 
    public float GetTotalIncome()
    {
        float total = 0f;
 
        foreach (var slot in slots)
        {
            if (slot.IsEmpty) continue;
 
            Monsters data = null;
            for (int i = 0; i < allMonsterData.Length; i++)
            {
                if (allMonsterData[i].monsterName == slot.displayedMonster.monsterName)
                {
                    data = allMonsterData[i];
                    break;
                }
            }
 
            if (data == null) continue;
 
            float income = (float)data.baseIncome;
            if (slot.displayedMonster.isShiny) income *= (float)data.shinySellValueMultiplier;
 
            total += income * slot.slotMultiplier;
        }
 
        return total;
    }

 
    public List<ShelfSlotSaveData> GetSaveSnapshot()
    {
        var result = new List<ShelfSlotSaveData>();
 
        foreach (var slot in slots)
        {
            result.Add(new ShelfSlotSaveData
            {
                slotId = slot.slotId,
                slotMultiplier = slot.slotMultiplier,
                hasMonster = !slot.IsEmpty,
                monster = slot.displayedMonster
            });
        }
 
        return result;
    }
 
    public void LoadFromSnapshot(List<ShelfSlotSaveData> snapshot)
    {
        if (snapshot == null) return;
 
        foreach (var saved in snapshot)
        {
            ShelfSlot slot = Array.Find(slots, s => s.slotId == saved.slotId);
            if (slot == null) continue;
 
            slot.slotMultiplier = saved.slotMultiplier;
            slot.displayedMonster = saved.hasMonster ? saved.monster : null;
        }
    }
}
