using System;
using Mono.Cecil;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[Serializable]
public class CounterSlot
{
    public string slotId;
    [NonSerialized] public OwnedMonster displayedMonster;

    public bool IsEmpty => displayedMonster == null;
}

public class CounterManager : MonoBehaviour
{
    public static CounterManager Instance {get; private set;}

    public Monsters[] allMonsterData;
    [SerializeField] private Rarity minimumRarityAllowed = Rarity.Legendary;

    private CounterSlot[] counterSlots;
    private float crowdPleaserTimer = 0f;

    public event Action OnFreeRollTriggered;

    void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;

        counterSlots = new CounterSlot[2];
        counterSlots[0] = new CounterSlot{ slotId = "Counter_1" };
        counterSlots[1] = new CounterSlot{ slotId = "Counter_2" };

        Debug.Log("[CounterManager] Initialized 2 counter slots.");
    }

    void Update()
    {
        if (!HasAbility(AbiltyID.CrowdPleaser)) return;

        crowdPleaserTimer += Time.deltaTime;
        if (crowdPleaserTimer >= 60f)
        {
            crowdPleaserTimer = 0f;
            OnFreeRollTriggered?.Invoke();
            Debug.Log("[CounterManager] Crowd Pleaser triggered a free roll!");
        }
    }

    public bool TryPlace(int index, OwnedMonster monster)
    {
        if (index < 0 || index >= counterSlots.Length) return false;
        if (!counterSlots[index].IsEmpty) return false;

        if (EquipmentChecker.IsEquippedAnywhere(monster.instanceId))
        {
            Debug.Log($"[CounterManager] {monster.monsterName} is already equipped elsewhere.");
            return false;
        }

        Monsters data = LookupMonsterData(monster.monsterName);
        if (data == null || data.rarity < minimumRarityAllowed)
        {
            Debug.Log($"[CounterManager] {monster.monsterName} is below minimum rarity for the counter.");
            return false;
        }

        counterSlots[index].displayedMonster = monster;
        Debug.Log($"[CounterManager] Placed {monster.monsterName} in {counterSlots[index].slotId}");
        return true;
    }

    public void RemoveFromSlot(int index)
    {
        if(index < 0 || index > counterSlots.Length) return;

        counterSlots[index].displayedMonster = null;
    }

    public CounterSlot GetSlot(int index)
    {
        if(index < 0 || index > counterSlots.Length) return null;
        return counterSlots[index];
    }

    public CounterSlot GetFirstFreeSlot()
    {
        foreach(var slot in counterSlots)
        {
            if(slot.IsEmpty) return slot;
        }

        return null;
    }

    public int GetIndexFromId(CounterSlot slot)
    {
        for(int i = 0; i < counterSlots.Length; i++)
        {
            if(slot.slotId == counterSlots[i].slotId)
            {
                return i;
            }
        } 

        return -1;
    }

    public bool IsInstanceOnCounter(string instanceId)
    {
        foreach(var slot in counterSlots)
        {
            if(!slot.IsEmpty && slot.displayedMonster.instanceId == instanceId)
            {
                return true;
            }
        }

        return false;
    }

    //ab queries

    public float GetShelfIncomeMultiplier()
    {
        float mult = 1f;
        foreach(var slot in counterSlots)
        {
            if(slot.IsEmpty) continue;
            AbiltyID abiltyID = GetAbility(slot.displayedMonster.monsterName);

            if (abiltyID == AbiltyID.GoldenPaw) mult *= 1.25f;
            if (abiltyID == AbiltyID.TavernAmbience) mult *= 2f;
        }

        return mult;
    }

    public float GetShinyChanceCounterBonus()
    {
        float bonus = 0f;
        if(HasAbility(AbiltyID.LuckyCoin)) bonus += 0.04f;
        return bonus;
    }

    public float GetBoxCostDiscount()
    {
        float discount = 0f;
        if (HasAbility(AbiltyID.BargainMaster)) discount += 0.2f;
        return Mathf.Clamp(discount, 0f, 0.9f);
    }

    //helpers

    private bool HasAbility(AbiltyID id)
    {
        foreach(var slot in counterSlots)
        {
            if(slot.IsEmpty) continue;
            if(GetAbility(slot.displayedMonster.monsterName) == id) return true;
        }

        return false;
    }

    private AbiltyID GetAbility(string monsterName)
    {
        Monsters data = LookupMonsterData(monsterName);
        return data != null ? data.uniquePowerId : AbiltyID.None;
    }

    private Monsters LookupMonsterData(string monsterName)
    {
        for(int i = 0; i < allMonsterData.Length; i++)
        {
            if(allMonsterData[i].monsterName == monsterName)
            {
                return allMonsterData[i];
            }
        }

        return null;
    }
}
