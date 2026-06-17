using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxesManager : MonoBehaviour
{
    public static BoxesManager Instance;

    public List<MonsterBoxes> monsterBoxes = new List<MonsterBoxes>();
    
    [SerializeField] private TMP_Text boxNameText;  
    [SerializeField] private Image boxImage;
    [SerializeField] private Button leftArrowButton;
    [SerializeField] private Button rightArrowButton;

    private int currentBoxIndex = 0;

    void Awake()
    {
        if(Instance != null)
        {
            return;
        }

        Instance = this;
    }

    async Task Start()
    {
        leftArrowButton.onClick.RemoveAllListeners();
        rightArrowButton.onClick.RemoveAllListeners();

        leftArrowButton.onClick.AddListener(CyclePrevious);
        rightArrowButton.onClick.AddListener(CycleNext);

        UpdateUI();
    }
    
    private void CycleNext()
    {
        if(monsterBoxes.Count == 0)
        {
            return;
        }

        currentBoxIndex++;

        if(currentBoxIndex >= monsterBoxes.Count)
        {
            currentBoxIndex = 0;
        }

        UpdateUI();
    }

    private void CyclePrevious()
    {
        if(monsterBoxes.Count == 0)
        {
            return;
        }

        currentBoxIndex--;

        if(currentBoxIndex < 0)
        {
            currentBoxIndex = monsterBoxes.Count - 1;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if(monsterBoxes.Count == 0)
        {
            return;
        }

        MonsterBoxes monsterBox = monsterBoxes[currentBoxIndex];

        boxNameText.text = monsterBox.boxName;
        boxImage.sprite = monsterBox.boxIcon;
    }
}
