using UnityEngine;

public class DoorControl : MonoBehaviour
{
    [SerializeField] private ItemChecker checker1;
    [SerializeField] private ItemChecker checker2;

    [SerializeField] private GameObject door;
    private bool locked = true;

    void Update()
    {
        if (!locked)
            return;

        if (checker1.IsMatched && checker2.IsMatched)
        {
            locked = false;
            OpenDoor();
        }
    }   

    void OpenDoor()
    {
        door.SetActive(false);
    }
}
