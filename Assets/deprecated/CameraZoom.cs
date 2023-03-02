using Insolence.SaveUtility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] Cinemachine.CinemachineFreeLook cam = null;
    [SerializeField]  float[] minCameraDistance = {1f, 4f, 1f};
    [SerializeField] float[] maxCameraDistance = { 10f, 13f, 10f };
    [SerializeField] float[] currentCameraDistance = new float[3];
    [SerializeField] float sensitivity = 10f;

    private void Update()
    {
        //check if cam exists, else log no cam
        if (cam == null && SceneManager.GetActiveScene().name != "MainMenu")
        {
            Debug.Log("current scene: " + SceneManager.GetActiveScene().name);
            Debug.Log("No zoomable camera attached! searching...");
            cam = SaveUtils.GetPlayer().GetComponentInChildren<Cinemachine.CinemachineFreeLook>();

            if(cam != null)
            {
                Debug.Log("Found Camera!");
            }
        }
        else if(cam == null && SceneManager.GetActiveScene().name == "MainMenu")
        {
            Debug.Log("getting temp FreeLook Cam on main menu!");
            cam = GameObject.Find("CM FreeLook1").GetComponent<Cinemachine.CinemachineFreeLook>();
        }
        else { 
            //get current distances
            for (int i = 0; i < 3; i++)
            {
                currentCameraDistance[i] = cam.m_Orbits[i].m_Radius;
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    //change rig radii on scroll
                    cam.m_Orbits[i].m_Radius -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;


                    //reset cam to min/max
                    if (cam.m_Orbits[i].m_Radius < minCameraDistance[i])
                    {
                        cam.m_Orbits[i].m_Radius = minCameraDistance[i];
                    }
                    else if (cam.m_Orbits[i].m_Radius > maxCameraDistance[i])
                    {
                        cam.m_Orbits[i].m_Radius = maxCameraDistance[i];
                    }
                }
            }
        }
    }
}
