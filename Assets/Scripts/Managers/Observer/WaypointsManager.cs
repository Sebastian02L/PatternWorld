using UnityEngine;

namespace ObserverMinigame 
{
    public class WaypointsManager : MonoBehaviour
    {
        [SerializeField] Transform[] waypoints;
        int direction = 1;
        int index = 0;

        public Transform GetNextWaypoint()
        {
            return waypoints[index];
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
            index = Random.Range(0, waypoints.Length);
        }

        public void ChangeDirection()
        {
            direction *= -1;
        }
    }
}
