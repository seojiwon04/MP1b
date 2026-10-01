using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OrderedDeskKeySpot : MonoBehaviour
{
    public GameObject key;
    public CombinedRoomUnlock manager;
    public int order;

    bool placed = false;

    void OnTriggerStay(Collider other)
    {
        if (placed) return;
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null || rb.gameObject != key) return;

        XRGrabInteractable grab = key.GetComponent<XRGrabInteractable>();
        if (grab && grab.isSelected) return; 

        if (!manager.TryPlace(order)) return;

        placed = true;
        key.GetComponent<Rigidbody>().isKinematic = true;
        if (grab) grab.enabled = false;

        Debug.Log(name + ": " + key.name + " placed (step " + order + ")");
    }
}