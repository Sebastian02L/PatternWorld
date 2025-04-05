using System;
using ObserverMinigame;
using UnityEngine;

namespace ObserverMinigame
{
    public class TrapPlayerState : AState
    {
        float timer = 0f;

        public TrapPlayerState(IContext context, EnemyData agentData, GameObject player, GameObject agentGO, Action<int> notify) : base(context, player, agentGO, agentData, notify)
        {
        }

        public override void Enter()
        {
            player.GetComponent<PlayerObserverMovement>().PlayerLose();
            Notify(2);
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= agentData.checkHideSpotTimer && !GameManager.playerTrapped)
            {
                GameManager.SetPlayerTrapped();
                GameObject.FindAnyObjectByType<GameManager>().GameOver(true);
            }
        }
    }
}