using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private ShelfView shelfView;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveManager.Load();
        shelfView.RefreshView();
        CounterManager.Instance.RefreshView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
