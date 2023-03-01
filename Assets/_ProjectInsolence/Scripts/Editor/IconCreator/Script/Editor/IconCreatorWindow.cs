using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using Unity.VisualScripting;
using System.Drawing.Text;
using System.Collections.Generic;

public class IconCreatorWindow : EditorWindow
{
    Vector2 scrollPosition;
    //icon settings
    private string savePath = "Assets";
    private string customFileName = "New Icon";
    int customSizeX = 256;
    int customSizeY = 256;
    int customDepth = 16;

    //item prefab and icon creator references
    private GameObject itemContainer;
    private GameObject itemPrefab;
    private GameObject itemInstance;
    private GameObject iconCreator;
    private Image background;
    private Image foreground;
    private Camera itemCamera;
    private Light pointA;
    private Light pointB;

    //editable item settings
    private Vector3 centerOfMassOffset;
    private Vector3 itemPosition;
    private Vector3 itemRotation;
    private Vector3 itemScale;
    private float padding = 0.1f; // The padding to add to the object's scale


    private float _scale = 1;

    //Image settings
    private float _imageScale = 10;
    private float _imageScaleB = 10;
    private bool showPreview = false;
    private RenderTexture renderTexture;
    private Texture2D previewImage;
    private Texture2D texture;


    [MenuItem("Insolence Tools/Icon Creator")]
    public static void ShowWindow()
    {
        GetWindow<IconCreatorWindow>("Icon Creator");
    }
    private void OnEnable()
    {

        if (GameObject.Find("IGS-iconCreator") != null)
        {
            DestroyImmediate(GameObject.Find("IGS-iconCreator"));
        }
            //GameObject Setup
            if (GameObject.Find("IGS-iconCreator") == null)
        {
            iconCreator = new GameObject("IGS-iconCreator");

            // Create the item camera
            GameObject itemCameraObject = new GameObject("ItemCamera");
            itemCamera = itemCameraObject.AddComponent<Camera>();
            itemCamera.orthographic = true;
            itemCamera.orthographicSize = 1f;
            
            itemCamera.transform.SetParent(iconCreator.transform);
            itemCamera.transform.position = new Vector3(0, 0, -500);

            //Item setup
            GameObject itemSetup = new GameObject("ItemSetup");
            itemSetup.transform.SetParent(iconCreator.transform);

            // Create the canvas with a black image as the background
            GameObject canvasObject = new GameObject("Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = itemCamera;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
            canvasObject.transform.SetParent(itemSetup.transform);
            GameObject imageObject = new GameObject("BackgroundImage");
            imageObject.transform.SetParent(canvasObject.transform);
            Image image = imageObject.AddComponent<Image>();
            image.transform.position = new Vector3(0, 0, 500);
            image.color = Color.black;
            background = image;

            //create a foreground image
            GameObject imageObjectB = new GameObject("ForegroundImage");
            imageObjectB.transform.SetParent(canvasObject.transform);
            Image imageB = imageObjectB.AddComponent<Image>();
            imageB.transform.position = new Vector3(0, 0, -400);
            imageB.color = new Color(1, 1, 1, 0);
            foreground = imageB;

            // Create the item container
            GameObject itemContainerObject = new GameObject("ItemContainer");
            itemContainerObject.transform.SetParent(itemSetup.transform);

            // Set the item container as the default
            itemContainer = itemContainerObject;

            //create point lights
            pointA = new GameObject("PointA").AddComponent<Light>();
            pointA.type = LightType.Point;
            pointA.color = Color.red;
            pointA.intensity = 10;
            pointA.transform.position = new Vector3(-1, 0, 1);
            pointA.transform.SetParent(iconCreator.transform);

            pointB = new GameObject("PointB").AddComponent<Light>();
            pointB.type = LightType.Point;
            pointB.color = Color.white;
            pointB.intensity = 10;
            pointB.transform.position = new Vector3(1, 0, 0);
            pointB.transform.SetParent(iconCreator.transform);
        }
        //texture setup
        renderTexture = new RenderTexture(customSizeX, customSizeY, customDepth, RenderTextureFormat.ARGB32);
        texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
    }

    private void OnGUI()
    {
        // Display the item prefab field
        GUILayout.Space(10f);

        if (itemPrefab == null)
        {
            GUILayout.Label("Drag & Drop item prefab:", EditorStyles.boldLabel);
            Rect dropArea = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drop here");
            Event evt = Event.current;

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                    {
                        break;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        itemPrefab = (GameObject)DragAndDrop.objectReferences[0];
                    }

                    break;
            }
            EditorGUILayout.LabelField("Or choose from the list: ", EditorStyles.boldLabel);
            // Display the item prefab field
            itemPrefab = EditorGUILayout.ObjectField("Item Prefab", itemPrefab, typeof(GameObject), true) as GameObject;
        }
        else
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(position.width), GUILayout.Height(position.height - 50));
            
            if (itemContainer == null)
            {
                itemContainer = GameObject.Find("ItemContainer");
            }

            // Display the item prefab field
            itemPrefab = EditorGUILayout.ObjectField("Item Prefab", itemPrefab, typeof(GameObject), true) as GameObject;

            //instantiate itemPrefab in itemContainer if its empty or the contained prefab is different from the one selected
            if (itemPrefab != null && itemContainer.transform.childCount == 0)
            {
                InstantiatePrefab();
            }
            else if (itemPrefab != null && itemContainer.transform.GetChild(0).gameObject.name != itemPrefab.name + "(Clone)")
            {
                DestroyImmediate(itemContainer.transform.GetChild(0).gameObject);
                InstantiatePrefab();
            }
            else if (itemPrefab == null && itemContainer.transform.childCount > 0)
            {
                DestroyImmediate(itemContainer.transform.GetChild(0).gameObject);
            }

            // Display the save path selection and filename fields

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Save Settings", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Save Path", savePath + "/" + customFileName + ".png");
            if (savePath == "Assets")
            {
                if (GUILayout.Button("Select Save Path"))
                {
                    savePath = EditorUtility.SaveFolderPanel("Select Folder to Save Icon", "Assets", "");
                }
            }
            EditorGUILayout.EndHorizontal();

            customFileName = EditorGUILayout.TextField("Save Name", customFileName);

            // Display the size fields
            EditorGUILayout.LabelField("Image Settings", EditorStyles.boldLabel);

            customSizeX = EditorGUILayout.IntField("Height", customSizeX);
            customSizeY = EditorGUILayout.IntField("Width", customSizeY);
            customDepth = EditorGUILayout.IntField("Depth", customDepth);
            
            

            EditorGUILayout.EndVertical();

            //Display background image settings exposing color and an image field in the ui
            
       
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Background Settings", EditorStyles.boldLabel);
            background.color = EditorGUILayout.ColorField("Background Color", background.color);
            
            EditorGUILayout.BeginHorizontal();
            
            background.sprite = EditorGUILayout.ObjectField("Background Image", background.sprite, typeof(Sprite), true) as Sprite;
            
            EditorGUILayout.BeginVertical();

            background.transform.localPosition = EditorGUILayout.Vector3Field("Position", background.transform.localPosition);
            _imageScale = EditorGUILayout.FloatField("Scale", _imageScale);
            background.transform.localScale = new Vector2(_imageScale, _imageScale);
           
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            //Display foreground image settings exposing color and an image field in the ui
            EditorGUILayout.BeginVertical();
            

            EditorGUILayout.LabelField("Foreground Settings", EditorStyles.boldLabel);
            foreground.color = EditorGUILayout.ColorField("Foreground Color", foreground.color);

            EditorGUILayout.BeginHorizontal();

            foreground.sprite = EditorGUILayout.ObjectField("Foreground Image", foreground.sprite, typeof(Sprite), true) as Sprite;

            EditorGUILayout.BeginVertical();

            foreground.transform.localPosition = EditorGUILayout.Vector3Field("Position", foreground.transform.localPosition);
            _imageScaleB = EditorGUILayout.FloatField("Scale", _imageScaleB);
            foreground.transform.localScale = new Vector2(_imageScaleB, _imageScaleB);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            //Display camera settings: camera size, clipping planes, background type and color
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Camera Settings", EditorStyles.boldLabel);
            
            itemCamera.orthographicSize = EditorGUILayout.FloatField("Size", itemCamera.orthographicSize);
            itemCamera.nearClipPlane = EditorGUILayout.FloatField("Near Clipping", itemCamera.nearClipPlane);
            itemCamera.farClipPlane = EditorGUILayout.FloatField("Far Clipping", itemCamera.farClipPlane);

            EditorGUILayout.EndVertical();

            // Display the item settings
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Item Transform", EditorStyles.boldLabel);

            itemPosition = EditorGUILayout.Vector3Field("Position", itemPosition);
            itemRotation = EditorGUILayout.Vector3Field("Rotation", itemRotation);
            _scale = EditorGUILayout.FloatField("Scale", _scale);
            itemScale = new Vector3(_scale, _scale, _scale);
            EditorGUILayout.EndVertical();

            //Display the light setting section
            EditorGUILayout.LabelField("Lighting Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();


            EditorGUILayout.LabelField("Light A");
            pointA.color = EditorGUILayout.ColorField("Color", pointA.color);
            pointA.transform.position = EditorGUILayout.Vector3Field("Position", pointA.transform.position);
            pointA.intensity = EditorGUILayout.FloatField("Intensity", pointA.intensity);

            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Light B");
            pointB.color = EditorGUILayout.ColorField("Color", pointB.color);
            pointB.transform.position = EditorGUILayout.Vector3Field("Position", pointB.transform.position);
            pointB.intensity = EditorGUILayout.FloatField("Intensity", pointB.intensity);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            // Display the preview section
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);

            // Create the preview texture
            if (itemInstance != null)
            {                
                itemInstance.SetActive(true);
                itemInstance.transform.position = itemPosition;
                itemInstance.transform.eulerAngles = itemRotation;
                itemInstance.transform.localScale = itemScale;
                itemCamera.targetTexture = renderTexture;
                showPreview = true;

                // Display the preview image
                if (showPreview && previewImage != null)
                {
                    GUI.DrawTexture(GUILayoutUtility.GetRect(customSizeX, customSizeY), previewImage, ScaleMode.ScaleToFit);
                }
            }

            EditorGUILayout.EndVertical();

            // Display the Create Icon button
            if (GUILayout.Button("Create Icon"))
            {
                // Save the texture to file
                if (itemInstance != null && !string.IsNullOrEmpty(savePath))
                {
                    // construct savepath if it does not contain the fiename
                    //check if directory exists

                    if (!savePath.Contains(customFileName))
                    {
                        // Remove everything before the "Assets" string in the save path
                        int index = savePath.IndexOf("Assets");
                        if (index > 0)
                        {
                            savePath = savePath.Substring(index);
                        }

                        savePath = savePath + "/" + customFileName + ".png";

                    }
                    // Check if the save path exists
                    if (!Directory.Exists(Path.GetDirectoryName(savePath)))
                    {
                        if (EditorUtility.DisplayDialog("Warning!", "The specified directory does not exists. Create?", "OK", "Cancel"))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                            SaveImage();
                            ResetWindow();
                        }
                        else
                        {
                            ResetPath();
                            return;
                        }

                    }
                    else if (File.Exists(savePath))
                    {
                        if (EditorUtility.DisplayDialog("Warning!", "The specified filename already exists. Overwrite?", "OK", "Cancel"))
                        {
                            SaveImage();
                            ResetWindow();
                        }
                        else
                        {
                            ResetPath();
                            return;
                        }
                    }
                    else
                    {
                        SaveImage();
                        ResetWindow();
                    }
                    
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
    private void SaveImage()
    {
        //save the image
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(savePath, bytes);
        AssetDatabase.ImportAsset(savePath);
        TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(savePath);
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.spriteImportMode = SpriteImportMode.Single;
        textureImporter.spritePixelsPerUnit = 100;
        AssetDatabase.ImportAsset(savePath);

        Debug.Log("Icon saved at " + savePath);
    }
    private void ResetWindow()
    {
        ResetPath();
        itemPrefab = null;
        showPreview = false;
    }

    private void ResetPath()
    {
        //reset path
        savePath = savePath.Remove(savePath.IndexOf("/" + customFileName));
        customFileName = "New Icon";
    }


    private void InstantiatePrefab()
    {
        itemInstance = Instantiate(itemPrefab, itemContainer.transform);

        //set itemPosition, itemRotation and itemScale to itemContainer's position, rotation and scale
        itemPosition = itemContainer.transform.position;
        itemRotation = itemContainer.transform.eulerAngles;
        itemScale = itemContainer.transform.localScale;

        // Get the object's mesh renderer
        Renderer[] renderers = itemContainer.transform.GetChild(0).GetComponentsInChildren<Renderer>();

        // set the camera orthographic size to the total height of the item + padding
        itemCamera.orthographicSize = FindTotalHeight(renderers) + padding;

        // Calculate the offset between the transform position and the center of mass
        centerOfMassOffset = itemContainer.transform.position - FindCenterOfMass(renderers);
        // Add the offset to the item position
        itemPosition = itemContainer.transform.position + centerOfMassOffset;
    }
    private Vector3 FindCenterOfMass(Renderer[] renderers)
    {
        if (renderers.Length > 1)
        {
            // Create variables to store the total volume and weighted sum of the centers
            float totalVolume = 0f;
            Vector3 weightedSumOfCenters = Vector3.zero;

            // Loop through all the renderers and calculate their volumes and weighted centers
            foreach (Renderer renderer in renderers)
            {
                // Get the center of the renderer's bounding box
                Vector3 center = renderer.bounds.center;

                // Calculate the volume of the renderer's bounding box
                float volume = renderer.bounds.size.x * renderer.bounds.size.y * renderer.bounds.size.z;

                // Add the volume and weighted center to the total
                totalVolume += volume;
                weightedSumOfCenters += volume * center;
            }

            // Calculate the center of mass by dividing the weighted sum of centers by the total volume
            Vector3 centerOfMass = weightedSumOfCenters / totalVolume;

            return centerOfMass;
        }
        else if (renderers.Length == 1)
        {
            return renderers[0].bounds.center;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }
    private float FindTotalHeight( Renderer[] renderers)
    {
        if (renderers.Length > 1)
        {
            // Create a list to store the minimum and maximum Y positions of all the renderers
            List<float> yPositions = new List<float>();

            // Loop through all the renderers and add their Y positions to the list
            foreach (Renderer renderer in renderers)
            {
                yPositions.Add(renderer.bounds.min.y);
                yPositions.Add(renderer.bounds.max.y);
            }

            // Calculate the minimum and maximum Y positions of all the renderers
            float minY = Mathf.Min(yPositions.ToArray());
            float maxY = Mathf.Max(yPositions.ToArray());

            // Calculate the total height of the prefab
            float totalHeight = maxY - minY;

            return totalHeight;
        }
        else if (renderers.Length == 1)
        {
            return renderers[0].bounds.extents.y;
        }
        else
        {
            return 0;
        }
    }

    private void OnDestroy()
    {
        if (itemInstance != null)
        {
            DestroyImmediate(itemInstance);
            
        }
        DestroyImmediate(iconCreator);
    }

    private void Update()
    {
        if (itemInstance != null)
        {
            itemInstance.transform.position = itemPosition;
            itemInstance.transform.eulerAngles = itemRotation;
            itemInstance.transform.localScale = itemScale;

        }

        if (renderTexture.height != customSizeX || renderTexture.width != customSizeY || renderTexture.depth != customDepth)
        {
            renderTexture = new RenderTexture(customSizeX, customSizeY, customDepth);
            texture = new Texture2D(customSizeX, customSizeY, TextureFormat.RGB24, false);
            itemCamera.targetTexture = renderTexture;
        }

        // Take snapshot
        if (itemInstance != null && itemCamera != null)
        {
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();
            previewImage = texture;
            RenderTexture.active = null;
        }
        
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }
}
