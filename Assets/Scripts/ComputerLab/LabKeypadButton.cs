using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace EscapeRoom.LabSecurity
{
    public class LabKeypadButton : MonoBehaviour
    {
        public LabDisplayLock target;
        public string key = "0";
        public Transform keyObject;
        private XRSimpleInteractable interactable;
        private Vector3 initPosition;

        private void Awake()
        {
            interactable = GetComponent<XRSimpleInteractable>();
            initPosition = keyObject.localPosition;
        }
        private void OnEnable()
        {
            interactable = GetComponent<XRSimpleInteractable>();
            interactable.activated.AddListener(OnPressed);
            interactable.deactivated.AddListener(OnReleased);
            interactable.hoverExited.AddListener(OnHoverExited);
        }
        private void OnDisable()
        {
            interactable.activated.RemoveListener(OnPressed);
            interactable.deactivated.RemoveListener(OnReleased);
            interactable.hoverExited.RemoveListener(OnHoverExited);
            keyObject.localPosition = initPosition;
        }
        private void OnPressed(ActivateEventArgs args)
        {
            target.PressKey(key);
            keyObject.localPosition = initPosition + Vector3.forward * .004f;
        }
        private void OnHoverExited(HoverExitEventArgs args) => keyObject.localPosition = initPosition;
        private void OnReleased(DeactivateEventArgs args) => keyObject.localPosition = initPosition;
    }
}
