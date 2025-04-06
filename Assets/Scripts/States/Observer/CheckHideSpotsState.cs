using System;
using ObserverMinigame;
using UnityEngine;
using UnityEngine.AI;

namespace ObserverMinigame
{
    public class CheckHideSpotsState : AState
    {
        NavMeshAgent agent;
        HideSpotsLocalizator hideSpotsLocalizator;
        HideController selectedHideSpot;
        float timer = 0f;
        int phase = 0;

        public CheckHideSpotsState(IContext context, EnemyData agentData, GameObject player, GameObject agent, Action<int> notify) : base(context, player, agent, agentData, notify)
        {
            this.agent = agentGameObject.GetComponent<NavMeshAgent>();
            hideSpotsLocalizator = agentGameObject.GetComponent<HideSpotsLocalizator>();
        }
        public override void Enter()
        {
            Debug.Log("     Entre CheckState");
            agent.stoppingDistance = agent.radius + 0.5f;
            selectedHideSpot = hideSpotsLocalizator.CalculateRandomHideSpot(agentGameObject.transform.position);
            agent.isStopped = false;
            agent.SetDestination(selectedHideSpot.transform.position);
        }

        public override void Update()
        {
            if (CheckPlayerInFOV(agentData.FOV, agentData.visionDistance))
            {
                agent.isStopped = true;
                agentGameObject.GetComponent<SoundEffectsController>().Movement(true);
                context.SetState(new ShootPlayerState(context, agentData, player, agentGameObject, notify));
            }
            else
            {
                if (HideSpotReached())
                {
                    agent.isStopped = true;
                    timer += Time.deltaTime;

                    if(timer >= agentData.checkHideSpotTimer && phase == 0)
                    {
                        timer = 0f;
                        phase++;

                        selectedHideSpot.OpenAnimation();
                        if (selectedHideSpot.IsPlayerHidden)
                        {
                            context.SetState(new TrapPlayerState(context, agentData, player, agentGameObject, notify));
                        }
                    }
                    else if (timer >= agentData.checkHideSpotTimer && phase == 1)
                    {
                        context.SetState(new MoveState(context, agentData, player, agentGameObject, notify));
                    }
                }
            }
        }

        bool HideSpotReached()
        {
            return agent.remainingDistance <= agent.stoppingDistance;
        }

        public override void Exit()
        {
            Debug.Log("    Sali CheckState");
            agent.stoppingDistance = 0f;
        }

        public override void FixedUpdate()
        {
        }
    }
}