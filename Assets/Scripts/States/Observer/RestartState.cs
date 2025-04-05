using System;
using UnityEngine;

namespace ObserverMinigame
{
    public class RestartState : AState
    {
        float timer = 0f;
        float intensity;
        Light sensor;

        public RestartState(IContext context, EnemyData agentData, GameObject player, GameObject agent, Action<int> notify) : base(context, player, agent, agentData, notify)
        {
            sensor = agentGameObject.GetComponentInChildren<Light>();
            intensity = sensor.intensity;
            sensor.intensity = 0;
        }

        public override void Enter()
        {
            Notify(0);
            agentGameObject.GetComponent<SoundEffectsController>().SpecialStateSound(); ;
            Debug.Log("Restarting");
        }

        public override void Exit()
        {
            agentGameObject.GetComponent<SoundEffectsController>().SpecialStateSound2();
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            if (timer >= agentData.restartDuration)
            {
                sensor.intensity = intensity;
                context.SetState(new RotateState(context, agentData, player, agentGameObject, notify));
            }
        }
    }
}
