using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace EscapeRoom.Baseball
{
    public class BaseballTrophyReward : MonoBehaviour
    {
        public BaseballChallenge challenge;
        public BaseballThrowingStation throwingStation;
        public XRSimpleInteractable claimInteraction;
        public XRGrabInteractable grabInteraction;
        public Rigidbody trophyBody;
        public GameObject portal;
        public TMP_Text instructions;
        // public UnityEvent onRewardClaimed = new UnityEvent();
        public bool IsUnlocked { get; private set; }
        public bool IsClaimed { get; private set; }
        Coroutine claimRoutine;

        void Awake()
        {
            claimInteraction.enabled = false;
            grabInteraction.enabled = false;
            trophyBody.isKinematic = true;
            trophyBody.useGravity = false;
            portal.SetActive(false);
            SetText("TROPHY LOCKED\nPASS ALL 3 TARGETS");
        }

        void OnEnable()
        {
            claimInteraction.activated.AddListener(OnActivated);
            challenge.onChallengeCompleted.AddListener(UnlockReward);
            if (challenge.IsComplete)
            {
                UnlockReward();
            }
        }

        void OnDisable()
        {
            claimInteraction.activated.RemoveListener(OnActivated);
            challenge.onChallengeCompleted.RemoveListener(UnlockReward);
            if (claimRoutine != null)
            {
                StopCoroutine(claimRoutine);
                claimRoutine = null;
                if (IsClaimed) EnablePickup();
            }
        }

        public void UnlockReward()
        {
            if (IsUnlocked || !challenge.IsComplete) return;
            IsUnlocked = true;
            
            if (throwingStation) throwingStation.ExitThrowingMode();
            claimInteraction.enabled = true;
            SetText("CHALLENGE COMPLETE\nTRIGGER TO CLAIM TROPHY");
        }

        void OnActivated(ActivateEventArgs args) => ClaimReward();

        public void ClaimReward()
        {
            if (!IsUnlocked || IsClaimed) return;
            IsClaimed = true;
            claimRoutine = StartCoroutine(ClaimAfterInput());
        }

        IEnumerator ClaimAfterInput()
        {
            yield return null;
            EnablePickup();
            claimRoutine = null;
            // onRewardClaimed.Invoke();
        }

        void EnablePickup()
        {
            claimInteraction.enabled = false;
            trophyBody.useGravity = true;
            trophyBody.isKinematic = false;
            grabInteraction.enabled = true;
            portal.SetActive(true);
            SetText("TROPHY CLAIMED\nGRIP TO PICK UP");
        }

        void SetText(string text) { if (instructions) instructions.text = text; }
    }
}
