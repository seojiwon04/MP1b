using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace EscapeRoom.Inventory
{
    [System.Serializable]
    public class ItemPrefab
    {
        public string itemId;
        public GameObject prefab;
    }

    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Inventory { get; private set; }
        [SerializeField] private List<ItemPrefab> itemPrefabs;

        public Transform spawnPoint;

        public List<string> items = new();
        
        private Dictionary<string, GameObject> prefabMap = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics() => Inventory = null;

        private void Awake()
        {
            if (Inventory == null)
            {
                Inventory = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
                return;
            }

            foreach (ItemPrefab item in itemPrefabs)
            {
                prefabMap.Add(item.itemId, item.prefab);
            }
        }

        private void OnDestroy()
        {
            if (Inventory == this) Inventory = null;
        }

        public void AddItem(string itemId)
        {
            items.Add(itemId);
        }

        public bool HasItem(string itemId)
        {
            return items.Contains(itemId);
        }

        public void RemoveItem(string itemId)
        {
            items.Remove(itemId);
        }

        public void TakeOutItem(string itemId)
        {
            if (!items.Contains(itemId)) return;
            
            var prefab = prefabMap[itemId];
            Vector3 spawnPos = spawnPoint.position + spawnPoint.forward*.2f;
            Instantiate(prefab, spawnPos, spawnPoint.rotation);
            RemoveItem(itemId);
        }
    }
}
