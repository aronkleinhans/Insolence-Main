using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI; 
using UnityEngine;
using Insolence.KinematicCharacterController;
using Insolence.Core;

namespace Insolence.AIBrain.KCC
{
    public class NpcKinematicController : MonoBehaviour
    {
        public KineCharacterController character;
        
        private Vector3 target;
        private NavMeshAgent agent;
        private NPCAIController npc;
        

        // Use this for initialization
        void OnEnable()
        {
            agent = GetComponent<NavMeshAgent>();
            character = GetComponent<KineCharacterController>();
            npc = GetComponent<NPCAIController>();
        }

        void Update()
        {          
            ApplyInputs(agent.velocity);

            //update the max speed of the character based on distance to target
            if (Vector3.Distance(transform.position, target) > 20f && GetComponent<CharacterStatus>().currentStamina > 0)
            {
                character.MaxStableMoveSpeed = 5f;
                character.isRunning = true;
            }
            else
            {
                character.MaxStableMoveSpeed = 2f;
                character.isRunning = false;
            }
        }
        
        private void ApplyInputs(Vector3 target)
        {
            AICharacterInputs inputs = new AICharacterInputs();

            //set the KKC inputs from navmesh agent velocity
            
            inputs.MoveVector = target;
            
            inputs.LookVector = target;

            character.SetInputs(ref inputs);
            
        }
        public void MoveTo(Vector3 target)
        {
            if (agent.destination != target)
            {
                agent.SetDestination(target);
            }

            this.target = target;

        }

        public void Stop()
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        public Vector3 GetDestination()
        {
            return agent.destination;
        }

        public bool AtDestination()
        {
            if (npc.destination != null && Vector3.Distance(transform.position, npc.destination.transform.position) < 1.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}