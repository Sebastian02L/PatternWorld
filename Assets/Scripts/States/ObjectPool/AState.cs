using System;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

namespace ObjectPoolMinigame 
{
    public abstract class AState : IState
    {
        protected IContext context;
        protected GameObject agentEyes;
        protected GameObject playerHead;
        protected EnemyData agentData;
        protected Animator animator;

        public AState(IContext context, EnemyData agentData, Animator animator, GameObject playerHead, GameObject agentEyes)
        {
            this.context = context;
            this.agentData = agentData;
            this.animator = animator;
            this.playerHead = playerHead;
            this.agentEyes = agentEyes;
        }

        public abstract void Enter();

        public abstract void Exit();

        public abstract void FixedUpdate();

        public abstract void Update();

        protected bool CheckPlayerInFOV()
        {
            Vector3 playerDirection = playerHead.transform.position - agentEyes.transform.position;

            if (Mathf.Abs(Vector3.Angle(agentEyes.transform.forward, playerDirection)) <= (agentData.FOV / 2f) && playerDirection.magnitude <= agentData.visionDistance)
            {
                return CheckPlayerInSight(playerDirection);
            }
            else
            {
                return false;
            }
        }

        bool CheckPlayerInSight(Vector3 playerDirection)
        {
            RaycastHit hit;
            if (Physics.Raycast(agentEyes.transform.position, playerDirection, out hit))
            {
                Debug.DrawRay(agentEyes.transform.position, playerDirection * hit.distance, Color.red);

                if (hit.collider.gameObject.tag == "Player") // Verifica si el rayo golpea al jugador
                {
                    return true;
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
    }
}

