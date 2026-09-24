using UnityEngine;
using System.Collections;
using TMPro;

public class FinalUnlockManager : MonoBehaviour
{
    public int keysNeeded = 4; // number of keys needed to unlock the final part
    public GameObject finalBox; // box
    private Vector3 boxOpenOffset = new Vector3(0f, 0f, -2.4f);
    public GameObject door; // door
    public GameObject finalKey; // final star
    private float doorOpenAngle = 90f;
    private float openDuration = 1f;

    int keysPlaced = 0;
    bool opened = false;

    public int locksRemaining = 0;
    public TextMeshProUGUI locksRemainingText;

    public bool debugMode = false; 

    public void KeyPlaced()
    {
        keysPlaced++;
        Debug.Log("Final lock: " + keysPlaced + "/" + keysNeeded);

        if (!opened && keysPlaced >= keysNeeded) // if not opened and all keys placed, open the final box and door
        {
            opened = true;
            StartCoroutine(openAll());
        }
    }

    public void DEBUG_OPEN()
    {
        if (debugMode && !opened)
        {
            opened = true;
            StartCoroutine(openAll());
        }
    }

    IEnumerator openAll()
    {
        Vector3 boxStart = finalBox.transform.localPosition;
        Vector3 boxTarget = boxStart + boxOpenOffset;
        Quaternion doorStart = door.transform.localRotation;
        Quaternion doorTarget = doorStart * Quaternion.Euler(0f, doorOpenAngle, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / openDuration;
            float s = Mathf.SmoothStep(0f, 1f, t);
            finalBox.transform.localPosition = Vector3.Lerp(boxStart, boxTarget, s);
            door.transform.localRotation = Quaternion.Slerp(doorStart, doorTarget, s);
            yield return null;
        }
        finalKey.GetComponent<Rigidbody>().isKinematic = false;
        locksRemaining++;
        locksRemainingText.text = "Locks\n\n" + locksRemaining + "/5";
    }

    private void Update()
    {
        if (debugMode) DEBUG_OPEN();
    }
}