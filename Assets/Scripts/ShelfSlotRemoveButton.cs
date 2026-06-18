using UnityEngine;
using UnityEngine.UI;

public class ShelfSlotRemoveButton : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private ShelfView shelfView;
    [SerializeField] private Button removeButton;

    void Awake()
    {
        removeButton.onClick.AddListener(OnRemovePressed);
    }

    private void OnRemovePressed()
    {
        ShelfManager.Instance.RemoveFromSlot(slotIndex);
        shelfView.RefreshView();
    }
}
