using UnityEngine;

namespace Items
{
    public class ExampleItem : InventoryItem
    {
        public override void Activate()
        {
            Debug.Log("Activated " + displayname);
        }

        public override void Deactivate()
        {
            Debug.Log("Deactivated " + displayname);
        }
    }
}