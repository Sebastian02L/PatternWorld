using System;
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
        protected Action<int> notify;
        protected int lastBarkState = 0;

        public AState(IContext context, GameObject player, GameObject agent, EnemyData agentData, Action<int> notify)
        {
            this.context = context;
            this.agentGameObject = agent;
            this.player = player;
            this.agentData = agentData;
            this.notify = notify;
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

            if (Mathf.Abs(Vector2.Angle(agentForward2d, dir2d)) <= (FOV / 2f) && dir3d.magnitude <= visionDistance)
            {
                return CheckPlayerInSight(dir3d, visionDistance);
            }
            else
            {
                Notify(0);
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
                    //Debug.Log("Choque con" + hit.collider.gameObject.name);
                    Notify(0);
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
                Notify(2);
                return true;
            }
            else 
            {
                Debug.Log("Player is in warning zone");
                Notify(1);
                return false;
            }
        }

        //Is the received barkState is different from the last one, there are two possible cases:
        //1. The enemy was looking the player before and now is not looking him anymore
        //2. The enemy was not looking the player before and now is looking him
        protected void Notify(int barkState)
        {
            if(lastBarkState != barkState)
            {
                notify.Invoke(barkState);
                lastBarkState = barkState;
            }
        }
    }
}

