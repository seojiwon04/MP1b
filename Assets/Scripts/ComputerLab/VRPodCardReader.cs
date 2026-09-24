using UnityEngine;

namespace EscapeRoom.LabSecurity
{
    public class VRPodCardReader : MonoBehaviour
    {
        public VRPodAccessDoor door;

        private void OnTriggerEnter(Collider other)
        {
            LabAccessCard card = other.GetComponentInParent<LabAccessCard>();
            if (card != null) 
            {
                door.TryOpenWithCard(card);
            }
        }
    }
}
