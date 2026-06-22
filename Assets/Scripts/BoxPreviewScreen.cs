using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxPreviewScreen : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject screenRoot;
    [SerializeField] private Image boxImage;
    [SerializeField] private TextMeshProUGUI boxNameText;
    [SerializeField] private TextMeshProUGUI boxCostText;
    [SerializeField] private Button closingButton;
    [SerializeField] private Button openButton;

    [Space]
    [SerializeField] private GachaManager gachaManager;

    [Header("Probability Grid")]
    [SerializeField] Transform gridContainer;
    [SerializeField] ProbabilityCell cellPrefab;

    private MonsterBoxes currentBox;

    void Awake()
    {
        screenRoot.SetActive(false);
        closingButton.onClick.AddListener(Close);
        openButton.onClick.AddListener(OnConfirmRollPressed);
    }

    public void Open(MonsterBoxes box)
    {
        currentBox = box;

        boxImage.sprite = box.boxIcon;
        boxNameText.text = box.boxName;
        boxCostText.text = $"{box.cost:F0}";

        PopulateProbabilityGrid(box);

        screenRoot.SetActive(true);
    }

    public void Close()
    {
        screenRoot.SetActive(false);
    }

    private void PopulateProbabilityGrid(MonsterBoxes box)
    {
        foreach(Transform child in gridContainer)
            Destroy(child.gameObject);

        float totalWeight = 0f;
        foreach(var m in box.monstersPool)
            totalWeight += m.rollWeight;

        var sortedMonsters = box.monstersPool.OrderByDescending(m => m.rollWeight);

        foreach(var m in sortedMonsters)
        {
            float chancePercentual = (m.rollWeight / totalWeight) * 100f;
            ProbabilityCell cell = Instantiate(cellPrefab, gridContainer);
            cell.Setup(m, chancePercentual);
        }
    }

    private void OnConfirmRollPressed()
    {
        BoxesManager.Instance.OpenBoxFromPreview(currentBox);
        Close();
    }
}
