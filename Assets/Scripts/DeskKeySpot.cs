using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables; 

public class DeskKeySpot : MonoBehaviour
{
    public GameObject key; // the key for desk
    public FinalUnlockManager manager; 

    bool placed = false;

    void OnTriggerStay(Collider other)
    {
        if (placed || !other.attachedRigidbody.CompareTag(key.tag)) return;
        Debug.Log(name + ": " + key.name + " is in the spot");

        XRGrabInteractable grab = key.GetComponent<XRGrabInteractable>();
        if (grab && grab.isSelected) return; // if still being grabbed, return

        placed = true;
        key.GetComponent<Rigidbody>().isKinematic = true;
        if (grab) grab.enabled = false; // disable grabbing one placed

        Debug.Log(name + ": " + key.name + " placed");
        manager.KeyPlaced();
    }
}