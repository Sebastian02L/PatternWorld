using UnityEngine;
using UnityEngine.UIElements;

namespace ObserverMinigame 
{
    public abstract class AState : IState
    {
        protected IContext context;
        protected GameObject agentGameObject;
        protected GameObject player;
        protected EnemyData agentData;

        public AState(IContext context, GameObject player, GameObject agent, EnemyData agentData)
        {
            this.context = context;
            this.agentGameObject = agent;
            this.player = player;
            this.agentData = agentData;
        }

        public abstract void Enter();

        public abstract void Exit();

        public abstract void FixedUpdate();

        public abstract void Update();

        protected bool CheckPlayerInFOV(float FOV, float visionDistance)
        {
            Vector3 dir3d = player.transform.position - agentGameObject.transform.position;
            Vector2 dir2d = new Vector2(dir3d.x, dir3d.z);
            Vector2 agentForward2d = new Vector2(agentGameObject.transform.forward.x, agentGameObject.transform.forward.z);

            if (Mathf.Abs(Vector2.Angle(agentForward2d, dir2d)) <= (FOV / 2f))
            {
                return CheckPlayerInSight(dir3d, visionDistance);
            }
            else
            {
                return false;
            }
        }

        bool CheckPlayerInSight(Vector3 dir3d, float visionDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(agentGameObject.transform.position, dir3d, out hit))
            {
                if (hit.collider.gameObject.tag == "Player") // Verifica si el rayo golpea al jugador
                {
                    return CheckPlayerDestiny(dir3d, visionDistance);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        bool CheckPlayerDestiny(Vector3 dir3d, float visionDistance)
        {
            if (dir3d.magnitude <= (visionDistance * 2 / 3))
            {
                Debug.Log("Player is dead");
                return true;
            }
            else if (dir3d.magnitude <= visionDistance)
            {
                Debug.Log("Player is in warning zone");
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}

