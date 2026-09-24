using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace EscapeRoom.Inventory
{
    public class InventoryWindow : MonoBehaviour
    {
        public InputActionReference toggleAction;
        public GameObject panel;
        public TMP_Text contents;
        public bool IsOpen => panel && panel.activeSelf;
        private InputAction boundAction;
        private InventoryManager inventory;

        private void Awake() 
        { 
            panel.SetActive(false); 
        }

        private void OnEnable()
        {
            boundAction = toggleAction.action;
            boundAction.performed += OnToggle;
            boundAction.Enable();
        }
        private void OnDisable()
        {
            if (boundAction != null)
            {
                boundAction.performed -= OnToggle;
                boundAction.Disable();
            }
            boundAction = null;
            Close();
        }
        private void OnToggle(InputAction.CallbackContext context) => Toggle();

        public void Toggle()
        {
            if (IsOpen) 
            { 
                Close(); 
                return; 
            }
            inventory = InventoryManager.Inventory;
            if (inventory) inventory.ItemsChanged += RefreshContents;
            RefreshContents();
            panel.SetActive(true);
        }
        public void Close()
        {
            if (inventory) inventory.ItemsChanged -= RefreshContents;
            inventory = null;
            panel.SetActive(false);
        }
        public void RefreshContents()
        {
            if (!inventory || inventory.items.Count == 0) 
            { 
                contents.text = "Your inventory is empty."; 
                return; 
            }
            var text = new StringBuilder();
            foreach (var id in inventory.items)
            {

                text.Append("* ").Append(DisplayName(id)).AppendLine();
            }
            contents.text = text.ToString().TrimEnd();
        }
        private static string DisplayName(string id) => id switch
        {
            "COMPUTER_LAB" => "Lab Keycard",
            "TROPHY" => "Trophy",
            "TEST_ITEM" => "Practice Item",
            "YELLOW_SCISSOR" => "Yellow Scissors",
            _ => id.Replace('_', ' ')
        };
    }
}
        