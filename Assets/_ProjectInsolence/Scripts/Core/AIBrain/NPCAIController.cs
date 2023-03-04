using Insolence.AIBrain.KCC;
using Insolence.Core;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.UIElements;

namespace Insolence.AIBrain
{
    public class NPCAIController : MonoBehaviour
    {
        public NpcKinematicController mover { get; set; }
        public CharacterStatus status { get; set; }
        public Inventory inventory { get; set; }
        public ShopInventory shopInventory { get; set; }


        [SerializeField] NPCPointOfInterest home;
        public AIBrain brain { get; set; }
        public JobType job;
        public Action[] availableActions;
        public GameObject targetInteractable;
        //show interest in editor
        
        public Interest.InterestStruct interest = new Interest.InterestStruct();
        public GameObject destination;

        [Header("Public Flags")]
        public bool enRoute = false;
        public bool hasArrived = false;
        public bool isWorking = false;
        public bool isInteracting = false;
        public bool isTalking = false;

        [Header("Travel Stats")]

        public float travelDistance;
        public float hungerRate = 0.1f;
        public float hungerGainOnArrival;
        public float neededFood;
        public float ownedFood;
            
        public enum JobType
        {
            None,
            Woodcutter,
            Miner,
            Fisher,
            Crafter,
            Cook,
            Hunter,
            Smith,
            Tailor,
            Carpenter,
            Mason,
            Alchemist,
            Gardener,
            Merchant
        }

        public void Start()
        {
            mover = GetComponent<NpcKinematicController>();
            brain = GetComponent<AIBrain>();
            status = GetComponent<CharacterStatus>();
            inventory = GetComponent<Inventory>();

            //if npc is merchant get shopInventory
            if (job == JobType.Merchant)
            {
                shopInventory = GetComponent<ShopInventory>();
            }

        }

        public void Update()
        {
            if (brain.finishedDeciding)
            {
                if (brain.bestAction != null)
                {
                    brain.bestAction.Execute(this);
                    brain.finishedDeciding = false;
                }
            }
            //check inventory for food items every 10 frames
            if (Time.frameCount % 10 == 0)
            {
                ownedFood = 0;
                foreach (Item item in GetComponent<Inventory>().CreateItemList())
                {
                    if (item != null && item.consumableType == ItemEnums.ConsumableType.Food)
                    {
                        ownedFood += item.hungerRestore;
                    }
                }
                //if job is merchant check shopInventory too
                if (job == JobType.Merchant)
                {
                    foreach (Item item in GetComponent<ShopInventory>().CreateItemList())
                    {
                        if (item != null && item.consumableType == ItemEnums.ConsumableType.Food)
                        {
                            ownedFood += item.hungerRestore;
                        }
                    }
                }
            }
            
            hungerGainOnArrival = travelDistance * hungerRate;
            
            if(neededFood < 0)
            {
                neededFood = 0;
            }
            //check if npc is a merchant and has food for sale if yes can eat food for sale and remove it from shop inventory(paying for it still)
            if (job == JobType.Merchant)
            {
                List<Item> inventoryFood = new List<Item>();
                inventoryFood.Clear();

                var inventoryFoodList = from item in inventory.bagItems
                                        where item != null && item.consumableType == ItemEnums.ConsumableType.Food
                                        select item;
                inventoryFood = inventoryFoodList.ToList();

                if (inventoryFood.Count == 0)
                {
                    List<Item> shopFood = new List<Item>();
                    shopFood.Clear();

                    var shopFoodList = from item in GetComponent<ShopInventory>().CreateItemList()
                                       where item != null && item.consumableType == ItemEnums.ConsumableType.Food
                                       select item;
                    shopFood = shopFoodList.ToList();

                    if (shopFood.Count != 0)
                    {
                        ownedFood += shopFood[0].hungerRestore;
                        inventory.AddItem(shopFood[0]);
                        status.gold -= shopFood[0].value;
                        shopFood.Remove(shopFood[0]);
                    }
                }
            }
            
            //if destination changes update navmesh agent
            if (destination != null && destination.transform.position != mover.GetDestination() && enRoute)
            {
                mover.MoveTo(destination.transform.position);
            }
            //set enRout and hasArrived to false if destination changes to null mid-action
            if (destination == null)
            {
                enRoute = false;
                hasArrived = false;
                mover.Stop();
            }
            else
            {
                hasArrived = mover.AtDestination();
                if (hasArrived)
                {
                    hungerGainOnArrival = 0;
                }
            }

        }
        public void OnFinishedAction()
        {
            StopAllCoroutines();
            brain.ChooseBestAction(availableActions);
        }

        public GameObject GetInteractable()
        {
            if (targetInteractable != null)
            {
                return targetInteractable;
            }
            else
            {
                return null;
            }

        }


        #region Coroutine Starters


        public void DoWork(int time)
        {
            if(!isWorking) 
            { 
                isWorking = true;
                StartCoroutine(WorkCoroutine(time));
            }
            else return;
        }

        public void DoSleep(int time)
        {
            StartCoroutine(SleepCoroutine(time));
        }
        public void DoEat(int time)
        {
            StartCoroutine(EatCoroutine(time));
        }

        public void DoInteract()
        {
            StartCoroutine(InteractCoroutine());
        }

        public void MoveToDestination()
        {
                StartCoroutine(MoveToDestinationCoroutine());
        }

        public void DecideDestination()
        {
             StartCoroutine(DecideDestinationCoroutine());
        }

        public void SetInterest(InterestType interestType)
        {
            StartCoroutine(SetInterestCoroutine(interestType));
        }
        public void DoSellMisc()
        {
            StartCoroutine(SellMiscCoroutine());
        }
        public void DoShopKeep(int time)
        {
            StartCoroutine(ShopKeepCoroutine(time));
        }
        public void DoBuyFood()
        {
            StartCoroutine(BuyFoodCoroutine());
        }
        #endregion

        #region Coroutines


        private IEnumerator WorkCoroutine(int time)
        {
            int counter = time;

            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;

            }
            //logic to update things involved with work
            int hunger = GetComponent<CharacterStatus>().hunger;

            if (hunger < 100)
            {
                GetComponent<CharacterStatus>().hunger += 5;
                
            }
            else
            {
                GetComponent<CharacterStatus>().maxHealth -= 50;
            }

            GetComponent<CharacterStatus>().currentMaxStamina -= 5;

            Resource res = destination.GetComponent<Resource>();
            if (res != null)
            {
                res.amount -= 1;
                GetComponent<Inventory>().AddItem(res.GetResourceType().item);
            }

            isWorking = false;
            destination = null;
            
            //decide new best action 
            OnFinishedAction();
        }

        private IEnumerator SleepCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            //logic to update max stamina
            GetComponent<CharacterStatus>().currentMaxStamina += 10;

            OnFinishedAction();
        }
        private IEnumerator EatCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }

            //check if npc has food in its inventory, if yes remove food from inventory and reduce hunger by food.hungerRestore
            List<Item> itemList = inventory.CreateItemList();
            foreach (Item item in itemList)
            {
                if (item != null && item.consumableType == ItemEnums.ConsumableType.Food && status.hunger > 0)
                {
                    
                    status.hunger -= item.hungerRestore;
                    inventory.RemoveItem(item);
                }
            }

            OnFinishedAction();
        }
        private IEnumerator InteractCoroutine()
        {
            if (!isInteracting && hasArrived)
            {
                isInteracting = true;
                GameObject lastInteractable = targetInteractable;

                lastInteractable.GetComponent<Interactable>().Interaction(transform);

                lastInteractable = null;
                targetInteractable = null;
                brain.bestAction = null;
                isInteracting = false;
                OnFinishedAction();
                yield return null;
            }

            if (targetInteractable == null)
            {
                isInteracting = false;
                brain.bestAction = null;
                OnFinishedAction();
            }
        }
        IEnumerator MoveToDestinationCoroutine()
        {

            enRoute = true;
            mover.MoveTo(destination.transform.position);

            yield return new WaitUntil(() => hasArrived);

            enRoute = false;
            status.hunger += (int)hungerGainOnArrival;
            neededFood -= (int)hungerGainOnArrival;
            OnFinishedAction();
            
        }

        public bool sceneIsReady()
        {
            return GameObject.Find("GameManager").GetComponent<GameManager>().sceneReady;
        }
        IEnumerator DecideDestinationCoroutine()
        {
            yield return new WaitUntil(sceneIsReady);

            List<NPCPointOfInterest> poiList = new List<NPCPointOfInterest>();

            if (home != null && home.HasNeededInterest(interest))
            {
                poiList.Add(home);
            }

            var pois = from poi in FindObjectsOfType<NPCPointOfInterest>()
                       where poi != null && poi.GetComponent<NPCPointOfInterest>().HasNeededInterest(interest) && poi.GetComponent<NPCPointOfInterest>().owner == null
                       select poi;

            if (interest.interestType == InterestType.Work)
            {
                pois = from poi in pois
                       where poi != null && poi.GetComponent<NPCPointOfInterest>().HasNeededWorkType(interest)
                       select poi;
            }

            poiList.Clear();
            poiList = pois.ToList();
            
            Interest closestPOI = null;
            float closestDistance = Mathf.Infinity;
            foreach (var poi in poiList)
            {
                if(poi != null)
                {
                    foreach (Interest i in poi.GetComponent<NPCPointOfInterest>().interests)
                    {
                        if (i != null)
                        {
                            float distance = Vector3.Distance(transform.position, i.transform.position);
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestPOI = i;
                            }
                        }
                    }
                }

            }
            if (closestPOI == null)
            {
            }
            else
            {
                if(neededFood == 0)
                {
                    neededFood = closestDistance * hungerRate * 2;
                }
                else if (neededFood < hungerGainOnArrival)
                {
                    neededFood = hungerGainOnArrival;
                }
                destination = closestPOI.gameObject;
                hasArrived = false;
                travelDistance = Vector3.Distance(transform.position, destination.transform.position);
            }
            yield return null;
            OnFinishedAction();
        }

        IEnumerator SetInterestCoroutine(InterestType interestType)
        {
            interest.interestType = interestType;
            interest.UpdateWorkType(this);
            ClearDestAndArrived();

            yield return null;
            OnFinishedAction();
        }

        IEnumerator SellMiscCoroutine()
        {
            if (destination.GetComponent<Interest>().interestType == InterestType.Trade)
            {
                //trade resource for money
                Inventory inv = GetComponent<Inventory>();
                Item resource = inv.equippedInRightHandSlot;
                
                if (destination.GetComponent<CharacterStatus>().gold >= resource.value)
                {
                    destination.GetComponent<CharacterStatus>().gold -= resource.value;
                    GetComponent<CharacterStatus>().gold += resource.value;

                    destination.GetComponent<ShopInventory>().AddItem(resource);
                    inv.RemoveItem(resource);
                }
                    
                
                
            }
            yield return null;
            OnFinishedAction();
        }
        IEnumerator ShopKeepCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }
            int hunger = GetComponent<CharacterStatus>().hunger;

            if (hunger < 100)
            {
                GetComponent<CharacterStatus>().hunger += 1;
                GetComponent<CharacterStatus>().currentMaxStamina -= 1;
            }
            else
            {
                GetComponent<CharacterStatus>().maxHealth -= 100;
            }
            
            yield return null;
            OnFinishedAction();
        }

        //buy food

        IEnumerator BuyFoodCoroutine()
        {
            if (destination.GetComponent<Interest>().interestType == InterestType.Trade)
            {
                //trade money for food
                Inventory inv = GetComponent<Inventory>();
                List<Item> food = new List<Item>();
                
                food.Clear();

                var foodList = from item in destination.GetComponent<ShopInventory>().CreateItemList()
                               where item != null && item.consumableType == ItemEnums.ConsumableType.Food
                               select item;

                food = foodList.ToList();

                if (food.Count > 0 )
                {

                    foreach(Item item in food)
                    {

                        if (GetComponent<CharacterStatus>().gold >= item.value && (neededFood > ownedFood || ownedFood == 0))
                        {

                            GetComponent<CharacterStatus>().gold -= item.value;
                            destination.GetComponent<CharacterStatus>().gold += item.value;

                            ownedFood += item.hungerRestore;

                            inv.AddItem(item);
                            destination.GetComponent<ShopInventory>().RemoveItem(item);
                        }
                    }
                }
                
                if(ownedFood >= neededFood)
                {
                    neededFood = 0;
                }
                
            }
            yield return null;
            OnFinishedAction();
        }
        #endregion

        private void ClearDestAndArrived()
        {
            destination = null;
            hasArrived = false;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Interactable")
            {
                //get interactable
                targetInteractable = other.gameObject;

            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Interactable" && other.gameObject.name == targetInteractable.name)
            {
                targetInteractable = null;
            }
            OnFinishedAction();
        }

        private void OnTriggerStay(Collider other)
        {
            if (targetInteractable == null)
            {
                if (other.gameObject.tag == "Interactable")
                {
                    targetInteractable = other.gameObject;
                }
            }
        }


    }
}