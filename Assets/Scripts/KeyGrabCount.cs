using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; 

public class KeyGrabCount : MonoBehaviour
{
    public KeyCounter counter;

    bool counted = false;
    XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        if (counter == null) counter = FindFirstObjectByType<KeyCounter>();
    }

    void OnEnable() => grab.selectEntered.AddListener(OnGrabbed);
    void OnDisable() => grab.selectEntered.RemoveListener(OnGrabbed);

    void OnGrabbed(SelectEnterEventArgs args)
    {
        if (counted) return;
        counted = true;
        counter.keysFound++;
    }
}