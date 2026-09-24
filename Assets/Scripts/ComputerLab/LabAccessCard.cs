using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using EscapeRoom.Inventory;

namespace EscapeRoom.LabSecurity
{
    public class LabAccessCard : XRGrabInteractable
    {
        [SerializeField] private string accessId = "COMPUTER_LAB";
        [SerializeField] private bool isLocked = false;   // Define whether the access card is interactable

        // Read Only; for other scripts to read certain attributes of a Card
        public string AccessId => accessId; // the ID of a card
        public bool IsLocked => isLocked;   // the status of a card

        protected override void Awake()
        {
            base.Awake();
            SetLocked(isLocked);
        }

        // base.IsSelectableBy(interactor) => Status of select/hover
        public override bool IsSelectableBy(IXRSelectInteractor interactor) => !isLocked && base.IsSelectableBy(interactor);
        public override bool IsHoverableBy(IXRHoverInteractor interactor) => !isLocked && base.IsHoverableBy(interactor);

        public void SetLocked(bool locked)
        {
            isLocked = locked;
            Rigidbody body = GetComponent<Rigidbody>();
            body.isKinematic = locked;
            body.useGravity = !locked;
        }

        public void StoreInInventory()
        {
            InventoryManager.Inventory.AddItem(accessId);
            Destroy(gameObject);
        }
    }
}
