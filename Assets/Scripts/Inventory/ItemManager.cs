using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EscapeRoom.LabSecurity;
using EscapeRoom.Inventory;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new();
    [SerializeField] private Transform spawnPoint;
    public InputActionReference takeOutAction;
    private List<string> items = new();

    void TakeOutItem()
    {
        items = InventoryManager.Inventory.items;
        if (items.Count <= 0) return;
        InventoryManager.Inventory.TakeOutItem(items[0]);
    }

    void Start()
    {
        takeOutAction.action.Enable();
        takeOutAction.action.performed += Performed;
    }

    private void Performed(InputAction.CallbackContext context)
    {
        TakeOutItem();
    }
}
