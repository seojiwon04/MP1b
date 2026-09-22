
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.Events;

namespace EscapeRoom.Baseball
{
    [RequireComponent(typeof(XRSimpleInteractable))]
    public class BaseballThrowingStation : MonoBehaviour
    {
        public BaseballProjectile ballPrefab;
        public InputActionReference fireLeft, fireRight, exitMode;
        public float launchSpeed = 12f;
        public float fireCooldown = .3f;
        public int maximumLiveBalls = 40;
        public TMP_Text status;
        public ParticleSystem launchEffectPrefab;
        public AudioClip launchSound;

        public bool IsInThrowingMode { get; private set; }

        private XRSimpleInteractable interactable;
        private XRBaseInputInteractor leftHand, rightHand;
        private InputAction leftAction, rightAction, exitAction;
        private Coroutine session;

        private readonly HashSet<InputAction> enabledActions = new();
        private readonly Dictionary<XRBaseInputInteractor, bool> previousActivate = new();
        private readonly List<BaseballProjectile> balls = new();

        private bool armed, leftRequested, rightRequested, exitRequested;
        private float leftNext, rightNext;

        private void Awake()
            => interactable = GetComponent<XRSimpleInteractable>();

        private void OnEnable()
        {
            interactable.activated.AddListener(OnActivate);
            SetStatus(false);
        }

        private void OnDisable()
        {
            interactable.activated.RemoveListener(OnActivate);
            ExitThrowingMode();
        }

        private void OnActivate(ActivateEventArgs args)
            => EnterThrowingMode();

        public void EnterThrowingMode()
        {
            if (IsInThrowingMode) return;

            leftHand = rightHand = null;

            foreach (var hand in FindObjectsByType<XRBaseInputInteractor>(FindObjectsSortMode.None))
            {
                if (!hand.isActiveAndEnabled ||
                    !(hand is NearFarInteractor || hand is XRRayInteractor))
                    continue;

                if (hand.handedness == InteractorHandedness.Left)
                    leftHand = hand;
                else if (hand.handedness == InteractorHandedness.Right)
                    rightHand = hand;
            }

            if (!leftHand && !rightHand) return;

            IsInThrowingMode = true;
            session = StartCoroutine(ThrowingSession());
        }

        private void EnableAction(InputAction action)
        {
            if (action.enabled) return;
            enabledActions.Add(action);
            action.Enable();
        }

        private void BindActions()
        {
            armed = leftRequested = rightRequested = exitRequested = false;

            leftAction = fireLeft.action;
            rightAction = fireRight.action;
            exitAction = exitMode.action;

            leftAction.performed += OnLeftPerformed;
            rightAction.performed += OnRightPerformed;
            exitAction.performed += OnExitPerformed;

            EnableAction(leftAction);
            EnableAction(rightAction);
            EnableAction(exitAction);
        }

        private void UnbindActions()
        {
            leftAction.performed -= OnLeftPerformed;
            rightAction.performed -= OnRightPerformed;
            exitAction.performed -= OnExitPerformed;

            foreach (var action in enabledActions)
                action.Disable();

            enabledActions.Clear();
            leftAction = rightAction = exitAction = null;
            armed = leftRequested = rightRequested = exitRequested = false;
        }

        private void OnLeftPerformed(InputAction.CallbackContext context)
        {
            if (IsInThrowingMode && armed) leftRequested = true;
        }

        private void OnRightPerformed(InputAction.CallbackContext context)
        {
            if (IsInThrowingMode && armed) rightRequested = true;
        }

        private void OnExitPerformed(InputAction.CallbackContext context)
        {
            if (IsInThrowingMode) exitRequested = true;
        }

        private IEnumerator ThrowingSession()
        {
            yield return null;

            BindActions();

            foreach (var hand in new[] { leftHand, rightHand })
            {
                if (!hand) continue;

                previousActivate[hand] = hand.allowActivate;
                hand.allowActivate = false;
            }

            SetStatus(true);

            yield return null;

            while (IsInThrowingMode)
            {
                if (exitRequested)
                {
                    ExitThrowingMode();
                    yield break;
                }

                if (!armed)
                    armed = !leftAction.IsPressed() && !rightAction.IsPressed();

                if (leftRequested && Time.time >= leftNext)
                {
                    Fire(leftHand);
                    leftNext = Time.time + fireCooldown;
                }

                if (rightRequested && Time.time >= rightNext)
                {
                    Fire(rightHand);
                    rightNext = Time.time + fireCooldown;
                }

                leftRequested = rightRequested = false;
                yield return null;
            }
        }

        private Transform Aim(XRBaseInputInteractor hand)
        {
            if (hand is NearFarInteractor near)
                return near.curveOrigin;

            if (hand is XRRayInteractor ray)
                return ray.rayOriginTransform;

            return hand.transform;
        }

        public void Fire(XRBaseInputInteractor hand)
        {
            if (!IsInThrowingMode || !hand) return;

            balls.RemoveAll(ball => !ball);
            if (balls.Count >= maximumLiveBalls) return;

            Transform aim = Aim(hand);
            Vector3 origin = aim.position + aim.forward * .2f;
            var ball = Instantiate(ballPrefab, origin, Random.rotation);

            var ignored = new List<Collider>(
                hand.transform.root.GetComponentsInChildren<Collider>()
            );
            ignored.AddRange(GetComponentsInChildren<Collider>());

            ball.Launch(aim.forward * launchSpeed, ignored.ToArray());
            balls.Add(ball);

            AudioSource.PlayClipAtPoint(launchSound, origin, .45f);

            var fx = Instantiate(launchEffectPrefab, origin, aim.rotation);
            fx.Play(true);
            Destroy(fx.gameObject, 2f);
        }

        public void ExitThrowingMode()
        {
            IsInThrowingMode = false;

            if (session != null)
            {
                StopCoroutine(session);
                session = null;
            }

            UnbindActions();

            foreach (var pair in previousActivate)
                if (pair.Key) pair.Key.allowActivate = pair.Value;

            previousActivate.Clear();
            SetStatus(false);
        }

        private void SetStatus(bool active)
        {
            status.text = active
                ? "THROW MODE\nTRIGGER: FIRE   B: EXIT"
                : "BASEBALLS\nAIM + TRIGGER TO START";
        }
    }
}
