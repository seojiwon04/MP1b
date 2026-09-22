using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeRoom.ARInterface
{
    public class ARModeController : MonoBehaviour
    {
        public Camera viewCamera;
        public Canvas panelCanvas;
        public CanvasGroup panelGroup;

        public InputActionReference exitAction;
        // public UnityEvent onEnteredAR = new UnityEvent();
        // public UnityEvent onExitedAR = new UnityEvent();

        public bool IsInAR { get; private set; }

        private Coroutine pendingExit;
        private InputAction boundAction;
        private bool enabledAction;
        private static readonly int Glasses = Shader.PropertyToID("_GlassesEnabled");

        private void Awake()
        {
            panelGroup.alpha = 0;
            panelGroup.interactable = false;
            panelGroup.blocksRaycasts = false;
            panelCanvas.gameObject.SetActive(false);
        }

        public void EnterAR()
        {
            if (IsInAR || !isActiveAndEnabled) return;
            viewCamera = Camera.main;
            transform.SetParent(viewCamera.transform,false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            
            panelCanvas.worldCamera = viewCamera;
            Shader.SetGlobalFloat(Glasses,1);
            IsInAR = true;
            BindExit();
            panelGroup.alpha = 1f;
            panelCanvas.gameObject.SetActive(true);
            // onEnteredAR.Invoke();
        }

        public void ExitAR()
        {
            if (pendingExit != null) { StopCoroutine(pendingExit); pendingExit = null; }
            if (!IsInAR) return;
            IsInAR = false;
            UnbindExit();
            Shader.SetGlobalFloat(Glasses, 0);
            panelGroup.alpha = 0f;
            panelCanvas.gameObject.SetActive(false);
            // onExitedAR.Invoke();
        }

        public void ToggleAR() 
        {
            if(IsInAR)
            {
                ExitAR();
            }
            else 
            {
                EnterAR();
            } 
        }

        private void BindExit()
        {
            boundAction = exitAction.action;
            boundAction.performed += OnExitPerformed;
            enabledAction = !boundAction.enabled;
            if (enabledAction) boundAction.Enable();
        }
        private void UnbindExit()
        {
            boundAction.performed -= OnExitPerformed;
            if (enabledAction) boundAction.Disable();
            boundAction = null;
            enabledAction = false;
        }
        private void OnExitPerformed(InputAction.CallbackContext context)
        {
            if (IsInAR && pendingExit == null && isActiveAndEnabled)
                pendingExit = StartCoroutine(ExitNextFrame());
        }

        private IEnumerator ExitNextFrame()
        {
            yield return null;
            pendingExit = null;
            ExitAR();
        }
    }
}
