using UnityEngine;
using EscapeRoom.Inventory;
using Unity.XR.CoreUtils;

public class ClearTestIteem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<XROrigin>() != null)
        {
            if (InventoryManager.Inventory.HasItem("TEST_ITEM"))
                InventoryManager.Inventory.RemoveItem("TEST_ITEM");
        }
    }
}