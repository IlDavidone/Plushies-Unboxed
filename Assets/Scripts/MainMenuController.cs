using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button quitButton;

    private string SavePath => Application.persistentDataPath + "/save.json";

    void Start()
    {
        playButton.onClick.AddListener(OnPlayPressed);

        if (continueButton != null)
        {
            bool hasSave = File.Exists(SavePath);
            continueButton.gameObject.SetActive(hasSave);
            continueButton.onClick.AddListener(OnContinuePressed);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitPressed);
            #if UNITY_WEBGL
            quitButton.gameObject.SetActive(false);
            #endif
        }
    }

    private void OnPlayPressed()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save file deleted, starting fresh.");
        }

        SceneManager.LoadScene("MainScene");
    }

    private void OnContinuePressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnQuitPressed()
    {
        Application.Quit();
    }
}