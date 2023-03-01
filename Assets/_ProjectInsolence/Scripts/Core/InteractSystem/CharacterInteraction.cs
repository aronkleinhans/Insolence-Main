using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Insolence.UI;
using Insolence.KinematicCharacterController;
using System.Linq;

namespace Insolence.Core
{
    public class CharacterInteraction : MonoBehaviour
    {
        [Header("Interactables")]
        [SerializeField] List<Interactable> interactablesInRange = new List<Interactable>();
        [SerializeField] InGameUIController uiControl;
        [SerializeField] InputReader _inputReader;
        [SerializeField] HeadTracking headTracking;
        [SerializeField] Interactable currentInteractable;
        [SerializeField] Interactable nextInteractable;
        [SerializeField] int currentInteractableIndex = 0;
        [SerializeField] int nextInteractableIndex = 0;

        private void OnEnable()
        {
            _inputReader.InteractEvent += OnInteraction;
            _inputReader.CycleInteractableEvent += OnCycleInteractables;
        }
        private void OnDisable()
        {
            _inputReader.InteractEvent -= OnInteraction;
            _inputReader.CycleInteractableEvent -= OnCycleInteractables;
        }
        private void Start()
        {
            uiControl = GameObject.Find("InGameUI").GetComponent<InGameUIController>();
            _inputReader = GameObject.Find("GameManager").GetComponent<GameManager>().inputReader;
            headTracking = GetComponentInChildren<HeadTracking>();
        }
        private void Update()
        {
            HandleInteraction();
        }
        private void HandleInteraction()
        {
            if (interactablesInRange.Count == 0)
            {
                headTracking.Target = null;
                currentInteractable = null;
                currentInteractableIndex = 0;
                uiControl.closeInteractPopUp();
                uiControl.closeInteractNextPopUp();
            }
            else if (interactablesInRange[currentInteractableIndex].GetComponent<Interactable>() != null)
            {
                currentInteractable = interactablesInRange[currentInteractableIndex];
                uiControl.InteractPopUp(currentInteractable);
                headTracking.Target = currentInteractable.transform;
                if (interactablesInRange.Count > 1)
                {
                    //update nextInteractableIndex
                    if (interactablesInRange.Count > 1)
                    {

                        if (currentInteractableIndex + 1 >= interactablesInRange.Count)
                        {
                            nextInteractableIndex = 0;
                        }
                        else
                        {
                            nextInteractableIndex = currentInteractableIndex + 1;
                        }
                    }
                    //update nextInteractable
                    nextInteractable = interactablesInRange[nextInteractableIndex];
                    if (nextInteractable.GetComponent<Interactable>() != null)
                    {
                        uiControl.InteractNextPopUp(nextInteractable);
                    }
                    else
                    {
                        uiControl.closeInteractNextPopUp();
                    }
                }
                else
                {
                    uiControl.closeInteractNextPopUp();
                }
            }
            else
            {
                headTracking.Target = null;
                uiControl.closeInteractPopUp();
                uiControl.closeInteractNextPopUp();
            }
        }
        public bool HasInteractable()
        {
            return interactablesInRange.Count > 0 ? true : false;

        }

        private void OnInteraction()
        {
            if (interactablesInRange.Count > 0)
            {
                Debug.Log(name + "is interacting with " + interactablesInRange[currentInteractableIndex].name);
                Interactable currentInteractable = interactablesInRange[currentInteractableIndex];
                currentInteractable.GetComponent<Interactable>().Interaction(transform);
                interactablesInRange.RemoveAt(currentInteractableIndex);
                currentInteractableIndex = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Interactable>() != null && !interactablesInRange.Contains(other.gameObject.GetComponent<Interactable>()) && other.gameObject.GetComponent<InteractBackstab>() == null)
            {
                interactablesInRange.Add(other.gameObject.GetComponent<Interactable>());
            }
            if (other.gameObject.GetComponent<InteractBackstab>() != null && GetComponent<KineCharacterController>().isCrouching)
            {
                interactablesInRange.Add(other.gameObject.GetComponent<Interactable>());
            }
        }

        private void OnTriggerStay(Collider other)
        {
            bool wasCrouching = GetComponent<KineCharacterController>().isCrouching;

            if (other.gameObject.GetComponent<Interactable>() != null)
            {
                if (!interactablesInRange.Contains(other.gameObject.GetComponent<Interactable>()))
                {
                    interactablesInRange.Add(other.gameObject.GetComponent<Interactable>());
                }
            }

            if (other.gameObject.GetComponent<InteractBackstab>() != null)
            {
                if (GetComponent<KineCharacterController>().isCrouching)
                {
                    if (!interactablesInRange.Contains(other.gameObject.GetComponent<Interactable>()))
                    {
                        interactablesInRange.Add(other.gameObject.GetComponent<Interactable>());
                    }
                }
                else
                {
                    interactablesInRange.Remove(other.gameObject.GetComponent<Interactable>());
                }
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<Interactable>() != null && interactablesInRange.Contains(other.gameObject.GetComponent<Interactable>()))
            {
                interactablesInRange.Remove(other.gameObject.GetComponent<Interactable>());
            }
        }

        private void OnCycleInteractables()
        {
            if (interactablesInRange.Count > 1)
            {
                NextInteractable();
            }
        }

        private void NextInteractable()
        {
            if (interactablesInRange.Count > 0)
            {
                currentInteractableIndex += 1;
                if (currentInteractableIndex >= interactablesInRange.Count)
                {
                    currentInteractableIndex = 0;
                }
            }
            
        }

    }
}