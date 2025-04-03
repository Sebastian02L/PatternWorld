using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ObserverMinigame
{
    public class ShootPlayerState : AState
    {
        Quaternion playerDirection;
        float rotationSpeed;
        bool alingmnetCompleted = false;
        bool shootCompleted = false;

        float deadTime = 2f;
        float timer = 0f;

        public ShootPlayerState(IContext context, EnemyData agentData, GameObject player, GameObject agentGO, Action<int> notify) : base(context, player, agentGO, agentData, notify)
        {
            rotationSpeed = this.agentData.rotationSpeed;
            Vector3 direction = (player.transform.position - agentGameObject.transform.position);
            direction.y = 0;
            playerDirection = Quaternion.LookRotation(direction);
        }   
        public override void Enter()
        {
            player.GetComponent<PlayerObserverMovement>().PlayerLose();
            GameManager.SetPlayerTrapped();
        }

        public override void Update()
        {
            if (!alingmnetCompleted && !shootCompleted)
            {
                agentGameObject.transform.rotation = Quaternion.RotateTowards(agentGameObject.transform.rotation, playerDirection, rotationSpeed * Time.deltaTime);

                float angle = Quaternion.Angle(agentGameObject.transform.rotation, playerDirection);
                if (angle < 0.1f)
                {
                    alingmnetCompleted = true;
                }
            }
            else if (alingmnetCompleted && !shootCompleted)
            {
                timer += Time.deltaTime;
                
                if(timer >= deadTime) Shoot();
            }
            else if(shootCompleted)
            {
                timer += Time.deltaTime;

                if (timer >= deadTime) GameObject.FindAnyObjectByType<GameManager>().GameOver();
            }
        }

        void Shoot()
        {
            Transform shootOrigin = agentGameObject.transform.Find("ShootOrigin").gameObject.transform;
            GameObject cylinder = GameObject.Instantiate(agentData.grabCylinder);
            GameObject sphere = GameObject.Instantiate(agentData.grabSphere);

            Vector3 cylinderOrigin = (player.transform.position + shootOrigin.position) / 2;
            cylinder.transform.position = cylinderOrigin;

            Vector3 direction = player.transform.position - shootOrigin.position;

            cylinder.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0); ; 
            float height = direction.magnitude;

            cylinder.transform.localScale = new Vector3(0.03f, height/2, 0.03f);

            sphere.transform.position = player.transform.position;
            sphere.transform.localScale = new Vector3(1f, 1f, 1f);

            timer = 0f;
            shootCompleted = true;
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
        }
    }
}
