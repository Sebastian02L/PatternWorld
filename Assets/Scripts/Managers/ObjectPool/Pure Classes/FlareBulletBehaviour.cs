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
            AudioClip explosionClip = AudioManager.Instance.GetAudioClip("OPM_FlareRifleExplosion");
            AudioSource.PlayClipAtPoint(explosionClip, bulletGO.transform.position);
            GameObject vfx = GameObject.Instantiate(Resources.Load<GameObject>(SceneManager.GetActiveScene().name + "/" + "ExplosionFVX"), bulletGO.transform);
            vfx.transform.parent = null;

            activeEnemies.Clear();
            activeEnemies = GameObject.FindObjectsByType<EnemyBrain>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList<EnemyBrain>();
            //Debug.Log("Se han encontrado " + activeEnemies.Count + " enemigos");

            float enemyDistance = float.MaxValue;

            foreach (EnemyBrain enemy in activeEnemies)
            {
                float enemyBulletDistance = (enemy.transform.position - bulletGO.transform.position).magnitude;
                //Debug.Log("Enemigo a esta distancia de la bala: " +  enemyBulletDistance);

                if(enemyBulletDistance <= explosionRadius)
                {
                    enemy.GetComponent<HealthManager>().GetDamage(damage);
                }
            }
            callback();
        }
    }
}
