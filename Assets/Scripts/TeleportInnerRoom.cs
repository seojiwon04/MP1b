using UnityEngine;
using Unity.XR.CoreUtils;
using System.Collections;

public class TeleportInnerRoom : MonoBehaviour
{
    public Transform destination; // empty object placed where the player should end up
    public bool faceDestinationDirection = true; // turn the player to face the destination's blue arrow
    public GameObject door;
    private float doorOpenAngle = 90f;
    private float openDuration = 1f;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(name + " trigger hit by " + other.name);
        XROrigin origin = other.GetComponentInParent<XROrigin>();
        if (origin == null) return; // not the player

        CharacterController cc = origin.GetComponent<CharacterController>();
        if (cc) cc.enabled = false; // otherwise it snaps the player back

        if (faceDestinationDirection)
            origin.MatchOriginUpCameraForward(Vector3.up, destination.forward);

        // put the player's head above the destination, keeping their current eye height
        origin.MoveCameraToWorldLocation(destination.position + Vector3.up * origin.CameraInOriginSpaceHeight);

        if (cc) cc.enabled = true;
        StartCoroutine(openDoor());
    }

    IEnumerator openDoor()
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
}