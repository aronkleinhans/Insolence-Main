using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Insolence.SaveUtility;
using UnityEditor;

namespace Insolence.Core
{
    public class PlayerInfo : MonoBehaviour
    {
        [Header("Player Stats")]
        [SerializeField] string playerName;
        [SerializeField] int level;
        [SerializeField] int maxHealth;
        [SerializeField] int currentHealth;
        [SerializeField] int maxStamina;
        [SerializeField] int currentStamina;
        [SerializeField] int hunger;
        [SerializeField] int gold;
        [SerializeField] int maxMana;
        [SerializeField] int currentMana;

        [SerializeField] Inventory inv;
        List<string> invList = new List<string>();
        [SerializeField] public AllItemsDB database;

        public Vector3 playerPosition;
        public GameObject spawnPoint;


        private void Start()
        {

        }
        void Update()
        {

        }

        public void GetPlayerInfo()
        {

            Dictionary<string, string> currentState = SaveUtils.GetPlayer().GetComponent<CharacterStatus>().GetStatus(); //SaveUtils.GetPlayer().GetComponent<CharacterStatus>();

            playerName = currentState["name"];
            level =  Convert.ToInt32(currentState["level"]);
            maxHealth = Convert.ToInt32(currentState["maxHealth"]);
            currentHealth = Convert.ToInt32(currentState["currentHealth"]);
            maxStamina = Convert.ToInt32(currentState["maxStamina"]);
            currentStamina = Convert.ToInt32(currentState["currentStamina"]);
            hunger = Convert.ToInt32(currentState["hunger"]);
            gold = Convert.ToInt32(currentState["gold"]);
            maxMana = Convert.ToInt32(currentState["maxMana"]);
            currentMana = Convert.ToInt32(currentState["currentMana"]);


            inv = SaveUtils.GetPlayer().GetComponent<Inventory>();
            invList = inv.CreateItemIDList();
            invList.Reverse();


            foreach (string itemID in invList)
            {
                Debug.Log("Adding ItemID to inventory list: " + database.FindItem(itemID).itemID);
            }
        }
        public void UpdateCharacterState(GameObject player)
        {
            Dictionary<string, string> currentState = new Dictionary<string, string>
            {
                { "name", playerName },
                { "level", level.ToString() },
                { "maxHealth", maxHealth.ToString() },
                { "currentHealth", currentHealth.ToString() },
                { "maxStamina", maxStamina.ToString() },
                { "currentStamina", currentStamina.ToString() },
                { "hunger", hunger.ToString() },
                { "gold", gold.ToString() },
                { "maxMana", maxMana.ToString() },
                { "currentMana", currentMana.ToString() }
            };

            foreach (KeyValuePair<string, string> item in currentState)
            {
                Debug.Log("Updating Character State: " + item);
            }

            player.GetComponent<CharacterStatus>().SetStatus(currentState);


            inv = player.GetComponent<Inventory>();

            foreach (string itemID in invList)
            {
                Debug.Log("Adding Item back to inventory by ID: " + itemID);
                inv.AddItem(database.FindItem(itemID));
            }
        }
    }
}