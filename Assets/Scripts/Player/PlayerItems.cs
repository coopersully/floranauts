using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Planets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player
{
    public class PlayerItems : MonoBehaviour
    {
        [Header("UI Elements")]
        public RectTransform[] slots;

        private int _numSlots;
        public Image selectedFrame;
        private int _selectedIndex = 0;
        public PlayerActionBar playerActionBar;

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
            items = new List<PlanetType>();
            for (int i = 0; i < _numSlots; i++) items.Add(PlanetType.None);
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
            selectedFrame.rectTransform.SetPositionAndRotation(slots[_selectedIndex].position,
                selectedFrame.rectTransform.rotation);

            // Update & activate new item
            Debug.Log("Player selected index " + _selectedIndex + "/" + (items.Count - 1));
            selectedItem = items[_selectedIndex];

            // Send action bar event
            if (selectedItem == PlanetType.None) return;
            switch (selectedItem)
            {
                case PlanetType.None:
                    break;
                case PlanetType.Stick:
                    playerActionBar.Send("Press [RT] to swing stick.");
                    break;
                case PlanetType.FreezeGun:
                    playerActionBar.Send("Press [RT] to shoot Freeze Gun.");
                    break;
                case PlanetType.RocketLauncher:
                    playerActionBar.Send("Press [RT] to shoot Rocket Launcher.");
                    break;
                case PlanetType.Jetpack:
                    playerActionBar.Send("Press [RT] to use Jetpack.");
                    break;
                case PlanetType.SpeedIncrease:
                    playerActionBar.Send("Press [RT] to use Speed Boost.");
                    break;
                default:
                    playerActionBar.Send("Press [RT] to use item.");
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RefreshInventory()
        {
            spriteStick.gameObject.SetActive(false);
            spriteFreezeRay.gameObject.SetActive(false);
            spriteRocketLauncher.gameObject.SetActive(false);
            spriteSpeedIncrease.gameObject.SetActive(false);
            spriteJetpack.gameObject.SetActive(false);
            
            for (var i = 0; i < _numSlots; i++)
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
            
            for (var i = 0; i < items.Count; i++)
            {
                /* If the current item is not the one we
                 are trying to revoke from them, ignore it.*/
                if (items[i] != planetType) continue;
                
                items[i] = PlanetType.None; // Remove the item
                RefreshInventory(); // Update the inventory UI
                UpdateSelectedItem(); // Update selected item UI & registry
                break;
            }
        }
        
        public void AddItem(PlanetType planetType)
        {
            if (planetType == PlanetType.None) return;
            var success = false;
            for (var i = 0; i < slots.Length; i++)
            {
                // Debug statement for iterating over free slots
                if (items[i] != PlanetType.None)
                {
                    Debug.Log("Couldn't add to slot " + i + " because it was " + items[i]);
                    continue;
                }
                
                Debug.Log(name + " acquired the item " + planetType);

                items[i] = planetType; // Add & set the item
                RefreshInventory(); // Update the inventory UI
                UpdateSelectedItem(); // Update selected item UI & registry
                success = true;
                break;
            }
            if (!success) throw new IndexOutOfRangeException("Couldn't add item to player; inventory full");
        }
    }
}
