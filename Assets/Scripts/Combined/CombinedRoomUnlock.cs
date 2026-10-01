using UnityEngine;
using System.Collections;
using TMPro;

public class CombinedRoomUnlock : MonoBehaviour
{
    public int keysNeeded = 4; // number of keys needed to unlock the final part
    public GameObject door; // door
    private float doorOpenAngle = 90f;
    private float openDuration = 1f;

    int keysPlaced = 0; // also the order index of the next key expected
    bool opened = false;

    public bool debugMode = false;

    // Returns true if this key was placed in the correct order
    public bool TryPlace(int order)
    {
        if (opened) return false;

        if (order != keysPlaced)
        {
            Debug.Log("Final lock: wrong order (expected " + keysPlaced + ", got " + order + ")");
            return false;
        }

        keysPlaced++;
        Debug.Log("Final lock: " + keysPlaced + "/" + keysNeeded);

        if (keysPlaced >= keysNeeded) // all keys placed in order, open the final door
        {
            opened = true;
            StartCoroutine(openAll());
        }
        return true;
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
        Quaternion doorStart = door.transform.localRotation;
        Quaternion doorTarget = doorStart * Quaternion.Euler(0f, doorOpenAngle, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / openDuration;
            float s = Mathf.SmoothStep(0f, 1f, t);
            door.transform.localRotation = Quaternion.Slerp(doorStart, doorTarget, s);
            yield return null;
        }
    }

    private void Update()
    {
        if (debugMode) DEBUG_OPEN();
    }
}