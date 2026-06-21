using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementNotification : MonoBehaviour
{
    [SerializeField] private GameObject notificationRoot;
    [SerializeField] private Image achievementIcon;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private float notificationDuration = 4f;

    void Start()
    {
        notificationRoot.SetActive(false);
        AchivementManager.Instance.OnAchievementUnlock += ShowNotification;
    }

    private void ShowNotification(Achievement achievement)
    {
        StopAllCoroutines();
        StartCoroutine(NotificationRoutine(achievement));
    }

    private IEnumerator NotificationRoutine(Achievement achievement)
    {
        Debug.LogError("Arrived here");
        achievementIcon.sprite = achievement.icon;
        titleText.text = achievement.title;
        descriptionText.text = achievement.description;

        notificationRoot.SetActive(true);
        AudioManager.Instance.PlayRandomSqueakySound();

        yield return new WaitForSeconds(notificationDuration);

        notificationRoot.SetActive(false);
    }
}
