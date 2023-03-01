using System;
using System.Collections.Generic;
using Insolence.UI;
using UnityEngine;


namespace Insolence.Core
{
    [Serializable]
    public class Inventory : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] public List<Item> bigItems = new List<Item>();
        [SerializeField] public List<Item> mediumItems = new List<Item>();
        [SerializeField] public List<Item> quickItems = new List<Item>();
        [SerializeField] public List<Item> bagItems = new List<Item>();
        [SerializeField] public List<Item> essentialItems = new List<Item>();

        [Header("Equipped Items")]
        [SerializeField] public Item headSlot;
        [SerializeField] public Item faceSlot;
        [SerializeField] public Item chestSlot;
        [SerializeField] public Item legsSlot;
        [SerializeField] public Item handsSlot;
        [SerializeField] public Item feetSlot;
        [SerializeField] public Item backSlot;
        [SerializeField] public Item neckSlot;
        [SerializeField] public Item ringSlotA;
        [SerializeField] public Item ringSlotB;
        [SerializeField] public Item ringSlotC;
        [SerializeField] public Item ringSlotD;

        [SerializeField] public Item equippedInRightHandSlot;
        [SerializeField] public Item equippedInLeftHandSlot;

        [Header("Equipment Slots")]
        [SerializeField] public GameObject _headSlot;
        [SerializeField] public GameObject _faceSlot;
        [SerializeField] public GameObject _chestSlot;
        [SerializeField] public GameObject _legsSlot;
        [SerializeField] public GameObject _handsSlot;
        [SerializeField] public GameObject _feetSlot;
        [SerializeField] public GameObject _backSlot;
        [SerializeField] public GameObject _neckSlot;
        [SerializeField] public GameObject _ringSlotA;
        [SerializeField] public GameObject _ringSlotB;
        [SerializeField] public GameObject _ringSlotC;
        [SerializeField] public GameObject _ringSlotD;

        [SerializeField] public GameObject _equippedInRightHandSlot;
        [SerializeField] public GameObject _equippedInLeftHandSlot;

        [Header("Inventory Settings")]
        [SerializeField] public int maxBigSlots = 2; //on characters upper back
        [SerializeField] public int maxMediumSlots = 3; //on chars sides(hips) and lower back
        [SerializeField] public int maxQuickSlots = 3; //on chars upgradable belt, fills from bag items out of combat, type selectable per slot from dropdown on equip screen
        [SerializeField] public int maxBagSlots = 20; //potions etc, not visible on char, slower activation
        [SerializeField] public float pushForce = 5f;

        [SerializeField]public List<Item> weaponList = new List<Item>();
        [SerializeField] public Item emptyHand;

        [SerializeField] private bool isSuccessful;
        public bool holdingItem = false;
        public bool holdingWeapon = false;
        private Billboard billboard;


        private void OnEnable()
        {
            if (weaponList.Count == 0)
            {
                weaponList.Add(emptyHand);
            }
            if(equippedInRightHandSlot == emptyHand) 
            {
                weaponList.Remove(equippedInRightHandSlot);
            }
        }

        private void Update()
        {
            if(equippedInRightHandSlot != null && equippedInRightHandSlot.type == ItemEnums.ItemType.Weapon)
            {
                holdingWeapon = true;
            }
            else
            {
                holdingWeapon = false;
            }
        }
        public bool AddItem(Item item)
        {
            billboard = GetComponentInChildren<Billboard>();
            checkItemType(item, true);
            return isSuccessful;
        }

        /// <summary>
        /// Spawns the item with given ID in the world
        /// </summary>
        /// <param name="itemID"></param>
        public void SpawnItem(string itemID)
        {
            Item item = GameObject.Find("GameManager").GetComponent<PlayerInfo>().database.FindItem(itemID);
            if (item != null)
            {
                GameObject itemObject = Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
            }
        }
        public void DropItem(Item item)
        {
            Debug.Log("dropping " + item.name);
            GameObject droppedItem = Instantiate(item.itemPrefab, gameObject.transform.position + gameObject.transform.forward, Quaternion.LookRotation(gameObject.transform.forward));
            droppedItem.transform.parent = GameObject.FindGameObjectWithTag("DynamicRoot").transform.Find("Items").transform;
           
            
            if (item == emptyHand)
            {
                Destroy(droppedItem);
            }
            else
            {
                droppedItem.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward + transform.up * pushForce, ForceMode.Impulse);
            }



            _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = null;
            equippedInRightHandSlot = null;
            _equippedInLeftHandSlot.GetComponent<ItemSOHolder>().item = null;
            equippedInLeftHandSlot = null;


        }
        public void RemoveItem(Item item)
        {
            checkItemType(item, false);
        }

        /// <summary>
        /// Returns True if inventory is full
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            if (bigItems.Count >= maxBigSlots && mediumItems.Count >= maxMediumSlots && bagItems.Count >= maxBagSlots)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region inventory save/load

        public List<string> CreateItemIDList()
        {
            List<Item> inventory = new List<Item>();

            inventory.AddRange(bigItems);
            inventory.AddRange(mediumItems);
            inventory.AddRange(quickItems);
            inventory.AddRange(bagItems);
            inventory.AddRange(essentialItems);

            inventory.Add(headSlot);
            inventory.Add(faceSlot);
            inventory.Add(chestSlot);
            inventory.Add(legsSlot);
            inventory.Add(handsSlot);
            inventory.Add(feetSlot);
            inventory.Add(backSlot);
            inventory.Add(neckSlot);
            inventory.Add(ringSlotA);
            inventory.Add(ringSlotB);
            inventory.Add(ringSlotC);
            inventory.Add(ringSlotD);

            inventory.Add(equippedInRightHandSlot);

            List<string> itemIDs = new List<string>();

            foreach (Item item in inventory)
            {
                if (item != null)
                {
                    itemIDs.Add(item.itemID);
                }
            }

            return itemIDs;
        }

        public List<Item> CreateItemList()
        {
            List<Item> inventory = new List<Item>();

            inventory.AddRange(bigItems);
            inventory.AddRange(mediumItems);
            inventory.AddRange(quickItems);
            inventory.AddRange(bagItems);
            inventory.AddRange(essentialItems);

            inventory.Add(headSlot);
            inventory.Add(faceSlot);
            inventory.Add(chestSlot);
            inventory.Add(legsSlot);
            inventory.Add(handsSlot);
            inventory.Add(feetSlot);
            inventory.Add(backSlot);
            inventory.Add(neckSlot);
            inventory.Add(ringSlotA);
            inventory.Add(ringSlotB);
            inventory.Add(ringSlotC);
            inventory.Add(ringSlotD);

            inventory.Add(equippedInRightHandSlot);

            return inventory;
        }
            #endregion

            #region weapon cycling and dualwield

            public void CycleRightHandWeapons()
        {
            if (equippedInRightHandSlot != null)
            {
                if (AddItem(equippedInRightHandSlot))
                {
                    if (weaponList.Count > 0)
                    {
                        equippedInRightHandSlot = weaponList[0];
                        _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = equippedInRightHandSlot;
                        RemoveItem(equippedInRightHandSlot);
                        weaponList.Remove(weaponList[0]);
                    }

                    else
                    {
                        if(equippedInRightHandSlot.type != ItemEnums.ItemType.Misc)
                        {
                            weaponList.Add(equippedInRightHandSlot);
                        }
                        equippedInRightHandSlot = null;
                        _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = equippedInRightHandSlot;                      
                    }     
                    if(equippedInLeftHandSlot != equippedInRightHandSlot)
                    {
                        equippedInLeftHandSlot = null;
                        _equippedInLeftHandSlot.GetComponent<ItemSOHolder>().item = null;
                    }
                }
                else
                {
                    DropItem(equippedInRightHandSlot);
                    equippedInRightHandSlot = weaponList[0];
                    _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = equippedInRightHandSlot;
                    weaponList.Remove(weaponList[0]);
                }
            }
            else
            {
                equippedInRightHandSlot = weaponList[0];
                _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = equippedInRightHandSlot;
                RemoveItem(equippedInRightHandSlot);
                weaponList.Remove(weaponList[0]);
            }
            if (!weaponList.Contains(emptyHand) && equippedInRightHandSlot != emptyHand)
            {
                weaponList.Add(emptyHand);
            }
        }

        public void DualWieldWeapons()
        {
            if (equippedInRightHandSlot.pairedWeapon)
            {
                if (equippedInLeftHandSlot == null)
                {
                    equippedInLeftHandSlot = equippedInRightHandSlot;
                    _equippedInLeftHandSlot.GetComponent<ItemSOHolder>().item = equippedInLeftHandSlot;
                }
                
                else
                {
                    equippedInLeftHandSlot = null;
                    _equippedInLeftHandSlot.GetComponent<ItemSOHolder>().item = null;
                }
                

                Debug.Log("Equipped/unequipped DW item");
            }
        }
        #endregion
        
        #region Type&SlotChecks
        private void checkItemType(Item item, bool isAdd)
        {
            switch (item.type)
            {
                case ItemEnums.ItemType.Weapon:
                    {
                        checkSize(item, isAdd);
                        break;
                    }

                case ItemEnums.ItemType.Armor:
                    {
                        checkArmorType(item, isAdd);
                        break;
                    }


                case ItemEnums.ItemType.Consumable:
                    {
                        if (quickItems.Count < maxQuickSlots && item.consumableType != ItemEnums.ConsumableType.Food)
                        {
                            checkSize(item, isAdd);
                            break;
                        }
               
                        else if (quickItems.Count == maxQuickSlots || (item.consumableType == ItemEnums.ConsumableType.Food && bagItems.Count < maxBagSlots))
                        {
                            checkSlots("consumable", item, isAdd);
                            isSuccessful = true;
                            break;
                        }
                        else
                        {
                            isSuccessful = false;
                            Debug.Log("bag full, swap");
                        }
                        break;
                    }

                case ItemEnums.ItemType.Key:
                    {
                        checkSlots("key", item, isAdd);
                        break;
                    }

                case ItemEnums.ItemType.Quest:
                    {
                        checkSlots("quest", item, isAdd);
                        break;
                    }

                case ItemEnums.ItemType.Misc:
                    {
                        checkSlots("misc", item, isAdd);
                        break;
                    }
                
                default:
                    {
                        checkSlots("fail", item, isAdd);
                        break;
                    }
            }
        }

        private void checkSize(Item item, bool isAdd)
        {
            switch (item.size)
            {
                case ItemEnums.ItemSize.Big:
                    {
                        checkSlots("big", item, isAdd);
                        break;
                    }

                case ItemEnums.ItemSize.Medium:
                    {
                        checkSlots("medium", item, isAdd);
                        break;
                    }

                case ItemEnums.ItemSize.Small:
                    {
                        checkSlots("small", item, isAdd);
                        break;
                    }

                default:
                    {
                        checkSlots("fail", item, isAdd);
                        break;
                    }
            }
        }

        private void checkArmorType(Item item, bool isAdd)
        {
            switch (item.armorType)
            {
                case ItemEnums.ArmorType.Head:
                    {
                        checkSlots("head", item, isAdd);
                        break;
                    }

                case ItemEnums.ArmorType.Face:
                    {
                        checkSlots("face", item, isAdd);
                        break;
                    }

                case ItemEnums.ArmorType.Chest:
                    {
                        checkSlots("chest", item, isAdd);
                        break;
                    }

                case ItemEnums.ArmorType.Legs:
                    {
                        checkSlots("legs", item, isAdd);
                        break;
                    }

                case ItemEnums.ArmorType.Feet:
                    {
                        checkSlots("feet", item, isAdd);
                        break;
                    }

                case ItemEnums.ArmorType.Hands:
                    {
                        checkSlots("hands", item, isAdd);
                        break;
                    }

                case ItemEnums.ArmorType.Back:
                    {
                        checkSlots("back", item, isAdd);
                        break;
                    }

                case ItemEnums.ArmorType.Neck:
                    {
                        checkSlots("neck", item, isAdd);
                        break;
                    }

                case ItemEnums.ArmorType.Ring:
                    {
                        checkSlots("ring", item, isAdd);
                        break;
                    }

                default:
                    {
                        checkSlots("fail", item, isAdd);
                        break;
                    }
            }
        }

        public void checkSlots(string type, Item item, bool isAdd)
        {
            
            if (isAdd)
            {
                switch (type)
                {
                    case "big":
                        {
                            if (equippedInRightHandSlot != null && bigItems.Count == maxBigSlots)
                            {
                                DropItem(equippedInRightHandSlot);
                                equippedInRightHandSlot = item;
                                _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = item;
                                isSuccessful = true;
                            } 
                            else if (equippedInRightHandSlot == null)
                            {
                                equippedInRightHandSlot = item;
                                _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = item;
                                isSuccessful = true;
                            }
                            else if (bigItems.Count < maxBigSlots)
                            {
                                bigItems.Add(item);
                                weaponList.Add(item);
                                isSuccessful = true;
                            }
                            break;
                        }

                    case "medium":
                        {
                            if (equippedInRightHandSlot != null && mediumItems.Count == maxMediumSlots)
                            {
                                DropItem(equippedInRightHandSlot);
                                equippedInRightHandSlot = item;
                                _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = item;
                                isSuccessful = true;
                            }
                            else if (equippedInRightHandSlot == null)
                            {
                                equippedInRightHandSlot = item;
                                _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = item;
                                isSuccessful = true;
                            }
                            else if (mediumItems.Count < maxMediumSlots)
                            {
                                mediumItems.Add(item);
                                weaponList.Add(item);
                                isSuccessful = true;
                            }
                            break;
                        }

                    case "small":
                        {
                            if (quickItems.Count < maxQuickSlots)
                            {
                                quickItems.Add(item);
                                isSuccessful = true;
                            }
                            else
                            {
                                isSuccessful = false;
                                Debug.Log("quick slots full, goes to bag");
                            }
                            break;
                        }
                        
                    case "consumable":
                        {
                            if (bagItems.Count < maxBagSlots)
                            {
                                bagItems.Add(item);
                                isSuccessful = true;
                            }
                            else
                            {
                                isSuccessful = false;
                                Debug.Log("bag slots full, goes to quick");
                            }
                            break;
                        }

                    case "head":
                        {
                            if (headSlot == null)
                            {
                                headSlot = item;
                                isSuccessful = true;
                            }
                            else
                            {
                                isSuccessful = false;
                                Debug.Log("slot not empty, swap");
                            }
                            break;
                        }
                    case "face":
                        {
                            if (faceSlot == null)
                            {
                                faceSlot = item;
                                isSuccessful = true;
                            }
                            else
                            {
                                isSuccessful = false;
                                Debug.Log("slot not empty, swap");
                            }
                            break;
                        }
                    case "chest":
                        {
                            if (chestSlot == null)
                            {
                                chestSlot = item;
                                isSuccessful = true;
                            }
                            else
                            {
                                isSuccessful = false;
                                Debug.Log("slot not empty, swap");
                            }
                            break;
                        }
                    case "legs":
                        {
                            if (legsSlot == null)
                            {
                                legsSlot = item;
                            }
                            else
                            {
                                isSuccessful = false;
                                Debug.Log("slot not empty, swap");
                            }
                            break;
                        }
                    case "hands":
                        {
                            if (handsSlot == null)
                            {
                                handsSlot = item;
                                isSuccessful = true;
                            }
                            else
                            {
                                isSuccessful = false;
                                Debug.Log("slot not empty, swap");
                            }
                            break;
                        }
                    case "feet":
                        {
                            if (feetSlot == null)
                            {
                                feetSlot = item;
                                isSuccessful = true;
                            }
                            else
                            {
                                isSuccessful = false;
                                Debug.Log("slot not empty, swap");
                            }
                            break;
                        }
                    case "back":
                        {
                            if (backSlot == null)
                            {
                                backSlot = item;
                                _backSlot.GetComponent<ItemSOHolder>().item = item;
                                isSuccessful = true;
                            }
                            else
                            {
                                Debug.Log("slot not empty, swap");
                                isSuccessful = false;
                            }
                            break;
                        }
                    case "neck":
                        {
                            if (neckSlot == null)
                            {
                                neckSlot = item;
                                isSuccessful = true;
                            }
                            else
                            {
                                Debug.Log("slot not empty, swap");
                                isSuccessful = false;
                            }
                            break;
                        }
                    case "ring":
                        {
                            if (ringSlotA == null)
                            {
                                ringSlotA = item;
                                isSuccessful = true;
                                break;
                            }
                            if (ringSlotB == null)
                            {
                                ringSlotB = item;
                                isSuccessful = true;
                                break;
                            }
                            if (ringSlotC == null)
                            {
                                ringSlotC = item;
                                isSuccessful = true;
                                break;
                            }
                            if (ringSlotD == null)
                            {
                                ringSlotD = item;
                                isSuccessful = true;
                                break;
                            }
                            else
                            {
                                isSuccessful = false;
                                Debug.Log("slots not empty, swap");
                                break;
                            }

                        }
                    case "misc":
                        {
                            if (equippedInRightHandSlot == null)
                            {
                                equippedInRightHandSlot = item;
                                _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = item;
                                if (billboard != null)
                                {
                                    billboard.UpdateInventoryText(item.name);
                                }
                                
                                isSuccessful = true;
                            }
                            else
                            {
                                if (AddItem(equippedInRightHandSlot))
                                {
                                    equippedInRightHandSlot = item;
                                    _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = item;
                                }
                                else
                                {
                                    DropItem(equippedInRightHandSlot);
                                    equippedInRightHandSlot = item;
                                    _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = item;
                                }
                                
                                
                                if (billboard != null)
                                {
                                    billboard.UpdateInventoryText(item.name);
                                }
                                isSuccessful = true;
                            }
                            break;
                        }
                    case "fail":
                        {
                            Debug.Log("Failed to pick" + item.name + " up.");
                            isSuccessful = false;
                            break;
                        }
                    default:
                        {
                            essentialItems.Add(item);
                            isSuccessful = true;
                            break;
                        }
                }
            }
            else
            {
                switch (type)
                {
                    case "big":
                        {
                            if (bigItems.Contains(item))
                            {
                                bigItems.Remove(item);
                            }
                            break;
                        }

                    case "medium":
                        {
                            if (mediumItems.Contains(item))
                            {
                                mediumItems.Remove(item);
                            }
                            break;
                        }

                    case "small":
                        {
                            if (quickItems.Contains(item))
                            {
                                quickItems.Remove(item);
                            }
                            break;
                        }

                    case "consumable":
                        {
                            if (bagItems.Contains(item))
                            {
                                bagItems.Remove(item);
                            }
                            break;
                        }

                    case "head":
                        {
                            if (headSlot != null)
                            {
                                DropItem(headSlot);
                                headSlot = null;
                            }
                            break;
                        }
                    case "face":
                        {
                            if (faceSlot != null)
                            {
                                DropItem(faceSlot);
                                faceSlot = null;
                            }
                            break;
                        }
                    case "chest":
                        {
                            if (chestSlot != null)
                            {
                                chestSlot = null;
                            }
                            break;
                        }
                    case "legs":
                        {
                            if (legsSlot != null)
                            {
                                legsSlot = null;
                            }
                            break;
                        }
                    case "hands":
                        {
                            if (handsSlot != null)
                            {
                                handsSlot = null;
                            }
                            break;
                        }
                    case "feet":
                        {
                            if (feetSlot != null)
                            {
                                feetSlot = null;
                                isSuccessful = true;
                            }
                            break;
                        }
                    case "back":
                        {
                            if (backSlot != null)
                            {
                                backSlot = null;
                            }
                            break;
                        }
                    case "neck":
                        {
                            if (neckSlot != null)
                            {
                                neckSlot = null;
                            }
                            break;
                        }
                    case "ring":
                        {
                            if (ringSlotA != null)
                            {
                                ringSlotA = null;
                                break;
                            }
                            if (ringSlotB != null)
                            {
                                ringSlotB = null;
                                break;
                            }
                            if (ringSlotC != null)
                            {
                                ringSlotC = null;
                                break;
                            }
                            if (ringSlotD != null)
                            {
                                ringSlotD = null;
                                break;
                            }
                            break;

                        }
                    case "misc":
                        {
                            if (equippedInRightHandSlot != null)
                            {
                                equippedInRightHandSlot = null;
                                _equippedInRightHandSlot.GetComponent<ItemSOHolder>().item = null;
                            }
                            if (billboard != null)
                            {
                                billboard.UpdateInventoryText(" ");
                            }
                            break;
                        }
                    case "fail":
                        {
                            Debug.Log("Failed to pick" + item.name + " up.");
                            break;
                        }
                    default:
                        {
                            essentialItems.Remove(item);
                            break;
                        }

                }
            }
        }
        #endregion
    }
}
