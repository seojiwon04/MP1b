using System.Collections;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace EscapeRoom.LabSecurity
{
    public class VRPodAccessDoor : MonoBehaviour
    {
        [SerializeField] private string requiredAccessId = "COMPUTER_LAB";
        [Min(.1f)] public float openingDuration = 1.25f;
        [Min(.1f)] public float slideDistance = .95f;
        public bool curvedSliding;
        [Range(45f, 90f)] public float openingAngle = 48f;
        public Transform leftDoor;
        public Transform rightDoor;
        public Renderer[] glassPanels;
        public Material openGlassMaterial;
        public Collider entranceBlocker;
        public Transform interiorInteractionRoot;
        public TMP_Text readerStatus;
        public AudioSource audioSource;
        public AudioClip acceptedSound;
        public bool IsOpen { get; private set; }
        public bool IsOpening { get; private set; }

        private Vector3 leftClosed, rightClosed;
        private Quaternion leftClosedRotation, rightClosedRotation;
        private Coroutine opening;
        private Selectable[] uiControls;
        private bool[] uiStates;
        private XRBaseInteractable[] xrControls;
        private bool[] xrStates;
        private CanvasGroup interiorCanvasGroup;

        private void Awake()
        {
            leftClosed = leftDoor.localPosition;
            rightClosed = rightDoor.localPosition;
            leftClosedRotation = leftDoor.localRotation;
            rightClosedRotation = rightDoor.localRotation;
            uiControls = interiorInteractionRoot.GetComponentsInChildren<Selectable>(true);
            uiStates = uiControls.Select(c => c.interactable).ToArray();
            xrControls = interiorInteractionRoot.GetComponentsInChildren<XRBaseInteractable>(true);
            xrStates = xrControls.Select(c => c.enabled).ToArray();
            interiorCanvasGroup = interiorInteractionRoot.GetComponent<CanvasGroup>();
            SetInterior(false);
            entranceBlocker.enabled = true;
            SetStatus("TAP KEYCARD", new Color(1f,.5f,.5f));
        }

        public bool TryOpenWithCard(LabAccessCard card)
        {
            if (card == null || !card.isActiveAndEnabled || card.IsLocked || card.AccessId != requiredAccessId) return false;
            return BeginOpening();
        }

        private bool BeginOpening()
        {
            if (!isActiveAndEnabled || IsOpen || IsOpening) return false;
            IsOpening = true;
            SetStatus("ACCESS GRANTED", new Color(.3f,1f,.7f));
            
            foreach (Renderer panel in glassPanels)
                panel.sharedMaterial = openGlassMaterial;
            audioSource.PlayOneShot(acceptedSound);
            opening = StartCoroutine(SlideOpen());
            return true;
        }

        private IEnumerator SlideOpen()
        {
            float elapsed = 0f;
            while (elapsed < openingDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / openingDuration));
                SetDoorProgress(t);
                yield return null;
            }
            CompleteOpen();
        }

        private void SetDoorProgress(float t)
        {
            if (curvedSliding)
            {
                leftDoor.localRotation = leftClosedRotation * Quaternion.Euler(0, openingAngle * t, 0);
                rightDoor.localRotation = rightClosedRotation * Quaternion.Euler(0, -openingAngle * t, 0);
            }
            else
            {
                leftDoor.localPosition = leftClosed + Vector3.left * slideDistance * t;
                rightDoor.localPosition = rightClosed + Vector3.right * slideDistance * t;
            }
        }

        private void CompleteOpen()
        {
            SetDoorProgress(1f);
            Physics.SyncTransforms();
            entranceBlocker.enabled = false;
            IsOpening = false;
            IsOpen = true;
            opening = null;
            SetInterior(true);
            SetStatus("ENTER VR POD", new Color(.3f,1f,.7f));
        }

        private void SetInterior(bool available)
        {
            for (int i = 0; i < uiControls.Length; i++)
                uiControls[i].interactable = available && uiStates[i];
            for (int i = 0; i < xrControls.Length; i++)
                xrControls[i].enabled = available && xrStates[i];
            if (interiorCanvasGroup != null)
            {
                interiorCanvasGroup.interactable = available;
                interiorCanvasGroup.blocksRaycasts = available;
            }
        }

        private void SetStatus(string text, Color color)
        {
            readerStatus.text = text;
            readerStatus.color = color;
        }

        // private void OnDisable()
        // {
        //     if (opening == null) return;
        //     StopCoroutine(opening);
            
        //     CompleteOpen();
        // }
    }
}
