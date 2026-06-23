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

    [SerializeField] private Image pityFill;
    [SerializeField] private TMP_Text pityText;

    [SerializeField] private Image exclusivePityFill;
    [SerializeField] private TMP_Text exclusivePityText;

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

        double boxCost = box.cost - CounterManager.Instance.GetBoxCostReduction(box.cost);

        boxCostText.text = $"{boxCost:F0}";

        PopulateProbabilityGrid(box);

        UpdatePityBar(box);

        TutorialManager.Instance?.NotifyTrigger(TutorialTrigger.OnBoxPreviewOpened);

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

    private void UpdatePityBar(MonsterBoxes box)
    {
        int pity = gachaManager.GetPityCount(box);
        int pityThreshold = gachaManager.GetEffectivePityThreshold(box);

        pityFill.fillAmount = Mathf.Clamp01((float)pity / pityThreshold);
        pityText.text = $"{pityThreshold - pity} pulls left to find a guaranteed <color=#FF49D9> Epic+ <color=white> plushie";

        int exclusivePity = gachaManager.GetExclusivePityCount(box);
        int exclusiveThreshold = box.exclusiveMonsterPity;

        exclusivePityFill.fillAmount =
            Mathf.Clamp01((float)exclusivePity / exclusiveThreshold);

        exclusivePityText.text = $"{exclusiveThreshold - exclusivePity} pulls left to find a <color=#FF3D50> Secret <color=white> plushie";
    }

    private void OnConfirmRollPressed()
    {
        BoxesManager.Instance.OpenBoxFromPreview(currentBox);

        UpdatePityBar(currentBox);

        Close();
    }
}
