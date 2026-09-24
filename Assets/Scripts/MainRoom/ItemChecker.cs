using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections.Generic;

public class ItemChecker : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private List<string> keyItems;
    private bool isMatched;

    public bool IsMatched => isMatched;


    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnItemInserted);
        socket.selectExited.AddListener(OnItemRemoved);
        isMatched = false;
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnItemInserted);
        socket.selectExited.RemoveListener(OnItemRemoved);
    }

    private void OnItemInserted(SelectEnterEventArgs args)
    {
        var item = args.interactableObject.transform.GetComponent<InventoryItem>();
        if (item == null) 
            return;

        isMatched = keyItems.Contains(item.ItemId);
    }

    private void OnItemRemoved(SelectExitEventArgs args)
    {
        isMatched = false;
    }
}