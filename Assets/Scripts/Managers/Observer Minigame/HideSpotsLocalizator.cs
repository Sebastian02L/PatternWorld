using ObserverMinigame;
using UnityEngine;

namespace ObserverMinigame
{
    public class HideSpotsLocalizator : MonoBehaviour
    {
        [SerializeField] GameObject hideSpotsContainer;

        HideController[] hideSpots;
        [SerializeField] int numberOfClosestSpots = 3;

        float firstLowerDistance;
        float secondLowerDistance;
        float thirdLowerDistance;

        private void Start()
        {
            hideSpots = hideSpotsContainer.GetComponentsInChildren<HideController>();
        }

        public HideController CalculateRandomHideSpot(Vector3 agentPosition)
        {
            HideController[] closestSpots = GetClosestSpots(agentPosition);
            return closestSpots[Random.Range(0, numberOfClosestSpots)];
        }

        HideController[] GetClosestSpots(Vector3 agentPosition)
        {
            HideController[] closestSpots = new HideController[numberOfClosestSpots];

            firstLowerDistance = float.PositiveInfinity;
            secondLowerDistance = float.PositiveInfinity;
            thirdLowerDistance = float.PositiveInfinity;

            foreach (HideController hideSpot in hideSpots)
            {
                closestSpots = EvaluateHideSpot((hideSpot.gameObject.transform.position - agentPosition).magnitude, closestSpots, hideSpot);
            }

            return closestSpots;
        }

        HideController[] EvaluateHideSpot(float magnitude, HideController[] closestSpots, HideController hideSpot)
        {
            if (magnitude < firstLowerDistance)
            {
                thirdLowerDistance = secondLowerDistance;
                secondLowerDistance = firstLowerDistance;
                firstLowerDistance = magnitude;

                closestSpots[2] = closestSpots[1];
                closestSpots[1] = closestSpots[0];
                closestSpots[0] = hideSpot;
            }
            else if (magnitude < secondLowerDistance)
            {
                thirdLowerDistance = secondLowerDistance;
                secondLowerDistance = magnitude;

                closestSpots[2] = closestSpots[1];
                closestSpots[1] = hideSpot;
            }
            else if(magnitude < thirdLowerDistance)
            {
                thirdLowerDistance = magnitude;
                closestSpots[2] = hideSpot;
            }

            return closestSpots;
        }
    }
}
