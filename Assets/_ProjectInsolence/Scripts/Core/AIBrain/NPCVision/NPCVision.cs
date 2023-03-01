using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    public class NPCVision : MonoBehaviour
    {
        public float fovAngle = 90f;
        public float sightDistance = 10f;
        public float hearingRadius = 5f;
        [SerializeField] HeadTracking headTracking;
        [SerializeField] LayerMask targetMask;
        [SerializeField] LayerMask obstructionMask;

        [SerializeField]List<GameObject> visibleTargets = new List<GameObject>();
        [SerializeField]List<GameObject> audibleTargets = new List<GameObject>();

        Vector3 direction = Vector3.zero;
        void Update()
        {
            StartCoroutine(DetectCoroutine());

            // Set the target of the head tracking script to the first visible target if the list is not empty

            if (headTracking != null)
            {
                if (visibleTargets.Count > 0)
                {
                    headTracking.Target = visibleTargets[0].transform;
                }
            }

            // Use visibleTargets and audibleTargets in your utility AI to make decisions
        }

        private IEnumerator DetectCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                DetectAudibleTargets();
                DetectVisibleTargets();
            }
        }
        void DetectVisibleTargets()
        {
            visibleTargets.Clear();

            Collider[] colliders = Physics.OverlapSphere(transform.position, sightDistance);
            foreach (Collider collider in colliders)
            {
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < fovAngle * 0.5f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, direction.normalized, out hit, sightDistance))
                    {
                        if (hit.collider.gameObject == collider.gameObject && collider.gameObject != gameObject && collider.gameObject.tag == "Interactable")
                        {
                            visibleTargets.Add(collider.gameObject);
                        }
                    }
                }
            }
        }

        void DetectAudibleTargets()
        {
            audibleTargets.Clear();

            Collider[] colliders = Physics.OverlapSphere(transform.position, hearingRadius / 2);
            foreach (Collider collider in colliders)
            {
                if (!audibleTargets.Contains(collider.gameObject) && collider.gameObject != gameObject && collider.gameObject.tag == "Interactable")
                {
                    audibleTargets.Add(collider.gameObject);
                }
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            // Draw the field of view cone
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightDistance);
            Vector3 fovLine1 = Quaternion.AngleAxis(fovAngle * 0.5f, transform.up) * transform.forward * sightDistance;
            Vector3 fovLine2 = Quaternion.AngleAxis(-fovAngle * 0.5f, transform.up) * transform.forward * sightDistance;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);

            // Draw the hearing radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, hearingRadius);
            
            // Draw the raycast direction
            Gizmos.color = Color.green;
            if (visibleTargets.Count > 0)
            {
                Vector3 toTarget = visibleTargets[0].transform.position - transform.position;
                Gizmos.DrawRay(transform.position, toTarget);
            }

        }
#endif
    }

}
