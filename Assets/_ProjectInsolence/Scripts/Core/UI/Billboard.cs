using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Insolence.UI
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statsText;
        [SerializeField] private TextMeshProUGUI bestActionText;
        [SerializeField] private TextMeshProUGUI inventoryText;
        private Transform mainCameraTransform;

        private void Update()
        {
            try
            {
                if (mainCameraTransform == null)
                {
                    mainCameraTransform = Camera.main.transform;
                }
            }
            catch
            {
                Debug.Log("Camera not found");
            }

        }
        void LateUpdate()
        {
            try
            {
                transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);
            }
            catch (System.Exception)
            {
                Debug.Log("Billboard: Camera not found");
            }
        }

        public void UpdateStatsText(int energy, int hunger, int money)
        {
            statsText.text = $"Energy: {energy}\nHunger: {hunger}\nMoney: {money}";
        }

        public void UpdateBestActionText(string bestAction)
        {
            bestActionText.text = bestAction;
        }

        public void UpdateInventoryText(string item)
        {
            
            inventoryText.text = $"Holding: {item}";
        }
    }
}
