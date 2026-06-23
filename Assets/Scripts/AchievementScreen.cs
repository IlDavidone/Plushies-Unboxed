using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementScreen : MonoBehaviour
{
    [SerializeField] private GameObject screenRoot;
    [SerializeField] private Transform listContainer;
    [SerializeField] private AchievementBar barPrefab;
    [SerializeField] private Button closeButton;
    [SerializeField] private List<Achievement> allAchievements;

    void Awake()
    {
        screenRoot.SetActive(false);
        closeButton.onClick.AddListener(Close);
    }

    public void Open()
    {
        PopulateList();
        screenRoot.SetActive(true);
    }

    public void Close()
    {
        screenRoot.SetActive(false);
    }

    private void PopulateList()
    {
        foreach (Transform child in listContainer)
            Destroy(child.gameObject);

        List<Achievement> unlocked = new List<Achievement>();
        List<Achievement> locked = new List<Achievement>();

        foreach (var achievement in allAchievements)
        {
            if (AchivementManager.Instance.IsUnlocked(achievement.achievementId))
                unlocked.Add(achievement);
            else
                locked.Add(achievement);
        }

        foreach (var a in unlocked)
            SpawnBar(a, true);

        foreach (var a in locked)
            SpawnBar(a, false);
    }

    private void SpawnBar(Achievement achievement, bool isUnlocked)
    {
        AchievementBar bar = Instantiate(barPrefab, listContainer);
        bar.Setup(achievement, isUnlocked);
    }
}
