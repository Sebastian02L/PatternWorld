using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, 0f, player.transform.position.z);
        }
    }
}
