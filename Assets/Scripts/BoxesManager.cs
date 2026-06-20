using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxesManager : MonoBehaviour
{
    public static BoxesManager Instance;

    public List<MonsterBoxes> monsterBoxes = new List<MonsterBoxes>();
    [SerializeField] private RarityConfig[] rarityConfigs;
    
    [SerializeField] private GachaManager gachaManager;

    [SerializeField] private TMP_Text boxNameText;  
    [SerializeField] private RectTransform boxImageRect;
    [SerializeField] private Image boxImage;
    [SerializeField] private Button boxButton;
    [SerializeField] private RevealController revealController;
    [SerializeField] private Button leftArrowButton;
    [SerializeField] private Button rightArrowButton;
    [SerializeField] private float slideDuration = 0.5f;

    private int currentBoxIndex = 0;
    private Vector2 originalPosition;
    private bool isTransitioning = false;

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
        boxButton.onClick.RemoveAllListeners();
        
        leftArrowButton.onClick.AddListener(CyclePrevious);
        rightArrowButton.onClick.AddListener(CycleNext);
        boxButton.onClick.AddListener(OpenBox);

        originalPosition = boxImageRect.anchoredPosition;

        UpdateUI(instant: true);
    }
    
    private void CycleNext()
    {
        if (isTransitioning) return;
        currentBoxIndex = (currentBoxIndex + 1) % monsterBoxes.Count;
        StartCoroutine(SlideSwap(direction: -1));
    }

    private void CyclePrevious()
    {
        if (isTransitioning) return;
        currentBoxIndex = (currentBoxIndex - 1 + monsterBoxes.Count) % monsterBoxes.Count;
        StartCoroutine(SlideSwap(direction: 1));
    }

    private IEnumerator SlideSwap(int direction)
    {
        isTransitioning = true;

        float slideDistance = boxImageRect.rect.width;
        Vector2 outTarget = originalPosition + Vector2.right * direction * slideDistance;
        Vector2 inStart = originalPosition - Vector2.right * direction * slideDistance;

        yield return SlideTo(boxImageRect, outTarget, slideDuration);

        UpdateUI(instant: true);
        boxImageRect.anchoredPosition = inStart;

        yield return SlideTo(boxImageRect, originalPosition, slideDuration);

        isTransitioning = false;
    }

    private IEnumerator SlideTo(RectTransform rect, Vector2 target, float duration)
    {
        Vector2 start = rect.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            rect.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }

        rect.anchoredPosition = target;
    }

    private void UpdateUI(bool instant)
    {
        if(monsterBoxes.Count == 0)
        {
            return;
        }

        MonsterBoxes monsterBox = monsterBoxes[currentBoxIndex];

        boxNameText.text = monsterBox.boxName;
        boxImage.sprite = monsterBox.boxIcon;
    }

    private void OpenBox()
    {
        if(!CurrencyManager.Instance.TrySpend(monsterBoxes[currentBoxIndex].cost)) return;

        AchivementManager.Instance.TryUnlock("first_box");

        var result = gachaManager.RollMonster(monsterBoxes[currentBoxIndex]);

        RarityConfig config = Array.Find(rarityConfigs, r => r.rarity == result.monster.rarity);
    
        revealController.PlayReveal(result.monster, result.isShiny, config);
    }
}
