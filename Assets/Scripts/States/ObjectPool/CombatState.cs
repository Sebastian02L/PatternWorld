using UnityEngine;

namespace ObjectPoolMinigame
{
    public class CombatState : AState
    {
        HealthManager healthManager;
        Vector3 playerDirection;
        float timer = 0f;
        float lostViewTimer = 1.5f;

        public CombatState(IContext context, EnemyData agentData, GameObject player, GameObject agent, Animator animator, GameObject playerHead, GameObject agentHead, EnemyGunManager gunManager)
            : base(context, player, agent, agentData, animator, playerHead, agentHead, gunManager)
        {
        }

        public override void Enter()
        {
            Debug.Log("Entrando al estado de disparar");
            animator.SetTrigger("Shoot");
            AudioManager.Instance.PlaySoundEffect(agentGameObject.GetComponent<AudioSource>(), "OPM_PlayerSpotted", 0.5f);
            playerDirection = playerHead.transform.position - agentHead.transform.position;
            playerDirection.y = 0;
            agentGameObject.transform.forward = playerDirection.normalized;

            healthManager = agentGameObject.GetComponent<HealthManager>();
        }

        public override void Update()
        {
            if (CheckPlayerInFOV()) 
            {
                playerDirection = playerHead.transform.position - agentHead.transform.position;
                playerDirection.y = 0;
                agentGameObject.transform.forward = playerDirection.normalized;
                gunManager.Shoot();
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
                if(timer > lostViewTimer)
                {
                    context.SetState(new WanderState(context, player, agentGameObject, agentData, animator, playerHead, agentHead, gunManager));
                }
            }

            if (healthManager.GetHealth < (healthManager.GetMaxHealth / 3))
            {
                context.SetState(new EscapeState(context, agentData, player, agentGameObject, animator, playerHead, agentHead, gunManager));
            }
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
        }
    }
}
