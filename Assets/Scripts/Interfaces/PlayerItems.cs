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
        public Image spriteStick;
        public Image spriteRocketLauncher;
        public Image spriteSpeedIncrease;
        public Image spriteFreezeRay;
        public Image spriteJetpack;

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

        private void RefreshInventory()
        {
            spriteStick.gameObject.SetActive(false);
            spriteFreezeRay.gameObject.SetActive(false);
            spriteRocketLauncher.gameObject.SetActive(false);
            spriteSpeedIncrease.gameObject.SetActive(false);
            spriteJetpack.gameObject.SetActive(false);
            
            for (int i = 0; i < _numSlots; i++)
            {
                switch (items[i])
                {
                    case PlanetType.None:
                        continue;
                    case PlanetType.Stick:
                        spriteStick.gameObject.SetActive(true);
                        spriteStick.rectTransform.SetPositionAndRotation(slots[i].position, spriteStick.rectTransform.rotation);
                        break;
                    case PlanetType.FreezeGun:
                        spriteFreezeRay.gameObject.SetActive(true);
                        spriteFreezeRay.rectTransform.SetPositionAndRotation(slots[i].position, spriteFreezeRay.rectTransform.rotation);
                        break;
                    case PlanetType.RocketLauncher:
                        spriteRocketLauncher.gameObject.SetActive(true);
                        spriteRocketLauncher.rectTransform.SetPositionAndRotation(slots[i].position, spriteRocketLauncher.rectTransform.rotation);
                        break;
                    case PlanetType.SpeedIncrease:
                        spriteSpeedIncrease.gameObject.SetActive(true);
                        spriteSpeedIncrease.rectTransform.SetPositionAndRotation(slots[i].position, spriteSpeedIncrease.rectTransform.rotation);
                        break;
                    case PlanetType.Jetpack:
                        spriteJetpack.gameObject.SetActive(true);
                        spriteJetpack.rectTransform.SetPositionAndRotation(slots[i].position, spriteJetpack.rectTransform.rotation);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void RemoveItem(PlanetType planetType)
        {
            if (planetType == PlanetType.None) return;
            
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != planetType) continue;
                
                items[i] = PlanetType.None;
            }
            RefreshInventory();
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
            RefreshInventory();
            
            
            
            
            
            
            
            if (!success) throw new IndexOutOfRangeException("Couldn't add item to player; inventory full");
        }
    }
}
