using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonsterCell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;

    private Monsters monsterData;
    private OwnedMonster uniqueMonsterInstance;
    private MonsterDetailPanel detailPanel;

    public void Setup(OwnedMonster monsterInstance, Monsters monster, MonsterDetailPanel detail)
    {
        monsterData = monster;
        uniqueMonsterInstance = monsterInstance; 
        detailPanel = detail;
        icon.sprite = monsterInstance.isShiny ? monster.shinyIcon : monster.baseIcon;
    }  

    public void OnPointerClick(PointerEventData eventData)
    {
        detailPanel.ShowDetails(uniqueMonsterInstance ,monsterData);
    }
}
