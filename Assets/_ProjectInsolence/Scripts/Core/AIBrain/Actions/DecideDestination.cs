using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace Insolence.AIBrain.Actions
{
    [CreateAssetMenu(fileName = "DecideDestination", menuName = "Insolence/AIBrain/Actions/DecideDestination", order = 1)]
    public class DecideDestination : Action
    {
        public override void Execute(NPCAIController npc)
        {
            Debug.Log("started DecideDestination");
            List<NPCPointOfInterest> poiList = new List<NPCPointOfInterest>();

            poiList.Clear();
            //do the same as the above linq query but use the static allPois list instead(filter with where)
            foreach (NPCPointOfInterest poi in NPCAIController._allPois)
            {
                if (poi != null && poi.HasNeededInterest(npc.interest) && (poi.owner == null || poi.isPublic))
                {
                    if (npc.interest.interestType == InterestType.Work)
                    {
                        if (poi.HasNeededWorkType(npc.interest))
                        {
                            poiList.Add(poi);
                        }
                    }
                    else
                    {
                        poiList.Add(poi);
                    }
                }
            }

            Interest closestPOI = null;
            float closestDistance = Mathf.Infinity;

            //first check if NPC has any ownedpois and if they have the required interest

            if (npc.ownedPois.Count > 0)
            {
                foreach (var poi in npc.ownedPois)
                {
                    if (poi != null)
                    {
                        foreach (Interest i in poi.interests)
                        {
                            if (i != null && i.interestType == npc.interest.interestType)
                            {
                                    float dist = NPCAIController.GetDistance(npc, i);
                                    if (dist < closestDistance)
                                    {
                                        closestDistance = dist;
                                        closestPOI = i;
                                    }
                            }
                        }
                    }
                }
            }
            //else look for closest other interest
            if (closestPOI == null && poiList.Count > 0)
            {
                foreach (var poi in poiList)
                {
                    if (poi != null)
                    {
                        foreach (Interest i in poi.interests)
                        {
                            if (i != null && i.interestType == npc.interest.interestType)
                            {
                                    float dist = NPCAIController.GetDistance(npc, i);
                                    if (dist < closestDistance)
                                    {
                                        closestDistance = dist;
                                        closestPOI = i;
                                    }

                            }
                        }
                    }
                }
            }
            else
            {
                //if no poi is found, set interest to wander
                Debug.Log("No poi found");
            }
            if (closestPOI == null)
            {

            }
            else
            {
                if (npc.neededFood == 0)
                {
                    npc.neededFood = closestDistance * npc.hungerRate * 2;
                }
                else if (npc.neededFood < npc.hungerGainOnArrival)
                {
                    npc.neededFood = npc.hungerGainOnArrival;
                }
                npc.jDestination = closestPOI;
                npc.hasArrived = false;
                npc.travelDistance = NPCAIController.GetDistance(npc, closestPOI);
            }

            npc.OnFinishedAction();
        }       
    }
}
