using System;
using UnityEngine;
using Insolence.SaveUtility;
using System.Collections.Generic;
using Insolence.UI;
using Insolence.AIBrain;
using Insolence.KinematicCharacterController;
using System.Collections;
using UnityEngine.InputSystem.LowLevel;

namespace Insolence.Core
{
    public class CharacterStatus : MonoBehaviour, IDamageable
    {
        [Header("Character Stats")]
        [SerializeField] private int _gold;
        public int gold
        {
            get { return _gold; }
            set
            {
                _gold = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _goldNeededPerDay = 100;
        public int goldNeededPerDay
        {
            get { return _goldNeededPerDay; }
            set
            {
                _goldNeededPerDay = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _level;
        public int level
        {
            get { return _level; }
            set
            {
                _level = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _experience;
        public int experience
        {
            get { return _experience; }
            set
            {
                _experience = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _maxHealth;
        public int maxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _currentHealth;
        public int currentHealth
        {
            get { return _currentHealth; }
            set
            {
                _currentHealth = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _maxStamina;
        public int maxStamina
        {
            get { return _maxStamina; }
            set
            {
                _maxStamina = value;
                OnStatValueChanged?.Invoke();
            }
        }
        //add currentMaxStamina
        [SerializeField] private int _currentMaxStamina;
        public int currentMaxStamina
        {
            get { return _currentMaxStamina; }
            set
            {
                _currentMaxStamina = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _staminaRegen = 5;
        public int staminaRegen
        {
            get { return _staminaRegen; }
            set
            {
                _staminaRegen = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _runStaminaCost = 5;

        [SerializeField] private int _currentStamina;
        public int currentStamina
        {
            get { return _currentStamina; }
            set
            {
                _currentStamina = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _hunger;
        public int hunger
        {
            get { return _hunger; }
            set
            {
                _hunger = Mathf.Clamp(value, 0, _maxHunger);
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _maxHunger = 100;
        public int maxHunger
        {
            get { return _maxHunger; }
            set
            {
                _maxHunger = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _maxMana;
        public int maxMana
        {
            get { return _maxMana; }
            set
            {
                _maxMana = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private int _currentMana;
        public int currentMana
        {
            get { return _currentMana; }
            set
            {
                _currentMana = value;
                OnStatValueChanged?.Invoke();
            }
        }
        [SerializeField] private string _name;
        public new string name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        [SerializeField] private int _weaponDamage;
        public int weaponDamage
        {
            get { return _weaponDamage; }
            set
            {
                _weaponDamage = value;
            }
        }

        bool canRegenerate;
        public bool canRun = true;
        
        [SerializeField] private Billboard billboard;

        public delegate void StatValueChangedHandler();
        public event StatValueChangedHandler OnStatValueChanged;

        [SerializeField] public AllItemsDB database;

        [SerializeField] Inventory inv;
        List<string> invList = new List<string>();

        [SerializeField] private string currentScene;

        private void Start()
        {
            if (database == null)
            {
                database = GameObject.Find("GameManager").GetComponent<PlayerInfo>().database;
            }
            DynamicObject dynamicObject = GetComponent<DynamicObject>();
            dynamicObject.prepareToSaveDelegates += PrepareToSaveObjectState;
            dynamicObject.loadObjectStateDelegates += LoadObjectState;

            inv = GetComponent<Inventory>();

        }

        private void Update()
        {
            currentScene = gameObject.scene.name;
            
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            if (currentStamina >= currentMaxStamina)
            {
                currentStamina = currentMaxStamina;
            }
            if (currentMana >= maxMana)
            {
                currentMana = maxMana;
            }
            if (currentMaxStamina >= maxStamina)
            {
                currentMaxStamina = maxStamina;
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
            }
            if (currentStamina <= 0)
            {
                currentStamina = 0;
            }
            if (currentMana <= 0)
            {
                currentMana = 0;
            }
            if (currentMaxStamina <= 0)
            {
                currentMaxStamina = 0;
            }
            

            UpdateWeaponDamage();
            SetObjectName();

            if (GetComponent<KineCharacterController>().isRunning)
            {
                canRegenerate = false;
            }
            else
            {
                canRegenerate = true;
            }
            
            //call regenerateStamina() if stamina is less than current max stamina every 30 frames
            if (currentStamina < currentMaxStamina && Time.frameCount % 30 == 0)
            {
                if (canRegenerate)
                {
                    regenerateStamina();
                }

            }

            //call RunConsumeStamina() if KineCharacterController.isRunning is true every 30 frames
            if (GetComponent<KineCharacterController>().isRunning && Time.frameCount % 30 == 0)
            {
                ConsumeStamina(_runStaminaCost);
            }

            //start run CD if stamina == 0, until stamina is equal to max stamina
            if (currentStamina <= 0)
            {
                StartCoroutine(RunCD());
            }

        }

        private IEnumerator RunCD()
        {
            while(currentStamina != maxStamina)
            {
                canRun = false;
                yield return new WaitForSeconds(1);
            }

            canRun = true;
        }
        
        private void regenerateStamina()
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegen;
            }
        }

        private void ConsumeStamina(int amount)
        {
            currentStamina -= amount;
        }
        public Dictionary<string, string> GetStatus()
        {
            Dictionary<string, string> status = new Dictionary<string, string>();

            status.Add("name", name);
            status.Add("level", level.ToString());
            status.Add("maxHealth", maxHealth.ToString());
            status.Add("currentHealth", currentHealth.ToString());
            status.Add("maxStamina", maxStamina.ToString());
            status.Add("currentStamina", currentStamina.ToString());
            status.Add("hunger", hunger.ToString());
            status.Add("gold", gold.ToString());
            status.Add("maxMana", maxMana.ToString());
            status.Add("currentMana", currentMana.ToString());

            return status;
        }
        public void SetStatus(Dictionary<string, string> status)
        {
            Debug.Log("setting status after scene load");

            _name = status["name"];
            _level = Convert.ToInt32(status["level"]);
            _maxHealth = Convert.ToInt32(status["maxHealth"]);
            _currentHealth = Convert.ToInt32(status["currentHealth"]);
            _maxStamina = Convert.ToInt32(status["maxStamina"]);
            _currentStamina = Convert.ToInt32(status["currentStamina"]);
            _hunger = Convert.ToInt32(status["hunger"]);
            _gold = Convert.ToInt32(status["gold"]);
            _maxMana = Convert.ToInt32(status["maxMana"]);
            _currentMana = Convert.ToInt32(status["currentMana"]);


            Debug.Log("Done setting status");
        }

        public string GetScene()
        {
            return currentScene;
        }

        public void Damage(int damage)
        {
            currentHealth -= damage;
            Debug.Log(name + " took " + damage + " damage.");
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Debug.Log(name + " is dead");
            }
        }

        private void SetObjectName()
        {
            if(transform.name != name)
            {
                transform.name = name;
            }
        }
        private void UpdateWeaponDamage()
        {
            Item rWeapon = GetComponent<Inventory>().equippedInRightHandSlot;
            Item lWeapon = GetComponent<Inventory>().equippedInLeftHandSlot;

            if(rWeapon != null )
            {
                weaponDamage = rWeapon.damage;
            }
            else
            {
                weaponDamage = 0;
            }
            if(lWeapon != null)
            {
                weaponDamage += lWeapon.damage;
            }
            if(currentStamina < maxStamina)
            {
                weaponDamage *= (currentStamina / maxStamina);
            }
            
        }


        #region Inventory Save/Load
        private void PrepareToSaveObjectState(ObjectState objectState)
        {
            objectState.genericValues[name + ".Stats.level"] = level;
            objectState.genericValues[name + ".Stats.health"] = maxHealth;
            objectState.genericValues[name + ".Stats.currentHealth"] = currentHealth;
            objectState.genericValues[name + ".Stats.maxStamina"] = maxStamina;
            objectState.genericValues[name + ".Stats.currentStamina"] = currentStamina;
            objectState.genericValues[name + ".Stats.hunger"] = hunger;
            objectState.genericValues[name + ".Stats.name"] = name;
            objectState.genericValues["savedLevel"] = currentScene;

            objectState.genericValues[name + ".Inventory"] = inv.CreateItemIDList();
            objectState.genericValues[name + ".Inventory.gold"] = gold;

            //if npc merchant save shopinventory
            if (tag == "NPC" && GetComponent<ShopInventory>() != null)
            {
                objectState.genericValues[name + ".ShopInventory"] = GetComponent<ShopInventory>().CreateItemIDList();
            }


            //if tag == player save time and date from TimeManager
            if (gameObject.tag == "Player")
            {
                //TimeManager tm = GameObject.Find("TimeManager").GetComponent<TimeManager>();
                objectState.timeValues[name + ".timeScale"] = TimeManager.Instance.GetTimeScale();
                objectState.timeValues[name + ".timeOfDay"] = TimeManager.Instance.GetTimeOfDay();
                objectState.timeValues[name + ".date"] = SaveUtils.ConvertFromIntArray(TimeManager.Instance.GetDate());
                objectState.timeValues[name + ".currentSeason"] = TimeManager.Instance.GetSeason();
            }
        }
        private void LoadObjectState(ObjectState objectState)
        {
            // Load the player's position & rotation
            transform.position = SaveUtils.ConvertToVector3(objectState.position);
            transform.rotation = SaveUtils.ConvertToQuaternion(objectState.rotation);
            // Load the reference to the stats
            level = Convert.ToInt32(objectState.genericValues[name + ".Stats.level"]);
            maxHealth = Convert.ToInt32(objectState.genericValues[name + ".Stats.health"]);
            currentHealth = Convert.ToInt32(objectState.genericValues[name + ".Stats.currentHealth"]);
            maxStamina = Convert.ToInt32(objectState.genericValues[name + ".Stats.maxStamina"]);
            currentStamina = Convert.ToInt32(objectState.genericValues[name + ".Stats.currentStamina"]);
            hunger = Convert.ToInt32(objectState.genericValues[name + ".Stats.hunger"]);
            name = Convert.ToString(objectState.genericValues[name + ".Stats.name"]);
            currentScene = Convert.ToString(objectState.genericValues["savedLevel"]);

            invList = (List<string>)objectState.genericValues[name + ".Inventory"];
            invList.Reverse();
            foreach (string itemID in invList)
            {
                inv.AddItem(database.FindItem(itemID));
            }
            
            gold = Convert.ToInt32(objectState.genericValues[name + ".Inventory.gold"]);

            //if npc merchant load shopinventory
            if (tag == "NPC" && GetComponent<ShopInventory>() != null)
            {
                List<string> itemList = (List<string>)objectState.genericValues[name + ".ShopInventory"];
                itemList.Reverse();
                foreach (string itemID in itemList)
                {
                    GetComponent<ShopInventory>().AddItem(database.FindItem(itemID));
                }
            }

            //if tag == player load time and date and set them in TimeManager using the set methods
            if (gameObject.tag == "Player")
            {
                //TimeManager tm = GameObject.Find("TimeManager").GetComponent<TimeManager>();
                TimeManager.Instance.SetTimeScale((int)objectState.timeValues[name + ".timeScale"]);
                TimeManager.Instance.SetTimeOfDay(Convert.ToSingle(objectState.timeValues[name + ".timeOfDay"]));
                TimeManager.Instance.SetDate(SaveUtils.ConvertToIntArray((string)objectState.timeValues[name + ".date"]));
                TimeManager.Instance.SetSeason(Convert.ToInt32(objectState.timeValues[name + ".currentSeason"]));
            }
        }
        #endregion

        private void OnEnable()
        {
            OnStatValueChanged += UpdateDisplayText;
        }

        private void OnDisable()
        {
            OnStatValueChanged -= UpdateDisplayText;
        }

        void UpdateDisplayText()
        {
            if (billboard != null)
            {
                billboard.UpdateStatsText(maxStamina, hunger, gold);
            }
        }
    }
}