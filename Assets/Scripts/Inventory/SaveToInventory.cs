using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using EscapeRoom.Inventory;
using EscapeRoom.LabSecurity;

[RequireComponent(typeof(XRGrabInteractable))]
public class SaveToInventory : MonoBehaviour
{
    private XRGrabInteractable grab;
    private float lastPressTime = -10f;
    private bool collecting;
    private InventoryItem item;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        item = GetComponent<InventoryItem>();
    }

    private void OnEnable()
    {
        grab.activated.AddListener(OnActivated);
        grab.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        grab.activated.RemoveListener(OnActivated);
        grab.selectExited.RemoveListener(OnReleased);
    }

    private void OnActivated(ActivateEventArgs args)
    {
        if (!grab.isSelected || collecting)
            return;

        float now = Time.unscaledTime;

        if (now - lastPressTime <= 0.3f)
        {
            collecting = true;
            StartCoroutine(StoreNextFrame());
        }
        else
        {
            lastPressTime = now;
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        lastPressTime = -10f;
    }

    private IEnumerator StoreNextFrame()
    {
        yield return null;

        InventoryManager.Inventory.AddItem(item.ItemId);
        Destroy(gameObject);
    }
}