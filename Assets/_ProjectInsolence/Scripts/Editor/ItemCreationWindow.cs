using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Insolence.Core
{
    public class ItemCreationWindow : EditorWindow
    {
        private Item newItem;

        private SerializedObject serializedItem;

        [MenuItem("Insolence Tools/Item Creation Window")]
        public static void ShowWindow()
        {
            ItemCreationWindow window = GetWindow<ItemCreationWindow>("Item Creation Window");
            window.OnEnable();

        }

        private void OnEnable()
        {
            newItem = ScriptableObject.CreateInstance<Item>();
        }

        private void OnGUI()
        {
            
            newItem = (Item)EditorGUILayout.ObjectField("New Item", newItem, typeof(Item), false);

            // Create a SerializedObject from the Item object
            serializedItem = new SerializedObject(newItem);


            // Get the fields of the Item class
            FieldInfo[] fields = typeof(Item).GetFields();

            
            // Draw the properties in the inspector
            foreach (FieldInfo field in fields)
            {
                // Find the SerializedProperty for the field
                SerializedProperty serializedProperty = serializedItem.FindProperty(field.Name);

                // Draw the property in the inspector
                EditorGUILayout.PropertyField(serializedProperty, true);
            }

            serializedItem.ApplyModifiedProperties();

            if (GUILayout.Button("Create"))
            {
                string folderPath = "Assets/_ProjectInsolence/Scripts/Core/InventorySystem/Items/Items/ScriptableObjects/" + newItem.type.ToString() + "/";

                Debug.Log(folderPath);

                if (!AssetDatabase.IsValidFolder(folderPath))
                {
                    AssetDatabase.CreateFolder("Assets/_ProjectInsolence/Scripts/Core/InventorySystem/Items/Items/ScriptableObjects", newItem.type.ToString() );
                }
                
                string path = EditorUtility.SaveFilePanel("Save Item", folderPath, newItem.name, "asset");
 
                if (path.Length > 0)
                {
                    path = FileUtil.GetProjectRelativePath(path);
                    AssetDatabase.CreateAsset(newItem, path);
                }
            }
        }
    }

}
