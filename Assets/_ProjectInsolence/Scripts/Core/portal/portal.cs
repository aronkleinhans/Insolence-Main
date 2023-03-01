using Insolence.KinematicCharacterController;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Insolence.SaveUtility;

namespace Insolence.Core
{

    //fix this script so it works like Teleporter.cs


    public class portal : MonoBehaviour
    {
        [SerializeField] string targetScene;
        [SerializeField] string targetSpawn;
        [SerializeField] public GameObject spawnPoint;
        public portal TeleportTo;

        public UnityAction<KineCharacterController> OnCharacterTeleport;

        public bool isBeingTeleportedTo { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
            
                Debug.Log("Player entered portal: " + other.name);
                KineCharacterController player = other.GetComponent<KineCharacterController>();

                if (player != null)
                {
                    //check if scene loading is required, then load
                    if (SceneManager.GetActiveScene().name != targetScene)
                    {
                        
                        Debug.Log("Teleported (" + other.name + ") to " + targetScene + " at " + targetSpawn + " spawn.");

                        GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();

                        player.gameObject.GetComponent<KinematicCharacterMotor>()
                            .SetPositionAndRotation
                            (
                                GetComponentInChildren<SpawnPlayer>().transform.position, 
                                GetComponentInChildren<SpawnPlayer>().transform.rotation
                            );

                        GM.SaveOnPortal();
                        GM.LoadOnPortal(targetScene, targetSpawn);

                        GameObject.Find("Main Camera").GetComponent<Camera>().enabled = false;
                    }
                    //otherwise move player to target spawn point
                    else
                    {
                        Debug.Log("Teleported (" + other.name + ") to " + targetSpawn + " in the scene.");

                        TeleportPlayer();
                        
                        if (OnCharacterTeleport != null)
                        {
                            OnCharacterTeleport(player);
                        }
                        TeleportTo.isBeingTeleportedTo = true;
                    }
                }
            }
        }
        public void TeleportPlayer()
        {
            KinematicCharacterMotor player = SaveUtils.GetPlayer().GetComponent<KinematicCharacterMotor>();
            player = SaveUtils.GetPlayer().GetComponent<KinematicCharacterMotor>();
            player.SetPositionAndRotation
                (
                    TeleportTo.GetComponentInChildren<SpawnPlayer>().transform.position,
                    TeleportTo.GetComponentInChildren<SpawnPlayer>().transform.rotation
                );
        }
    }
}