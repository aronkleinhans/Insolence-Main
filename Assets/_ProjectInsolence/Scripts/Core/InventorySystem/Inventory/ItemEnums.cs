using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    [Serializable]
    public class ItemEnums : MonoBehaviour
    {
        public enum ItemType
        {
            Weapon,
            Armor,
            Consumable,
            Quest,
            Key,
            Misc
        }

        public enum WeaponType
        {
            None,
            Sword,
            Axe,
            Hammer,
            Bow,
            Staff,
            Dagger,
            Shield,
            AncientScroll,
            Tome
        }
        public enum ArmorType
        {
            None,
            Head,
            Face,
            Chest,
            Legs,
            Feet,
            Hands,
            Back,
            Neck,
            Ring
        }
        public enum ItemSize
        {
            None,
            Big,
            Medium,
            Small
        }
        public enum ConsumableType
        {
            None,
            Food,
            Drink,
            Potion,
            Scroll
        }
    }
}
