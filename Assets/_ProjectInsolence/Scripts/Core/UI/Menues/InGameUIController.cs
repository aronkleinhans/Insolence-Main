using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insolence.KinematicCharacterController;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using Insolence.Core;

namespace Insolence.UI
{
    public class InGameUIController : MonoBehaviour
    {
        [SerializeField] Canvas popUpCanvas_current;
        [SerializeField] Canvas popUpCanvas_next;
        [SerializeField] Canvas inGameMenuCanvas;
        [SerializeField] InputReader _inputReader;
        [SerializeField] GameStateSO _gameState;

        private bool _menuOpen = false;

        private void OnEnable()
        {
            _inputReader.MenuPauseEvent += OnPause;
            _inputReader.MenuUnpauseEvent += OnUnpause;
        }

        private void OnDisable()
        {
            _inputReader.MenuPauseEvent -= OnPause;
            _inputReader.MenuUnpauseEvent -= OnUnpause;
        }
        private void Start()
        {
            inGameMenuCanvas.enabled = false;
        }

        private void Update()
        {
            if (_menuOpen)
            {
                _gameState.UpdateGameState(GameState.Pause);
            }
            else
            {
                _gameState.UpdateGameState(GameState.Gameplay);
                
            }
        }

        //handle interactable pop up message
        public void InteractPopUp(Interactable targetInteractable)
        {
            popUpCanvas_current.enabled = true;
            popUpCanvas_current.GetComponentInChildren<TextMeshProUGUI>().text = targetInteractable.interactionType;
            popUpCanvas_current.GetComponentInChildren<TextMeshProUGUI>().text += " " + targetInteractable.interactableName;
        }
        //handle next interactable pop up message
        public void InteractNextPopUp(Interactable targetInteractable)
        {
            popUpCanvas_next.enabled = true;
            popUpCanvas_next.GetComponentInChildren<TextMeshProUGUI>().text = targetInteractable.interactionType;
            popUpCanvas_next.GetComponentInChildren<TextMeshProUGUI>().text += " " + targetInteractable.interactableName;
        }
        public void closeInteractPopUp()
        {
            popUpCanvas_current.enabled = false;
        }
        public void closeInteractNextPopUp()
        {
            popUpCanvas_next.enabled = false;
        }

        private void OnPause()
        {
            inGameMenuCanvas.enabled = true;
            _menuOpen = true;
        }

        private void OnUnpause()
        {
            inGameMenuCanvas.enabled = false;
            _menuOpen = false;
        }
    }
}