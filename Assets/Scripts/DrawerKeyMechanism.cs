using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // XRI 2.x: use UnityEngine.XR.Interaction.Toolkit instead

public class DrawerKeyMechanism : MonoBehaviour
{
    public GameObject drawer; // the drawer object itself
    public GameObject box; // the box object that contains the key
    public GameObject hiddenKey; // the hidden key object that will be revealed when the box is opened
    public GameObject openKey; // key object that opens the box

    private Vector3 openOffset = new Vector3(0f, 0f, -2.4f);
    private float openDuration = 1f;
    private string drawerTag;

    public string keyTag;

    bool opened = false;

    void OnTriggerStay(Collider other)
    {
        Debug.Log(name + " touched by " + other.name + " (rb: " + (other.attachedRigidbody ? other.attachedRigidbody.name + ", tag " + other.attachedRigidbody.tag : "none") + "), looking for " + keyTag);
        if (opened || keyTag == null || !other.CompareTag(keyTag)) return;
        Debug.Log("Hi");
        if (openKey.GetComponent<XRGrabInteractable>().isSelected) return;
        opened = true;
        Debug.Log("DrawerKeyMechanism: " + other.name + " has opened the box!");
        openKey.GetComponent<Rigidbody>().isKinematic = true;
        openKey.GetComponent<XRGrabInteractable>().enabled = false;
        StartCoroutine(slideOpen());
    }

    IEnumerator slideOpen()
    {
        Vector3 boxStart = box.transform.localPosition;
        Vector3 boxTarget = boxStart + openOffset;
        Vector3 openKeyStart = openKey.transform.localPosition;
        Vector3 openKeyTarget = openKeyStart + openOffset;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / openDuration;
            box.transform.localPosition = Vector3.Lerp(boxStart, boxTarget, Mathf.SmoothStep(0f, 1f, t));
            openKey.transform.localPosition = Vector3.Lerp(openKeyStart, openKeyTarget, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        hiddenKey.GetComponent<Rigidbody>().isKinematic = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hiddenKey.GetComponent<Rigidbody>().isKinematic = true;

        drawerTag = drawer.tag;
        if (drawerTag.Equals("Pen Drawer"))
        {
            keyTag = "Pen Key";
        }
        else if (drawerTag.Equals("Scissors Drawer"))
        {
            keyTag = "Scissors Key";
        }
        else if (drawerTag.Equals("Pencil Drawer"))
        {
            keyTag = "Pencil Key";
        }
        else if (drawerTag.Equals("Sticky Note Drawer"))
        {
            keyTag = "Sticky Note Key";
        }

        Debug.Log("DrawerKeyMechanism: drawerTag = " + drawerTag + ", keyTag = " + keyTag);
    }

    // Update is called once per frame
    void Update()
    {

    }
}