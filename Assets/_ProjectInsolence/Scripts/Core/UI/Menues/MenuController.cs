using Insolence.Core;
using Insolence.SaveUtility;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Insolence.UI
{
    public class MenuController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] string newGameLevel;
        
        
        public string levelToLoad;
        GameManager GM;
        public bool bypassMainMenu = false;

        [Header("UI elements")]
        public GameObject noSaveDialog;
        [SerializeField] GameObject savedGameButtonPrefab;
        [SerializeField] GameObject loadMenuContent;
        [SerializeField] float sGBP_Y = 300f;


        //the start method here just starts a new game and hides the main menu(comment for menu testing)
        private void Start()
        {
            GM = GetComponent<GameManager>();
            
            if (bypassMainMenu)
            {
                GameObject.Find("MainMenuCanvas").SetActive(false);
                NewGameDialogYes();
            }

        }
        public void NewGameDialogYes()
        {
            StartCoroutine(GM.loadSceneAsync("", "", newGameLevel, ""));
        }
        public void LoadGameDialog()
        {
            //get existing save files
            string[] saves = SaveUtils.GetSaves();
            if (saves.Length == 0)
            {
                GameObject noSave = FindObjectOfType<MenuController>().GetNoSaveDialog();
                noSave.SetActive(true);
            }
            else
            {
                foreach (string s in saves)
                {

                    //skip meta & txt files 
                    if (s.Contains(".meta") || s.Contains(".txt"))
                    {
                        continue;
                    }
                    // create a list of saved games by player name
                    if (s.Contains(".insp"))
                    {
                        int startpos = s.LastIndexOf("/") + 1;
                        int length = s.Length - s.Substring(s.IndexOf(".")).Length - s.Substring(0, startpos).Length;
                        string playerName = s.Substring(startpos, length);
                        GameObject button = Instantiate(savedGameButtonPrefab);
                        RectTransform rectTransform = button.GetComponent<RectTransform>();

                        rectTransform.SetParent(loadMenuContent.transform);
                        rectTransform.anchoredPosition = new Vector2(0, sGBP_Y);
                        sGBP_Y -= 150f;

                        button.GetComponentInChildren<TextMeshProUGUI>().text = playerName;
                        button.GetComponent<Button>().onClick.AddListener(() => { LoadGameDialogYes(s); });
                        // gotta fix clamping later...
                    }
                }
            }
        }
        public void LoadGameDialogYes(string s)
        {

            GameObject.Find("MainMenuCanvas").SetActive(false);
            GM.playerPath = s;
            GM.objectPath = s.Substring(0, s.IndexOf(".")) + ".inso";

            Debug.Log("Loading scene: " + SaveUtils.GetSavedSceneName(GM.playerPath));

            StartCoroutine(GM.loadSceneAsync(GM.playerPath, GM.objectPath, "", ""));


        }

        public void ExitButton()
        {
            Debug.Log("quitting...");
            Application.Quit();
        }

        public GameObject GetNoSaveDialog()
        {
            return noSaveDialog;
        }
    }
}