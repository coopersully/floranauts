using System;
using System.Collections.Generic;
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

        [Header("Item Sprites")]
        public Texture spriteNone;
        public Texture spriteStick;
        public Texture spriteRocketLauncher;
        public Texture spriteSpeedIncrease;
        public Texture spriteFreezeRay;
        public Texture spriteJetpack;

        [Header("Inventory")]
        public List<PlanetType> items;
        public PlanetType selectedItem;

        private void Awake()
        {
            _numSlots = slots.Length;
            items = new List<PlanetType>(_numSlots);
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

            // Update & activate new item
            selectedItem = items[_selectedIndex];
        }

        public void RemoveItem(PlanetType planetType)
        {
            if (planetType == PlanetType.None) return;
            
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != planetType) continue;
                
                items[i] = PlanetType.None;
            }
        }
        
        public void AddItem(PlanetType planetType)
        {
            if (planetType == PlanetType.None) return;
            var success = false;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != PlanetType.None) continue;
                
                items[i] = planetType;
                success = true;
            }
            if (!success) throw new IndexOutOfRangeException("Couldn't add item to player; inventory full");
        }
    }
}
