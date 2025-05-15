using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ObjectPoolMinigame
{
    public class FlareBulletBehaviour : ABulletBehaviour
    {
        List<EnemyBrain> activeEnemies = new List<EnemyBrain>();
        float explosionRadius = 3f;

        public override void OnCollisionBehaviour(Collider other, float damage, Action callback, GameObject bulletGO)
        {
            //Plays the explosion sound effect on the bullet collision position
            AudioClip explosionClip = AudioManager.Instance.GetAudioClip("OPM_FlareRifleExplosion");
            AudioSource.PlayClipAtPoint(explosionClip, bulletGO.transform.position);

            //Instantiates an GameObject with only one ParticleSystem with the explosion visuals
            GameObject vfx = GameObject.Instantiate(Resources.Load<GameObject>(SceneManager.GetActiveScene().name + "/" + "ExplosionFVX"), bulletGO.transform);
            vfx.transform.parent = null;

            //Calculates how many enemies are in the explosion area reduces the health value of each one
            activeEnemies.Clear();
            activeEnemies = GameObject.FindObjectsByType<EnemyBrain>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList<EnemyBrain>();

            float enemyDistance = float.MaxValue;

            foreach (EnemyBrain enemy in activeEnemies)
            {
                float enemyBulletDistance = (enemy.transform.position - bulletGO.transform.position).magnitude;

                if(enemyBulletDistance <= explosionRadius)
                {
                    enemy.GetComponent<HealthManager>().GetDamage(damage);
                }
            }
            callback();
        }
    }
}
