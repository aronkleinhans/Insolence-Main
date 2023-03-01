using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Insolence.SaveUtility;
using System.Linq;

namespace Insolence.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("Save Paths")]
        [SerializeField] public string playerPath;
        [SerializeField] public string objectPath;
        [SerializeField] GameStateSO _gameState;
        [SerializeField] public InputReader inputReader;

        [SerializeField] PlayerInfo playerInfo;

        public bool sceneReady = false;

        private void Update()
        {
            if (GetCurrentSceneName() == "MainMenu" || GetCurrentSceneName() == "zzzLoadScreen")
            {
            }
            else if (GetCurrentSceneName() != "zzzLoadScreen" && GetCurrentSceneName() != "MainMenu")
            {
                if (playerPath == "" || objectPath == "")
                {
                    SaveUtils.DoSave(GetCurrentSceneName());
                    Debug.LogError("Save paths are not set! creating new save...");
                    playerPath = SaveUtils.SAVE_PLAYER_PATH;
                    objectPath = SaveUtils.SAVE_OBJECTS_PATH;
                }
            }
            if (GameObject.Find("Directional Light") != null)
            {
                //Destroy(GameObject.Find("Directional Light"));
            }
            //add a switch statement for gamestate to update the current gamestate and enable respective input maps in inputreader
            switch (_gameState.CurrentGameState)
            {
                case GameState.Pause:
                    inputReader.EnableMenuInput();
                    break;
                case GameState.Gameplay:
                    inputReader.EnableGameplayInput();
                    break;
                case GameState.Dialogue:
                    inputReader.EnableDialogueInput();
                    break;
                case GameState.LocationTransition:
                    inputReader.DisableAllInput();
                    break;
                default:
                    break;
            }


        }
        public static string GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
        public void SaveOnPortal()
        {
            SaveUtils.DoSave(GetCurrentSceneName());
        }
        public void LoadOnPortal(string sceneName, string spawn)
        {
            playerInfo = gameObject.GetComponent<PlayerInfo>();
            playerInfo.GetPlayerInfo();

            StartCoroutine(loadSceneAsync(playerPath, objectPath, sceneName, spawn));
        }

        public IEnumerator loadSceneAsync(string pp, string op, string sceneName, string spawn)
        {
            if (sceneName == "")
            {
                sceneName = SaveUtils.GetSavedSceneName(pp);
            }
            SceneManager.LoadScene("zzzLoadScreen");
            yield return new WaitForSeconds(1f);

            int sceneCount = SceneManager.sceneCount;
            bool sceneIsLoaded = false;

            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == sceneName)
                {
                    sceneIsLoaded = true;
                }
            }

            if (!sceneIsLoaded)
            {

                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                asyncLoad.completed += (AsyncOperation ao) =>
                {
                    Debug.Log("Unloading loadscene & setting active scene");
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                    SceneManager.UnloadSceneAsync("zzzLoadScreen");
                };


                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                if (asyncLoad.isDone)
                {
                    _gameState.UpdateGameState(GameState.Gameplay);
                    StartCoroutine(waitForSceneLoad(pp, op, sceneName, spawn));
                }
            }
        }
        private IEnumerator waitForSceneLoad(string pp, string op, string sceneName, string spawn)
        {
            while (!SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                yield return null;
            }
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                if (spawn != "")
                {
                    GameObject player = GameObject.Find(spawn).GetComponentInChildren<SpawnPlayer>().Spawn();
                    Debug.Log("Scene Change, Spawned Player");
                    SaveUtils.DoLoad(pp, op, false, sceneName);

                    playerInfo.UpdateCharacterState(player);
                }
                else
                {
                    //Debug.Log("Loaded from menu");
                    //SaveUtils.DoLoad(pp, op, true, sceneName);
                }
            }
            sceneReady = true;
        }
    }
}