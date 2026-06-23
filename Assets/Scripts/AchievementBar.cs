using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementBar : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image statusIcon;

    [SerializeField] private Sprite lockSprite;
    [SerializeField] private Sprite checkSprite;

    [Header("Colors")]
    [SerializeField] private Color unlockedColor = new Color(0.9f, 0.75f, 0.3f); // gold
    [SerializeField] private Color lockedColor = new Color(0.3f, 0.3f, 0.3f);    // dark gray

    public void Setup(Achievement achievement, bool isUnlocked)
    {
        titleText.text = achievement.title;
        descriptionText.text = achievement.description;

        if (isUnlocked)
        {
            iconImage.sprite = achievement.icon;
            iconImage.color = Color.white;
            background.color = unlockedColor;
        }
        else
        {
            iconImage.sprite = achievement.icon;
            iconImage.color = new Color(0f, 0f, 0f, 0.5f);
            background.color = lockedColor;
            titleText.text = "???";
            descriptionText.text = "Keep playing to unlock!";
        }
    }
}
