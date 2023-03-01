using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Insolence.KinematicCharacterController;
using Insolence.SaveUtility;

namespace Insolence.Core
{
    public class SpawnPlayer : MonoBehaviour
    {
        public GameObject player;
        public GameObject spawned;
        public KinematicCharacterMotor kcc;
        public GameObject Spawn()
        {

            Destroy(GameObject.FindGameObjectWithTag("Player"));
            spawned = Instantiate(player);
            
            kcc = spawned.GetComponent<KinematicCharacterMotor>();
            kcc.SetPosition(transform.position);
            kcc.SetRotation(transform.rotation);

            return spawned;
        }
    }
}