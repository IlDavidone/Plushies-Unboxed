using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] private GameObject screenRoot;
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button closeButton;

    [Header("SFX")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxValueText;

    [Header("Music")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicValueText;

    [Header("Master")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private TextMeshProUGUI masterValueText;

    [Header("Toggles")]
    [SerializeField] private Toggle toastToggle;
    [SerializeField] private Toggle tutorialToggle;

    [Header("Misc")]
    [SerializeField] private Button resetProgressButton;
    [SerializeField] private Button confirmResetButton;  // hidden by default, shown after first press
    [SerializeField] private Button cancelResetButton;
    [SerializeField] private GameObject resetConfirmGroup;
    [SerializeField] private TextMeshProUGUI versionText;

    [Header("Animation")]
    [SerializeField] private float animDuration = 0.35f;

    private const string SFX_KEY = "SFXVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string MASTER_KEY = "MasterVolume";
    private const string TOAST_KEY = "ToastEnabled";

    void Awake()
    {
        screenRoot.SetActive(false);
        canvasGroup.alpha = 0f;
        resetConfirmGroup.SetActive(false);

        closeButton.onClick.AddListener(Close);
        resetProgressButton.onClick.AddListener(OnResetPressed);
        confirmResetButton.onClick.AddListener(OnConfirmReset);
        cancelResetButton.onClick.AddListener(OnCancelReset);

        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        masterSlider.onValueChanged.AddListener(OnMasterChanged);
        toastToggle.onValueChanged.AddListener(OnToastChanged);
        tutorialToggle.onValueChanged.AddListener(OnTutorialToggleChanged);

        versionText.text = $"Build Version: v{Application.version}";
    }

    public void Open()
    {
        LoadSettings();
        screenRoot.SetActive(true);

        panelRect.localScale = Vector3.one * 0.85f;
        canvasGroup.alpha = 0f;

        panelRect.DOScale(1f, animDuration).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1f, animDuration);
    }

    public void Close()
    {
        panelRect.DOScale(0.85f, animDuration * 0.7f).SetEase(Ease.InBack);
        canvasGroup.DOFade(0f, animDuration * 0.7f).OnComplete(() =>
        {
            screenRoot.SetActive(false);
            resetConfirmGroup.SetActive(false);
        });
    }

    private void LoadSettings()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_KEY, 0.8f);
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_KEY, 0.6f);
        masterSlider.value = PlayerPrefs.GetFloat(MASTER_KEY, 1f);

        UpdateSliderText(sfxValueText, sfxSlider.value);
        UpdateSliderText(musicValueText, musicSlider.value);
        UpdateSliderText(masterValueText, masterSlider.value);

        // apply loaded values immediately
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
        AudioManager.Instance.SetMasterVolume(masterSlider.value);

        toastToggle.isOn = PlayerPrefs.GetInt(TOAST_KEY, 1) == 1;
        tutorialToggle.isOn = !PlayerPrefs.HasKey("TutorialComplete");
    }
    private void OnSFXChanged(float value)
    {
        UpdateSliderText(sfxValueText, value);
        PlayerPrefs.SetFloat(SFX_KEY, value);
        AudioManager.Instance.SetSFXVolume(value);
    }

    private void OnMusicChanged(float value)
    {
        UpdateSliderText(musicValueText, value);
        PlayerPrefs.SetFloat(MUSIC_KEY, value);
        AudioManager.Instance.SetMusicVolume(value);
    }

    private void OnMasterChanged(float value)
    {
        UpdateSliderText(masterValueText, value);
        PlayerPrefs.SetFloat(MASTER_KEY, value);
        AudioManager.Instance.SetMasterVolume(value);
    }

    private void UpdateSliderText(TextMeshProUGUI text, float value)
    {
        text.text = $"{Mathf.RoundToInt(value * 100)}%";
    }

    private void OnToastChanged(bool value)
    {
        PlayerPrefs.SetInt(TOAST_KEY, value ? 1 : 0);
        ToastMessage.Instance?.SetEnabled(value);
    }

    private void OnTutorialToggleChanged(bool value)
    {
        if (value)
            PlayerPrefs.DeleteKey("TutorialComplete");
        else
        {
            PlayerPrefs.SetInt("TutorialComplete", 1);
            PlayerPrefs.Save();
        }
    }

    private void OnResetPressed()
    {
        // show confirm/cancel instead of doing it immediately
        resetConfirmGroup.SetActive(true);
        resetConfirmGroup.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, vibrato: 1);
    }

    private void OnConfirmReset()
    {
        // wipe save
        #if UNITY_WEBGL
        PlayerPrefs.DeleteKey("SaveData");
        #else
        string savePath = Application.persistentDataPath + "/save.json";
        if (System.IO.File.Exists(savePath))
            System.IO.File.Delete(savePath);
        #endif

        PlayerPrefs.DeleteKey("TutorialComplete");
        PlayerPrefs.Save();

        ToastMessage.Instance?.Show("Progress reset! Reloading...");

        DOVirtual.DelayedCall(1.5f, () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        });
    }

    private void OnCancelReset()
    {
        resetConfirmGroup.SetActive(false);
    }
}