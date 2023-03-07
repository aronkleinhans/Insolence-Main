using System.Collections.Generic;
using System;
using UnityEngine;
using Insolence.SaveUtility;



namespace Insolence.Core
{
    public class RagdollManager : MonoBehaviour
    {
        private CharacterStatus status;
        [SerializeField] GameObject ragdollPrefab;
        [SerializeField] GameObject ragdoll;
        [SerializeField] GameObject character;
        [SerializeField] bool isSimpleModel; //inventory doesn't affect look & doesn't/shoudn't have a equipped armor items
        [SerializeField] string modelName;

        void Start()
        {
            status = GetComponent<CharacterStatus>();
            character = transform.Find("Root").GetChild(0).gameObject;
            modelName = character.name;
        }

        void Update()
        {
            if(status.currentHealth <= 0)
            {
                SwapToRagdoll();
            }
        }

        //swap the skeleton to ragdoll skeleton
        private void SwapToRagdoll()
        {
            //instantiate a ragdoll object
            if (ragdoll == null)
            {
                ragdoll = Instantiate(ragdollPrefab);
                ragdoll.transform.SetParent(GameObject.Find("DynamicObjects").transform);
                
            }

            //set characters mesh to visible based on its name
            if (isSimpleModel)
            {
                ragdoll.transform.Find(modelName).gameObject.SetActive(true);
            }
            else
            {
                //implement later for complex models
            }

            ragdoll.name = "Dead_" + status.name + "_Ragdoll";

            //copy the position & rotation of all parts
            ragdoll.transform.position = transform.position;
            ragdoll.transform.rotation = transform.rotation;

            copyPR("Root");

            //add list of transforms to ragdoll's lists but copy  not just reference
            RagdollInfo ragdollInfo = ragdoll.GetComponentInChildren<RagdollInfo>();
            //ragdollInfo.positions = new List<Vector3>(positions);
            //ragdollInfo.rotations = new List<Quaternion>(rotations);
            ragdollInfo.modelName = new string(modelName);
            ragdollInfo.characterName = new string(status.name);
            ragdollInfo.isSimpleModel = isSimpleModel;


            //set tag and set up interaction if needed

            //create loot & pillage interaction later (if npc has equipable items add pillage option after loot)


            //copy the inventory
            Inventory inv = GetComponent<Inventory>();

            List<string> ids = inv.CreateItemIDList();

            Inventory rInv = ragdoll.GetComponentInChildren<Inventory>();
            ragdollInfo.gold = status.gold;

            foreach (string id in ids)
            {
                rInv.AddItem(GetComponent<CharacterStatus>().database.FindItem(id));
            }
            //destroy the character
            Destroy(this.gameObject);

        }

        //copy all bone transforms to ragdoll
        private void copyPR(string path)
        {
            Transform t = character.transform.Find(path);
            Transform r = ragdoll.transform.Find(path);

            if (r != null)
            {
                r.position = t.position;
                r.rotation = t.rotation;
            }

            foreach (Transform child in t)
            {
                copyPR(path + "/" + child.name);
            }
        }
    }
}
