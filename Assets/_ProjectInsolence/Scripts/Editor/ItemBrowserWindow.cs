using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Insolence.Core
{
    public class ItemBrowserWindow : EditorWindow
    {
        private string searchString = "";
        private Vector2 scrollPos;
        private string[] itemTypes;
        private int selectedIndex;
        private Dictionary<string, Database> databases;

        List<bool> foldout = new List<bool>();
        
        [MenuItem("Insolence Tools/Item Browser")]
        public static void ShowWindow()
        {
            GetWindow<ItemBrowserWindow>("Item Browser");
        }

        private void OnEnable()
        {
            databases = new Dictionary<string, Database>();
            itemTypes = GetItemTypes();
            selectedIndex = 0;
        }

        private void OnGUI()
        {
            selectedIndex = EditorGUILayout.Popup("Type", selectedIndex, itemTypes);
            string itemType = itemTypes[selectedIndex];

            searchString = EditorGUILayout.TextField("Search", searchString);

            if (GUILayout.Button("Create Item"))
            {
                ItemCreationWindow.ShowWindow();
            }
            
            if (GUILayout.Button("Collect Items"))
            {
                itemTypes = GetItemTypes();

                CollectItems(itemType);
                   
            }
            if (!databases.ContainsKey(itemType))
            {

                databases[itemType] = CreateDatabase(itemType);

            }
                Database database = databases[itemType];
                EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

                int i = 0;

                foreach (Item entry in database.values)
                {
                    foldout.Add(false);
                    if (entry.name.ToLower().Contains(searchString.ToLower()) || entry.itemID.ToLower().Contains(searchString.ToLower()))
                        {
                            foldout[i] = EditorGUILayout.Foldout(foldout[i], entry.name);

                            if (foldout[i])
                            {
                                EditorGUILayout.BeginVertical();
                                EditorGUILayout.LabelField(entry.name, EditorStyles.boldLabel);
                                EditorGUILayout.ObjectField(entry, typeof(Item), false);
                                entry.itemID = EditorGUILayout.TextField("ID", entry.itemID);
                                EditorGUILayout.EndVertical();
                        }
                            EditorGUILayout.EndFoldoutHeaderGroup();
                    }
                    else
                    {
                    foldout[i] = EditorGUILayout.Foldout(foldout[i], entry.name);

                    if (foldout[i])
                    {
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField(entry.name, EditorStyles.boldLabel);
                        EditorGUILayout.ObjectField(entry, typeof(Item), false);
                        entry.itemID = EditorGUILayout.TextField("ID", entry.itemID);
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndFoldoutHeaderGroup();
                }
                    i++;
                }
            EditorGUILayout.EndScrollView();
        }

        private string[] GetItemTypes()
        {
            HashSet<string> types = new HashSet<string>();
            string[] guids = AssetDatabase.FindAssets("t:Item");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);
                types.Add(item.type.ToString());
            }
            types.Add("All Items");
            
            
            return new List<string>(types).ToArray();
        }

        private Database CreateDatabase(string itemType)
        {
            string path = "Assets/_ProjectInsolence/Databases/";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder("_ProjectInsolence", "Databases");
            }
            
            if (itemType == "All Items")
            {
                AllItemsDB allItemsDB = ScriptableObject.CreateInstance<AllItemsDB>();
                allItemsDB.itemType = itemType;
                (allItemsDB.values, allItemsDB.keys) = CollectAllItems(itemType);
                AssetDatabase.CreateAsset(allItemsDB, "Assets/_ProjectInsolence/Databases/AllItemsDatabase.asset");
                EditorUtility.SetDirty(allItemsDB);
                
                AssetDatabase.SaveAssets();
                return allItemsDB;
            }
            else
            {
                path += itemType + "Database.asset";
                Database database = ScriptableObject.CreateInstance<Database>();
                database.itemType = itemType;
                (database.values, database.keys) = CollectItems(itemType);
                AssetDatabase.CreateAsset(database, path);
                EditorUtility.SetDirty(database);
                AssetDatabase.SaveAssets();
                return database;
            }
        }

        #region itemCollection


        public (List<Item>, List<string>) CollectItems(string itemType)
        {
            List<string> items = new List<string>();
            List<Item> contents = new List<Item>();

            items.Clear();
            contents.Clear();
            string[] guids = AssetDatabase.FindAssets("t:Item");

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);

                if (item.type.ToString() == itemType)
                {
                    items.Add(item.itemID);
                    contents.Add(item);
                }
            }
            return (contents, items);
        }
        
        public (List<Item>, List<string>) CollectAllItems(string itemType)
        {
            List<string> items = new List<string>();
            List<Item> contents = new List<Item>();
            
            items.Clear();
            contents.Clear();
            string[] guids = AssetDatabase.FindAssets("t:Item");

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);

                items.Add(item.itemID);
                contents.Add(item);
            }
            return (contents, items);
        }
        
        #endregion
        public Item CreateItem()
        {
            string path = "Assets/_ProjectInsolence/Scripts/InventorySystem/Items/Items/ScriptableObjects/";

            path += "New Item.asset";
            Item item = ScriptableObject.CreateInstance<Item>();
            AssetDatabase.CreateAsset(item, path);
            AssetDatabase.SaveAssets();
            return item;
        }
    }
}
