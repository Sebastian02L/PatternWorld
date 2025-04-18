using UnityEngine;

namespace ObserverMinigame 
{
    public class WaypointsManager : MonoBehaviour
    {
        [SerializeField] Transform[] waypoints;
        int direction = 1;
        int index = 0;

        int lastIndex;

        public Transform GetNextWaypoint()
        {
            return waypoints[index];
        }

        public void SetWaypoints(Transform[] waypointsList)
        {
            waypoints = waypointsList;
        }

        public void CalculateNextIndex()
        {
            index += direction;
            if (direction == 1 && index >= waypoints.Length)
            {
                index = 0;
            }
            else if (direction == -1 && index < 0)
            {
                index = waypoints.Length - 1;
            }
        }

        public void CalculateRandomIndex()
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, waypoints.Length);
            } 
            while (randomIndex == lastIndex);

            lastIndex = index;
            index = randomIndex;
        }

        public void ChangeWise()
        {
            direction *= -1;
            CalculateNextIndex();
        }
    }
}
