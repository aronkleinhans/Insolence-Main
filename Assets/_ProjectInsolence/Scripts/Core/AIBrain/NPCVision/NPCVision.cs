using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insolence.Core
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;

    public class NPCVision : MonoBehaviour
    {
        public float heightOffset = 0.3f;
        public float fovAngle = 90f;
        public float sightDistance = 10f;
        public float hearingRadius = 5f;
        public bool drawGizmos = true;
        [SerializeField] HeadTracking headTracking;
        [SerializeField] LayerMask targetMask;
        [SerializeField] LayerMask obstructionMask;

        [SerializeField] public List<GameObject> visibleTargets = new List<GameObject>();
        [SerializeField] public List<GameObject> audibleTargets = new List<GameObject>();

        Vector3 direction = Vector3.zero;

        void Update()
        {
            StartCoroutine(DetectCoroutine());

            // Set the target of the head tracking script to the first visible target if the list is not empty
            if (headTracking != null && visibleTargets.Count > 0)
            {
                headTracking.Target = visibleTargets[0].transform;
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

            Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up * heightOffset, sightDistance, targetMask);
            foreach (Collider collider in colliders)
            {
                Vector3 direction = collider.transform.position - (transform.position + Vector3.up * heightOffset);
                direction.y = 0f;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < fovAngle * 0.5f)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + Vector3.up * heightOffset, direction.normalized, out hit, sightDistance, targetMask))
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

            Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up * heightOffset, hearingRadius, targetMask);
            foreach (Collider collider in colliders)
            {
                if (!audibleTargets.Contains(collider.gameObject) && collider.gameObject != gameObject)
                {
                    audibleTargets.Add(collider.gameObject);
                }
            }
        }

#if UNITY_EDITOR
        
        void OnDrawGizmosSelected()
        {
            if (drawGizmos)
            {
                // Draw the field of view cone
                Gizmos.color = Color.yellow;
                Vector3 fovLine1 = Quaternion.AngleAxis(fovAngle * 0.5f, transform.up) * transform.forward * sightDistance;
                Vector3 fovLine2 = Quaternion.AngleAxis(-fovAngle * 0.5f, transform.up) * transform.forward * sightDistance;
                Gizmos.DrawLine(transform.position + Vector3.up * heightOffset, transform.position + fovLine1);
                Gizmos.DrawLine(transform.position + Vector3.up * heightOffset, transform.position + fovLine2);
                DrawWireCircle(transform.position + Vector3.up * heightOffset, sightDistance, 16);

                // Draw the hearing radius
                Gizmos.color = Color.red;
                DrawWireCircle(transform.position + Vector3.up * heightOffset, hearingRadius, 16);

                // Draw the raycast direction
                Gizmos.color = Color.green;
                if (visibleTargets.Count > 0)
                {
                    //remove null items from list
                    visibleTargets.RemoveAll(item => item == null);
                    //draw line for each target
                    Vector3[] toTarget = new Vector3[visibleTargets.Count];
                    int i = 0;
                    foreach (GameObject target in visibleTargets)
                    {
                        if (target != null)
                        {
                            toTarget[i] = target.transform.position - transform.position;
                            Gizmos.DrawRay(transform.position, toTarget[i]);
                        }
                        i++;
                    }
                }

                // Draw the circle for detecting targets
                Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
                DrawWireCircle(transform.position + Vector3.up * heightOffset, sightDistance * 0.8f, 16);
            }
        }

        void DrawWireCircle(Vector3 position, float radius, int segments)
        {
            Vector3[] points = new Vector3[segments + 1];
            Quaternion rot = Quaternion.LookRotation(Vector3.up);
            for (int i = 0; i <= segments; i++)
            {
                float angle = i / (float)segments * 360f;
                points[i] = position + rot * Quaternion.Euler(0f, 0f, angle) * Vector3.right * radius;
            }
            for (int i = 0; i < segments; i++)
            {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }
#endif
    }
}
