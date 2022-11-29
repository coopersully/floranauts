using System.Collections.Generic;
using Items;
using Planets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Interfaces
{
    public class PlayerItems : MonoBehaviour
    {
        [Header("UI Elements")]
        public RectTransform[] slots;

        private int _numSlots;
        public Image selectedFrame;
        private int _selectedIndex = 0;

        [Header("Inventory")]
        public List<InventoryItem> items;
        private InventoryItem _selectedItem;

        private void Awake()
        {
            _numSlots = slots.Length;
            items = new List<InventoryItem>(_numSlots);
            UpdateSelectedItem();
        }

        public void ActionIncreaseSelectedIndex(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            _selectedIndex += 1;
            if (_selectedIndex >= _numSlots) _selectedIndex = 0;
            UpdateSelectedItem();
        }
        
        public void ActionDecreaseSelectedIndex(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            
            _selectedIndex -= 1;
            if (_selectedIndex < 0) _selectedIndex = _numSlots - 1;
            UpdateSelectedItem();
        }

        private void UpdateSelectedItem()
        {
            // Update UI cursor
            Debug.Log("Player selected slot " + _selectedIndex);
            selectedFrame.rectTransform.SetPositionAndRotation(slots[_selectedIndex].position, selectedFrame.rectTransform.rotation);
            
            // Deactivate old item
            _selectedItem.Deactivate();
            
            // Update & activate new item
            _selectedItem = items[_selectedIndex];
            _selectedItem.Activate();
        }

        public void RemoveItem(PlanetType planetType)
        {
            if (planetType == PlanetType.None) return;
            
            foreach (var item in items)
            {
                if (item.type != planetType) continue;
                
                item.Deactivate();
                Destroy(item);
            }
        }
        
        public void AddItem(PlanetType planetType)
        {
            if (planetType == PlanetType.None) return;
            // items.Add(); ??
        }
    }
}
