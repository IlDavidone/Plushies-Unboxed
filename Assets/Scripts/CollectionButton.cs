using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionButton : MonoBehaviour
{
    [SerializeField] private Image monsterIcon;

    private List<Monsters> allMonsters = new List<Monsters>();
    private List<OwnedMonster> ownedMonsters = new List<OwnedMonster>();

    void Start()
    {
        allMonsters = CollectionManager.Instance.allMonsterData;
        ownedMonsters = CollectionManager.Instance.ownedMonsters;

        StartCoroutine(CycleMonsters());
    }

    private IEnumerator CycleMonsters()
    {
        while (true)
        {
            ownedMonsters = CollectionManager.Instance.ownedMonsters;

            if (ownedMonsters.Count == 0)
            {
                yield return new WaitForSeconds(1f); // wait and recheck
                continue;
            }

            int randomIndex = Random.Range(0, ownedMonsters.Count);
            OwnedMonster randomMonster = ownedMonsters[randomIndex];
            Monsters desiredMonster = allMonsters.Find(m => m.monsterName == randomMonster.monsterName);

            if (desiredMonster != null)
            {
                monsterIcon.sprite = randomMonster.isShiny ? desiredMonster.shinyIcon : desiredMonster.baseIcon;
            }

            yield return new WaitForSeconds(3f);
        }
    }
}
