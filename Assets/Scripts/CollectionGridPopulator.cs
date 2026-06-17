using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectionGridPopulator : MonoBehaviour
{
    [SerializeField] private MonsterCell cellPrefab;
    [SerializeField] private Transform gridContainer;
    [SerializeField] private List<Monsters> allMonsterData;
    [SerializeField] private MonsterDetailPanel detailPanel;

    void OnEnable()
    {
        RefreshGrid();
    }

    public void RefreshGrid()
    {
        foreach (Transform child in gridContainer)
            Destroy(child.gameObject);

        List<OwnedMonster> ownedMonsters = CollectionManager.Instance.ownedMonsters;

        foreach (var owned in ownedMonsters)
        {
            Monsters data = allMonsterData.Find(m => m.monsterName == owned.monsterName);

            Debug.Log($"  Found matching Monsters asset: {data != null}");
            if (data == null) continue;

            MonsterCell cell = Instantiate(cellPrefab, gridContainer);
            cell.Setup(owned, data, detailPanel);
            Debug.Log("  Cell instantiated and set up");
        }
    }
}
