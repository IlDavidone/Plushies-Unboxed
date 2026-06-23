using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MessageBoxPosition
{
    Bottom,
    Top,
    Left,
    Right,
    Center
}

public enum TutorialTrigger
{
    TapToContinue,
    OnBoxPreviewOpened,
    OnRollPerformed,
    OnKeepOrSell,
    OnCollectionOpened
}

[Serializable]
public class TutorialStep
{
    public string message;
    public RectTransform highlightTarget;
    public TutorialTrigger trigger;
    public MessageBoxPosition messagePosition = MessageBoxPosition.Bottom;

    // Optional override for special cases (like final step)
    public bool forceCenterMessageBox = false;
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private GameObject tutorialOverlay;
    [SerializeField] private GameObject highlightFrame;
    [SerializeField] private RectTransform highlightRect;
    [SerializeField] private GameObject messageBox;
    [SerializeField] private RectTransform messageBoxRect;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button continueButton;
    [SerializeField] private List<TutorialStep> steps;

    [Header("Animation")]
    [SerializeField] private float messageBoxMoveDuration = 0.3f;
    [SerializeField] private float messageBoxOffset = 20f;
    [SerializeField] private CanvasGroup messageBoxCanvasGroup;

    private int currentStep = 0;
    private bool tutorialActive = false;

    private void Awake()
    {
        Instance = this;
        tutorialOverlay.SetActive(false);
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("TutorialComplete"))
            StartTutorial();
    }

    public void StartTutorial()
    {
        tutorialActive = true;
        currentStep = 0;
        ShowStep(currentStep);
    }

    private void ShowStep(int index)
    {
        if (index >= steps.Count)
        {
            CompleteTutorial();
            return;
        }

        TutorialStep step = steps[index];

        tutorialOverlay.SetActive(true);

        bool isLastStep = index == steps.Count - 1;

        messageBoxCanvasGroup.DOKill();

        messageBoxCanvasGroup.DOFade(0f, 0.15f).OnComplete(() =>
        {
            messageText.text = step.message;

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(messageBoxRect);

            if (step.highlightTarget != null)
            {
                highlightFrame.SetActive(true);

                highlightRect.DOKill();

                highlightRect.DOMove(
                    step.highlightTarget.position,
                    messageBoxMoveDuration)
                    .SetEase(Ease.OutBack);

                Vector2 targetSize = GetWorldUISize(step.highlightTarget) + new Vector2(20f, 20f);

                highlightRect.DOSizeDelta(
                    targetSize,
                    messageBoxMoveDuration);

                if (isLastStep || step.forceCenterMessageBox)
                {
                    //prevent last step drift
                    messageBoxRect
                        .DOAnchorPos(new Vector2(0, -400), messageBoxMoveDuration)
                        .SetEase(Ease.OutBack);
                }
                else
                {
                    PositionMessageBox(step.highlightTarget, step.messagePosition);
                }
            }
            else
            {
                highlightFrame.SetActive(false);
                SetMessageBoxAnchor(step.messagePosition);
            }

            continueButton.gameObject.SetActive(
                step.trigger == TutorialTrigger.TapToContinue);

            continueButton.onClick.RemoveAllListeners();

            if (step.trigger == TutorialTrigger.TapToContinue)
                continueButton.onClick.AddListener(NextStep);

            messageBoxCanvasGroup.DOFade(1f, 0.2f);

            messageBoxRect.DOPunchScale(
                Vector3.one * 0.05f,
                0.3f,
                1);
        });
    }

    private Vector2 GetWorldUISize(RectTransform target)
    {
        Vector3[] worldCorners = new Vector3[4];
        target.GetWorldCorners(worldCorners);

        Vector2 bl = RectTransformUtility.WorldToScreenPoint(null, worldCorners[0]);
        Vector2 tr = RectTransformUtility.WorldToScreenPoint(null, worldCorners[2]);

        return new Vector2(
            Mathf.Abs(tr.x - bl.x),
            Mathf.Abs(tr.y - bl.y)
        );
    }

    private void PositionMessageBox(RectTransform target, MessageBoxPosition position)
    {
        messageBoxRect.DOKill();

        Vector2 anchoredPos = CalculateAnchoredPosition(target, position);

        messageBoxRect
            .DOAnchorPos(anchoredPos, messageBoxMoveDuration)
            .SetEase(Ease.OutBack);
    }

    private Vector2 CalculateAnchoredPosition(RectTransform target, MessageBoxPosition position)
    {
        RectTransform parent = messageBoxRect.parent as RectTransform;

        Vector2 targetLocalPos = parent.InverseTransformPoint(target.position);

        Vector2 offset = Vector2.zero;

        float targetHalfW = target.rect.width * 0.5f;
        float targetHalfH = target.rect.height * 0.5f;

        float boxHalfW = messageBoxRect.rect.width * 0.5f;
        float boxHalfH = messageBoxRect.rect.height * 0.5f;

        switch (position)
        {
            case MessageBoxPosition.Bottom:
                offset = new Vector2(0, -(targetHalfH + boxHalfH + messageBoxOffset));
                break;

            case MessageBoxPosition.Top:
                offset = new Vector2(0, (targetHalfH + boxHalfH + messageBoxOffset));
                break;

            case MessageBoxPosition.Left:
                offset = new Vector2(-(targetHalfW + boxHalfW + messageBoxOffset), 0);
                break;

            case MessageBoxPosition.Right:
                offset = new Vector2((targetHalfW + boxHalfW + messageBoxOffset), 0);
                break;

            case MessageBoxPosition.Center:
                offset = Vector2.zero;
                break;
        }

        return targetLocalPos + offset;
    }

    private void SetMessageBoxAnchor(MessageBoxPosition position)
    {
        messageBoxRect.DOKill();

        Vector2 targetPos;
        RectTransform parent = (RectTransform)messageBoxRect.parent;
        Vector2 size = parent.rect.size;

        switch (position)
        {
            case MessageBoxPosition.Bottom:
                targetPos = new Vector2(0f, -size.y * 0.35f);
                break;

            case MessageBoxPosition.Top:
                targetPos = new Vector2(0f, size.y * 0.35f);
                break;

            case MessageBoxPosition.Left:
                targetPos = new Vector2(-size.x * 0.25f, 0f);
                break;

            case MessageBoxPosition.Right:
                targetPos = new Vector2(size.x * 0.25f, 0f);
                break;

            default:
                targetPos = Vector2.zero;
                break;
        }

        messageBoxRect
            .DOAnchorPos(targetPos, messageBoxMoveDuration)
            .SetEase(Ease.OutBack);
    }

    public void NotifyTrigger(TutorialTrigger trigger)
    {
        if (!tutorialActive || currentStep >= steps.Count)
            return;

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
        tutorialActive = false;

        messageBoxCanvasGroup
            .DOFade(0f, 0.3f)
            .OnComplete(() =>
            {
                tutorialOverlay.SetActive(false);
            });

        PlayerPrefs.SetInt("TutorialComplete", 1);
        PlayerPrefs.Save();

        ToastMessage.Instance.Show("Tutorial complete! Good luck!");
    }

    public bool IsTutorialActive()
    {
        return tutorialActive;
    }
}