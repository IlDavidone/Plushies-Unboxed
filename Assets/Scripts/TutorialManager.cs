using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[Serializable]
public class TutorialStep
{
    public string message;
    public RectTransform highlightTarget;
    public TutorialTrigger trigger;
}

public enum TutorialTrigger
{
    TapToContinue,
    OnBoxPreviewOpened,
    OnRollPerformed,
    OnKeepOrSell,
    OnCollectionOpened
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private GameObject tutorialOverlay;
    [SerializeField] private GameObject highlightFrame;
    [SerializeField] private RectTransform highlightRect;
    [SerializeField] private GameObject messageBox;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button continueButton;
    [SerializeField] private List<TutorialStep> steps;

    private int currentStep = 0;
    private bool isTutorialActive = false;
    private bool waitingForTap = false;

    public event Action<TutorialTrigger> OnStepAdvanceTrigger;

    void Awake()
    {
        if(Instance != null)
        {
            return;
        }
        Instance = this;

        tutorialOverlay.SetActive(false);
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("TutorialCompleted"))
            StartTutorial();
    }

    public void StartTutorial()
    {
        isTutorialActive = true;
        currentStep = 0;
        ShowStep(currentStep);
    }

    private void ShowStep(int index)
    {
        if(index >= steps.Count)
        {
            CompleteTutorial();
            return;
        }

        TutorialStep step = steps[index];

        tutorialOverlay.SetActive(true);
        messageText.text = step.message;
        messageBox.SetActive(true);

        if(step.highlightTarget != null)
        {
            highlightFrame.SetActive(true);
            highlightRect.position = step.highlightTarget.position;
            highlightRect.sizeDelta = step.highlightTarget.sizeDelta + new Vector2(20f, 20f);
        }
        else
        {
            highlightFrame.SetActive(false);
        }

        continueButton.gameObject.SetActive(step.trigger == TutorialTrigger.TapToContinue);

        continueButton.onClick.RemoveAllListeners();
        if (step.trigger == TutorialTrigger.TapToContinue)
            continueButton.onClick.AddListener(NextStep);
    }

    public void NotifyTrigger(TutorialTrigger trigger)
    {
        if (!isTutorialActive || currentStep >= steps.Count) return;
        if (steps[currentStep].trigger == trigger)
            NextStep();
    }

    private void NextStep()
    {
        currentStep++;
        ShowStep(currentStep);
    }

    private void CompleteTutorial()
    {
        isTutorialActive = false;
        tutorialOverlay.SetActive(false);
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        PlayerPrefs.Save();
        ToastMessage.Instance.Show("Tutorial complete! Good luck!");
    }

    public bool IsTutorialActive() => isTutorialActive;
}
