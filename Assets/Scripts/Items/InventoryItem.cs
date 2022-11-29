using System;
using Planets;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public abstract class InventoryItem : MonoBehaviour
    {
        [Header("UI Elements")]
        public string displayname = null;
        public Image icon;

        [Header("Item Settings")]
        public PlanetType type;

        private void Awake()
        {
            displayname ??= name;
        }

        public abstract void Activate();
        public abstract void Deactivate();
    }
}