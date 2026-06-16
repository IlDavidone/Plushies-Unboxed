using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveManager.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
