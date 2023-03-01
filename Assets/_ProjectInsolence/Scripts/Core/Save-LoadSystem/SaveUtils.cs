using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Insolence.KinematicCharacterController;
using Insolence.Core;
using Insolence.UI;

namespace Insolence.SaveUtility
{
    public class SaveUtils
    {
        // The directory under Resources that the dynamic objects' prefabs can be loaded from

        private static string PREFABS_PATH = "Prefabs";

        public static Dictionary<string, GameObject> allPrefabs = LoadPrefabs(PREFABS_PATH);

        public static string SAVE_OBJECTS_PATH = "";
        public static string SAVE_PLAYER_PATH = "";


        private static Dictionary<string, GameObject> LoadPrefabs(string prefabsPath)
        {
            Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

            GameObject[] allPrefabs = Resources.LoadAll<GameObject>(prefabsPath);


            
            foreach (GameObject prefab in allPrefabs)
            {
                DynamicObject dynamicObject = prefab.GetComponent<DynamicObject>();
                if (dynamicObject == null)
                {
                    //log error then continue
                    Debug.Log("Skipping Prefab: " + prefab.name + " does not have a DynamicObject component!");
                    
                    continue;
                }
                if (!dynamicObject.objectState.isPrefab)
                {
                    throw new InvalidOperationException("Prefab's ObjectState isPrefab = false");
                }
                prefabs.Add(dynamicObject.objectState.prefabGuid, prefab);
                
            }

            Debug.Log("Loaded " + prefabs.Count + " saveable prefabs.");
            
            //debug log all items in prefabs in a detailed, formatted log
            foreach (KeyValuePair<string, GameObject> prefab in prefabs)
            {
                Debug.Log("Prefab: " + prefab.Key + " " + prefab.Value.name);
            }


            return prefabs;
        }
        public static void DoSave(string sceneName)
        {
            SavePlayer(SAVE_PLAYER_PATH);
            SaveDynamicObjects(SAVE_OBJECTS_PATH, sceneName);

            MenuController mc = GameObject.FindGameObjectWithTag("GameManager").GetComponentInChildren<MenuController>();
            CharacterStatus ps = GetPlayer().GetComponentInChildren<CharacterStatus>();
            mc.levelToLoad = ps.GetScene();
        }

        public static void DoLoad(string playerPath, string objPath, bool menuLoad, string sceneName)
        {
            if (playerPath == "" || objPath == "") 
            {
                return;
            }

            if (menuLoad)
            {
                LoadDynamicObjects(objPath, sceneName);
                LoadPlayer(playerPath);
            }
            else
            {
                LoadDynamicObjects(objPath, sceneName);
            }

        }
        public static GameObject GetPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
           
            return player;
        }

        private static GameObject GetRootDynamicObject()
        {
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("DynamicRoot"))
            {
                if (gameObject.activeSelf)
                {
                    return gameObject;
                }
            }
            throw new InvalidOperationException("Cannot find root of dynamic objects");
        }
        public static string GetSavedSceneName(string path)
        {
            Debug.Log(path);
            if (File.Exists(path))
            {
                
                ObjectState objectState = ReadFile<ObjectState>(path);
                return objectState.genericValues["savedLevel"].ToString();
            }
            else
            {
                throw new InvalidOperationException("cannot find scene name on path " + path);
            }

        }
        private static void SavePlayer(string path)
        {
            GameObject player = GetPlayer();
            if (player == null)
            {
                throw new InvalidOperationException("Cannot find the Player");
            }
            //check if path exists if not create folder

            path = Application.persistentDataPath + "/savegames/" + player.GetComponent<CharacterStatus>().GetStatus()["name"];

            path = IteratePath(path, "player", 1);
            
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Debug.Log("Creating save folder");
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            DynamicObject dynamicObject = player.GetComponent<DynamicObject>();
            List<ObjectState> objectStates = dynamicObject.objectState.Save(player);          

            if (objectStates.Count != 1)
            {
                throw new InvalidOperationException("Expected only 1 object state for the Player");
            }
            WriteFile(path, objectStates[0]);

            SAVE_PLAYER_PATH = path;
            
            string json = JsonConvert.SerializeObject(objectStates[0], Formatting.Indented); File.WriteAllText(path + ".txt", json);

            Debug.Log("Saved Player to: " + path);
        }

        private static void LoadPlayer(string path)
        {
            if (File.Exists(path))
            {
                ObjectState objectState = ReadFile<ObjectState>(path);

                GameObject player = GetPlayer();
                if (player == null)
                {
                    Debug.Log("Player not found!");
                    player = Resources.Load<GameObject>("Prefabs/Characters/Character");
                    player = GameObject.Instantiate(player);
                }

                DynamicObject dynamicObject = player.GetComponent<DynamicObject>();

                player.GetComponent<KineCharacterController>().Motor.SetPositionAndRotation(ConvertToVector3(objectState.position), ConvertToQuaternion(objectState.rotation));

                dynamicObject.Load(objectState);

                SAVE_PLAYER_PATH = path;
                Debug.Log("Loaded Player from: " + path);
            }
            else
            {

                //GameObject noSave = UnityEngine.Object.FindObjectOfType<MenuController>().GetNoSaveDialog();
                //noSave.SetActive(true);
                Debug.LogError("Save file not found in " + SAVE_PLAYER_PATH);
            }
        }

        private static void SaveDynamicObjects(string path, string sceneName)
        {
            SAVE_OBJECTS_PATH = path;
            
            GameObject player = GetPlayer();
            if (player == null)
            {
                throw new InvalidOperationException("Cannot find the Player");
            }

            path = Application.persistentDataPath + "/savegames/" + player.GetComponent<CharacterStatus>().GetStatus()["name"];

            path = IteratePath(path, "obj", 1);

            if (File.Exists(path))
            {
                List<ObjectState> objectStates = ReadFile<List<ObjectState>>(path);

                Debug.Log("save path in SaveDynamicObjects(): " + path);

                foreach (ObjectState objectState in objectStates.ToArray())
                {
                    if (objectState.sceneName == sceneName)
                    {
                        objectStates.Remove(objectState);
                    }
                }

                objectStates.AddRange(ObjectState.SaveObjects(GetRootDynamicObject()));
                string json = JsonConvert.SerializeObject(objectStates, Formatting.Indented); File.WriteAllText(path + ".txt", json);

                WriteFile(path, objectStates);
                Debug.Log("Updated objects in: " + path);
            }
            else
            {

                List<ObjectState> objectStates = ObjectState.SaveObjects(GetRootDynamicObject());

                string json = JsonConvert.SerializeObject(objectStates, Formatting.Indented); File.WriteAllText(path + ".txt", json);
                WriteFile(path, objectStates);
                Debug.Log("Saved objects to: " + path);
            }
        }

        private static void LoadDynamicObjects(string path, string sceneName)
        {
            SAVE_OBJECTS_PATH = path;

            if (File.Exists(path))
            {
                List<ObjectState> objectStates = ReadFile<List<ObjectState>>(path);

                ObjectState.LoadObjects(allPrefabs, objectStates, GetRootDynamicObject(), sceneName);

                Debug.Log("Loaded objects from: " + path);
            }
            else
            {
                Debug.Log("Save file not found in " + SAVE_OBJECTS_PATH);
            }
        }

        private void saveTimeData()
        {
            
        }

        private void loadTimeData()
        {
            
        }
        
        private static string IteratePath(string originalPath, string type, int i)
        {
            string path = originalPath;

            GameObject player = GetPlayer();

            if (player == null)
            {
                throw new InvalidOperationException("Cannot find the Player");
            }


            if (type == "obj")
            {
                if (File.Exists(originalPath + ".inso") && SAVE_OBJECTS_PATH != originalPath + ".inso")
                {
                    path = Application.persistentDataPath + "/savegames/" + player.GetComponent<CharacterStatus>().name + "[" + i + "]";
                    i += 1;
                    path = IteratePath(path, type, i);

                }
                else
                {
                    path += ".inso";
                }
                SAVE_OBJECTS_PATH = path;
                return path;

            }
            else if (type == "player")
            {
                if (File.Exists(originalPath + ".insp") && SAVE_PLAYER_PATH != originalPath + ".insp")
                {
                    path = Application.persistentDataPath + "/savegames/" + player.GetComponent<CharacterStatus>().name + "[" + i + "]";
                    i += 1;
                    path = IteratePath(path, type, i);
                }
                else
                {

                    path += ".insp";
                }

                SAVE_PLAYER_PATH = path;
                return path;

            }
            return null;
        }

        public static string[] GetSaves()
        {
            string[] saves = Directory.GetFiles(Application.persistentDataPath + "/savegames/");

            return saves;
        }
        private static void WriteFile<T>(string path, T obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, obj);
            stream.Close();
        }

        private static T ReadFile<T>(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            T objectState = (T)formatter.Deserialize(stream);

            stream.Close();

            return objectState;
        }
        /// <summary>
        /// Converts a Quaternion into a float array containing the Quaternion's x, y and z values and returns it.
        /// </summary>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static float[] ConvertFromQuaternion(Quaternion quaternion)
        {
            float[] value = { quaternion.eulerAngles.x, quaternion.eulerAngles.y, quaternion.eulerAngles.z };

            return value;
        }

        /// <summary>
        /// Converts a float array containing the Quaternion's x, y and z values into a Quaternion and returns it.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Quaternion ConvertToQuaternion(float[] value)
        {
            return Quaternion.Euler(value[0], value[1], value[2]);
        }

        /// <summary>
        /// Converts a Vector3 to a float array and returns it.
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static float[] ConvertFromVector3(Vector3 vector3)
        {
            float[] values = { vector3.x, vector3.y, vector3.z };

            return values;
        }
        /// <summary>
        /// Converts a float array to a Vector3 and returns it.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Vector3 ConvertToVector3(float[] values)
        {
            return new Vector3(values[0], values[1], values[2]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static float[,] ConvertFromVector3Array(Vector3[] vector3)
        {
            if (vector3 == null)
            {
                return new float[0, 3];
            }

            float[,] values = new float[vector3.Length, 3];

            for (int i = 0; i < vector3.Length; i++)
            {
                values[i, 0] = vector3[i].x;
                values[i, 1] = vector3[i].y;
                values[i, 2] = vector3[i].z;
            }
            return values;
        }

        public static Vector3[] ConvertToVector3Array(float[,] array)
        {
            if (array.Length == 0)
            {
                return null;
            }

            Vector3[] vector3 = new Vector3[array.GetUpperBound(0) + 1];
            for (int i = 0; i < vector3.Length; i++)
            {
                vector3[i] = new Vector3(array[i, 0], array[i, 1], array[i, 2]);
            }
            return vector3;
        }

        internal static int[] ConvertToIntArray(string date)
        {
            //convert a string to an int array
            string[] stringArray = date.Split(',');
            int[] intArray = new int[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
            {
                intArray[i] = int.Parse(stringArray[i]);
            }
            return intArray;
        }

        internal static string ConvertFromIntArray(int[] date)
        {
            //convert an int array to a string
            string[] stringArray = new string[date.Length];
            for (int i = 0; i < date.Length; i++)
            {
                stringArray[i] = date[i].ToString();
            }
            return string.Join(",", stringArray);
        }
    }
}