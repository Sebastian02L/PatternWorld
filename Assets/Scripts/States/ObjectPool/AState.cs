using System;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

namespace ObjectPoolMinigame 
{
    public abstract class AState : IState
    {
        protected IContext context;
        protected GameObject agentGameObject;
        protected GameObject agentHead;
        protected GameObject player;
        protected GameObject playerHead;
        protected EnemyData agentData;
        protected EnemyGunManager gunManager;
        protected Animator animator;

        public AState(IContext context, GameObject player, GameObject agent, EnemyData agentData, Animator animator, GameObject playerHead, GameObject agentHead, EnemyGunManager gunManager)
        {
            this.context = context;
            this.agentGameObject = agent;
            this.player = player;
            this.agentData = agentData;
            this.animator = animator;
            this.playerHead = playerHead;
            this.agentHead = agentHead;
            this.gunManager = gunManager;
        }

        public abstract void Enter();

        public abstract void Exit();

        public abstract void FixedUpdate();

        public abstract void Update();

        protected bool CheckPlayerInFOV()
        {
            Vector3 playerDirection = playerHead.transform.position - agentHead.transform.position;

            if (Mathf.Abs(Vector3.Angle(agentHead.transform.forward, playerDirection)) <= (agentData.FOV / 2f) && playerDirection.magnitude <= agentData.visionDistance)
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
            if (Physics.Raycast(agentHead.transform.position, playerDirection, out hit))
            {
                Debug.DrawRay(agentHead.transform.position, playerDirection * hit.distance, Color.red);

                if (hit.collider.gameObject.tag == "Player") // Verifica si el rayo golpea al jugador
                {
                    return true;
                }
                else
                {
                    Debug.Log("Choque contra" + hit.collider.gameObject.name);
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

