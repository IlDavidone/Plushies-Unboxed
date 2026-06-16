using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterBoxes", menuName = "Game/MonsterBoxes")]
public class MonsterBoxes : ScriptableObject
{
    public string boxName;
    public Sprite boxIcon;
    public double cost;
    public List<Monsters> monstersPool;

    [Space]
    public int pityThreshold;
    public Rarity guaranteedPityRarity;

    [Space]
    public int exclusiveMonsterPity;
    public Monsters exclusiveMonster;
}
