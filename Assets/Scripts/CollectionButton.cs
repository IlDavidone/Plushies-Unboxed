using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CollectionButton : MonoBehaviour
{
    [SerializeField] private Image monsterIcon;
    [SerializeField] private RectTransform monsterIconRect;
    [SerializeField] private RectTransform containerRect;
    [SerializeField] private float slideDuration = 0.3f;
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float slideDistance = 200f; // how far it travels off-screen

    private Vector2 originalPosition;

    void Start()
    {
        float slideDistance = containerRect.rect.width * 2;
        originalPosition = monsterIconRect.anchoredPosition;
        StartCoroutine(CycleMonsters());
    }

    private IEnumerator CycleMonsters()
    {
        while (true)
        {
            var owned = CollectionManager.Instance.ownedMonsters;
            if (owned.Count == 0)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            int randomIndex = Random.Range(0, owned.Count);
            OwnedMonster randomMonster = owned[randomIndex];
            Monsters desiredMonster = CollectionManager.Instance.allMonsterData
                .Find(m => m.monsterName == randomMonster.monsterName);

            if (desiredMonster != null)
            {
                Sprite newSprite = randomMonster.isShiny ? desiredMonster.shinyIcon : desiredMonster.baseIcon;
                yield return SlideSwap(newSprite);
            }

            yield return new WaitForSeconds(displayDuration);
        }
    }

    private IEnumerator SlideSwap(Sprite newSprite)
    {
        // slide current sprite out to the left
        yield return SlideTo(originalPosition + Vector2.left * slideDistance, slideDuration);

        // swap sprite while off-screen, snap to right side (off-screen on the other side)
        monsterIcon.sprite = newSprite;
        monsterIconRect.anchoredPosition = originalPosition + Vector2.right * slideDistance;

        // slide new sprite in to the original position
        yield return SlideTo(originalPosition, slideDuration);
    }

    private IEnumerator SlideTo(Vector2 targetPosition, float duration)
    {
        Vector2 startPosition = monsterIconRect.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration); // eased, not linear
            monsterIconRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        monsterIconRect.anchoredPosition = targetPosition;
    }
}