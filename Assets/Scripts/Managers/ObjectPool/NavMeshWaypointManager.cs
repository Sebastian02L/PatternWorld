using Unity.VisualScripting;
using UnityEngine;

public class NavMeshWaypointManager : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;

    public Transform[] GetWaypoints()
    {
        return waypoints;
    }
}
