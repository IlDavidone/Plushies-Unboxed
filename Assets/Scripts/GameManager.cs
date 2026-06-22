using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private ShelfView shelfView;

    [SerializeField] private float autosaveInterval = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveManager.Load();
        shelfView.RefreshView();
        CounterManager.Instance.RefreshView();

        StartCoroutine(AutosaveRoutine());
    }

    private IEnumerator AutosaveRoutine()
    {
        while (true)
        {
            saveManager.Save();

            yield return new WaitForSeconds(autosaveInterval);
        }
    }
}
