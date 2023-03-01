using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Insolence.SaveUtility;
using Insolence.AIBrain;

namespace Insolence
{
    public class SetUpScene : MonoBehaviour
    {

        private void Start()
        {

        }
        [MenuItem("Insolence Tools/Setup Scene")]

        private static void SetupScene()
        {
            GameObject staticObjects = null;
            GameObject dynamicObjects = null;
            GameObject items = null;
            GameObject npcs = null;

            staticObjects = GameObject.Find("StaticObjects");
            dynamicObjects = GameObject.Find("DynamicObjects");
            items = GameObject.Find("Items");
            npcs = GameObject.Find("NPCs");

            Debug.Log("setting scene up...");

            if(staticObjects == null)
            {
                staticObjects = new GameObject();
                staticObjects.name = "StaticObjects";
            }
            if (dynamicObjects == null)
            {
                dynamicObjects = new GameObject();
                dynamicObjects.name = "DynamicObjects";
                dynamicObjects.tag = "DynamicRoot";
            }
            if (items == null)
            {
                items = new GameObject();
                items.transform.SetParent(dynamicObjects.transform);
                items.name = "Items";
                items.AddComponent<DynamicObject>();
            }
            if(npcs == null)
            {
                npcs = new GameObject();
                npcs.transform.SetParent(dynamicObjects.transform);
                npcs.name = "NPCs";
                npcs.AddComponent<DynamicObject>();
                npcs.AddComponent<NPCPointOfInterest>();
            }
        }
    }
}
