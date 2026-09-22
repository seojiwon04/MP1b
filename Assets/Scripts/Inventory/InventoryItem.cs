using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private string itemId;

    public string ItemId => itemId;
}
