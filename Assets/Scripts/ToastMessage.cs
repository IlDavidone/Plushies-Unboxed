using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastMessage : MonoBehaviour
{
    public static ToastMessage Instance;

    [SerializeField] private RectTransform toastRect;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Settings")]
    [SerializeField] private float slideInDuration = 0.3f;
    [SerializeField] private float displayDuration = 2.5f;
    [SerializeField] private float slideOutDuration = 0.2f;
    [SerializeField] private float offScreenY = -200f;
    [SerializeField] private float onScreenY = 100f;   // final resting Y position (from bottom)

    private Coroutine activeToast;

    void Awake()
    {
        Instance = this;
        toastRect.anchoredPosition = new Vector2(0f, offScreenY);
        canvasGroup.alpha = 0f;
    }

    public void Show(string message, Sprite iconSprite = null)
    {
        if (activeToast != null)
            StopCoroutine(activeToast);

        activeToast = StartCoroutine(ToastRoutine(message, iconSprite));
    }

    private IEnumerator ToastRoutine(string message, Sprite iconSprite)
    {
        messageText.text = message;
        icon.sprite = iconSprite;
        icon.gameObject.SetActive(iconSprite != null);

        toastRect.DOKill();
        canvasGroup.DOKill();

        toastRect.anchoredPosition = new Vector2(0f, offScreenY);
        canvasGroup.alpha = 0f;

        toastRect.DOAnchorPosY(onScreenY, slideInDuration).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1f, slideInDuration);

        yield return new WaitForSeconds(slideInDuration + displayDuration);

        toastRect.DOAnchorPosY(offScreenY, slideOutDuration).SetEase(Ease.InBack);
        canvasGroup.DOFade(0f, slideOutDuration);

        yield return new WaitForSeconds(slideOutDuration);

        activeToast = null;
    }
}
