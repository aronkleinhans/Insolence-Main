using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Insolence.Core;
using Insolence.SaveUtility;

namespace Insolence.UI
{
    public class DebugMenuController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] Button saveButton;
        [SerializeField] Button loadButton;
        [SerializeField] GameObject loadScreen;
        [SerializeField] GameObject debugConsole;
        [SerializeField] bool showDebug = false;

        private void Start()
        {
            saveButton.onClick.AddListener(() =>
            {
                SaveUtils.DoSave(GameManager.GetCurrentSceneName());
            });
        }

        private void Update()
        {
            gameObject.GetComponent<Canvas>().enabled = showDebug;
            debugConsole.GetComponent<Canvas>().enabled = showDebug;

            if (Input.GetKeyDown(KeyCode.F5))
            {
                showDebug = !showDebug;
            }
        }

        public void debugLoad()
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            StartCoroutine(gm.loadSceneAsync(gm.playerPath, gm.objectPath, "", ""));
        }

        public void KillJill()
        {
            GameObject.Find("friendlyNpc - Jill").GetComponent<CharacterStatus>().currentHealth = 0;
        }
    }
}