using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insolence.SaveUtility;
using UnityEngine.UIElements;
using UnityEngine.TextCore.Text;
using System.IO;
using System;

namespace Insolence.Core
{
    public class RagdollInfo : MonoBehaviour
    {
        public string modelName;
        public string characterName;
        public bool isSimpleModel;

        [SerializeField]private List<string> bones = new List<string>();
        private List<Vector3> positions = new List<Vector3>();
        private List<Quaternion> rotations = new List<Quaternion>();
        private List<Vector3> scales = new List<Vector3>();

        Inventory inv;
        public int gold;


        private void Start()
        {
            inv = GetComponentInChildren<Inventory>();
            //add save and load
            DynamicObject dynamicObject = GetComponent<DynamicObject>();
            dynamicObject.prepareToSaveDelegates += PrepareToSaveObjectState;
            dynamicObject.loadObjectStateDelegates += LoadObjectState;
        }

        private void Update()
        {
            if(isSimpleModel && !transform.Find(modelName).gameObject.activeSelf)
            {
                transform.Find(modelName).gameObject.SetActive(true);
            }
            
        }

        private void PrepareToSaveObjectState(ObjectState objectState)
        {
            //get the positions and rotations of all the parts using getPR
            bones.Clear();
            positions.Clear();
            rotations.Clear();
            scales.Clear();
            getPR("Root", positions, rotations, scales, bones);
            //save the current position and rotation of all bones of the object in dynamicObject.generic
            
            objectState.genericValues[name + ".name"] = characterName;
            objectState.genericValues[name + "." + characterName + ".modelName"] = modelName;
            objectState.genericValues[name + "." + characterName + ".isSimpleModel"] = isSimpleModel;
            
            for (int i = 0; i < positions.Count; i++)
            {
                if (!objectState.genericValues.ContainsKey(name + "." + characterName + "." + i + ".position"))
                {
                    objectState.genericValues[name + "." + characterName + "." + i + ".position"] = SaveUtils.ConvertFromVector3(positions[i]);
                    objectState.genericValues[name + "." + characterName + "." + i + ".rotation"] = SaveUtils.ConvertFromQuaternion(rotations[i]);
                    objectState.genericValues[name + "." + characterName + "." + i + ".scale"] = SaveUtils.ConvertFromVector3(scales[i]);
                    objectState.genericValues[name + "." + characterName + "." + i + ".bone"] = bones[i];

                }
            }
            //save inventory
            objectState.genericValues[name + ".Inventory"] = inv.CreateItemIDList();
            objectState.genericValues[name + ".Inventory.gold"] = gold;
        }
        private void LoadObjectState(ObjectState objectState)
        {
            //load the position and rotation of all bones of the object
            characterName = (string)objectState.genericValues[name + ".name"];
            modelName = (string)objectState.genericValues[name + "." + characterName + ".modelName"];
            isSimpleModel = (bool)objectState.genericValues[name + "." + characterName + ".isSimpleModel"];

            foreach (KeyValuePair<string, object> s in objectState.genericValues)
            {
                if (s.Key.Contains("position"))
                {
                    positions.Add(SaveUtils.ConvertToVector3((float[])s.Value));
                }
                else if (s.Key.Contains("rotation"))
                {
                    rotations.Add(SaveUtils.ConvertToQuaternion((float[])s.Value));
                }
                else if (s.Key.Contains("scale"))
                {
                    scales.Add(SaveUtils.ConvertToVector3((float[])s.Value));
                }
                else if (s.Key.Contains("bone"))
                {
                    bones.Add((string)s.Value);
                }
            }
            setPR("Root", positions, rotations, scales, bones, 0);

            //load inventory
            List<string> invList = new List<string>();
            AllItemsDB database = GameObject.Find("GameManager").GetComponent<PlayerInfo>().database;
            
            Debug.Log("Loading " + name + "'s inventory");
            invList = (List<string>)objectState.genericValues[name + ".Inventory"];
            invList.Reverse();
            foreach (string itemID in invList)
            {
                Debug.Log("Loading item: " + itemID);
                inv.AddItem(database.FindItem(itemID));
            }

            gold = Convert.ToInt32(objectState.genericValues[name + ".Inventory.gold"]);

        }

        private void getPR(string path, List<Vector3> positions, List<Quaternion> rotations, List<Vector3> scales, List<string> bones)
        {
            //add using a queue
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                string currentPath = queue.Dequeue();
                Transform currentTransform = transform.Find(currentPath);
                positions.Add(currentTransform.localPosition);
                rotations.Add(currentTransform.localRotation);
                scales.Add(currentTransform.localScale);
                bones.Add(currentTransform.name);
                foreach (Transform child in currentTransform)
                {
                    queue.Enqueue(currentPath + "/" + child.name);
                }
            }
        }

        private void setPR(string path, List<Vector3> positions, List<Quaternion> rotations, List<Vector3> scales, List<string> bones, int index)
        {
            //add using a queue
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                string currentPath = queue.Dequeue();
                Transform currentTransform = transform.Find(currentPath);
                currentTransform.localPosition = positions[index];
                currentTransform.localRotation = rotations[index];
                currentTransform.localScale = scales[index];
                index++;
                foreach (Transform child in currentTransform)
                {
                    queue.Enqueue(currentPath + "/" + child.name);
                }
            }
        }
    }
}
