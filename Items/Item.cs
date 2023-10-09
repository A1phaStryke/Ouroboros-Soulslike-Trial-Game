using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public string itemName;
        public Sprite itemIcon;
        [TextArea] public string itemDescription;
        public int itemID;
        
    }
}
