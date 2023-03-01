using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    public class ShopInventory : MonoBehaviour
    {
        [SerializeField] List<Item> inventory = new List<Item>();
        [SerializeField] bool stocked = false;
        [SerializeField] AllItemsDB allItemsDB;
        [SerializeField] TimeManager tm;

        private void Start()
        {
            tm = GameObject.Find("TimeManager").GetComponent<TimeManager>();
            allItemsDB = GameObject.Find("GameManager").GetComponent<PlayerInfo>().database;
        }

        private void Update()
        {
            //trigger restocks at 6 based on TimeManager
            if (tm.GetTimeOfDay() >= 5.2 && tm.GetTimeOfDay() <= 5.5f && !stocked)
            {
                stocked = true;
                RestockFood();
                RestockPotions();
                SellAllItems();
            }
            else if (tm.GetTimeOfDay() >= 5 && tm.GetTimeOfDay() <= 5.1  && stocked)
            {
                stocked = false;
            }
        }
        public void AddItem(Item item)
        {
            inventory.Add(item);
        }

        public void RemoveItem(Item item)
        {
            inventory.Remove(item);
        }

        //sell all misc items (add value to characterstatus.gold and remove item)
        public void SellAllItems()
        {
            foreach (Item item in inventory.ToList())
            {
                if (item.type == ItemEnums.ItemType.Misc)
                {
                    GetComponent<CharacterStatus>().gold += item.value;
                    RemoveItem(item);
                }
            }
        }
        //create ID list for save/load
        public List<string> CreateItemIDList()
        {
            List<string> idList = new List<string>();
            foreach (Item item in inventory.ToList())
            {
                idList.Add(item.itemID);
            }
            return idList;
        }

        //create item list
        public List<Item> CreateItemList()
        {
            List<Item> itemList = new List<Item>();
            foreach (Item item in inventory.ToList())
            {
                itemList.Add(item);
            }
            return itemList;
        }

        //restock food add random food items from allItemsDB to inventory and remove value from characterstatus.gold
        public void RestockFood()
        {
            int foodCount = 0;
            foreach (Item item in inventory.ToList())
            {
                if (item.type == ItemEnums.ItemType.Consumable && item.consumableType == ItemEnums.ConsumableType.Food)
                {
                    foodCount++;
                }
            }
            if (foodCount < 15)
            {
                int foodToAdd = 10 - foodCount;
                
                var foodQuery = from item in allItemsDB.values
                                where item.type == ItemEnums.ItemType.Consumable && item.consumableType == ItemEnums.ConsumableType.Food
                                select item;

                Item[] foodItems = foodQuery.ToArray();
                
                for (int i = 0; i < foodToAdd; i++)
                {                  
                    int randomFood = Random.Range(0, foodItems.Length);
                    if (GetComponent<CharacterStatus>().gold < foodItems[randomFood].value)
                    {
                        continue;
                    }
                    AddItem(foodItems[randomFood]);
                    GetComponent<CharacterStatus>().gold -= foodItems[randomFood].value;
                }
            }
        }

        //restock potions add random potions items from allItemsDB to inventory and remove value from characterstatus.gold
        
        public void RestockPotions()
        {
            int potionCount = 0;
            foreach (Item item in inventory.ToList())
            {
                if (item.type == ItemEnums.ItemType.Consumable && item.consumableType == ItemEnums.ConsumableType.Potion)
                {
                    potionCount++;
                }
            }
            if (potionCount < 10)
            {
                int potionsToAdd = 10 - potionCount;

                var potionQuery = from item in allItemsDB.values
                                  where item.type == ItemEnums.ItemType.Consumable && item.consumableType == ItemEnums.ConsumableType.Potion
                                  select item;

                Item[] potionItems = potionQuery.ToArray();
                
                
                for (int i = 0; i < potionsToAdd; i++)
                {
                    int randomPotion = Random.Range(0, potionItems.Length);
                    if (GetComponent<CharacterStatus>().gold < potionItems[randomPotion].value)
                    {
                        continue;
                    }
                    AddItem(potionItems[randomPotion]);
                    GetComponent<CharacterStatus>().gold -= potionItems[randomPotion].value;
                }
            }
        }
    }
}
