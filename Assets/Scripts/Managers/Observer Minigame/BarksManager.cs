using System.Collections.Generic;
using NUnit.Framework;
using ObserverMinigame;
using UnityEngine;
using UnityEngine.UI;

namespace ObserverMinigame
{
    public class BarksManager : MonoBehaviour
    {
        [SerializeField] List<Sprite> barks;
        [SerializeField] Image barkImage;

        AudioSource audioSourceBark;

        private void Start()
        {
            audioSourceBark = GetComponentsInParent<AudioSource>()[1];
            FloorDroneBrain.OnPlayerInSight += Bark;
            FlyingDroneBrain.OnPlayerInSight += Bark;
            TurretBrain.OnPlayerInSight += Bark;
            SentinelBrain.OnPlayerInSight += Bark;
        }

        private void OnDestroy()
        {
            FloorDroneBrain.OnPlayerInSight -= Bark;
            FlyingDroneBrain.OnPlayerInSight -= Bark;
            TurretBrain.OnPlayerInSight -= Bark;
            SentinelBrain.OnPlayerInSight -= Bark;
        }

        void Bark(int barkState)
        {
            switch (barkState)
            {
                case 0:
                    barkImage.gameObject.SetActive(false);
                    break;

                case 1:
                    barkImage.gameObject.SetActive(true);
                    barkImage.sprite = barks[0];
                    AudioManager.Instance.PlaySoundEffect(audioSourceBark, "OM_Warning", 0.5f, true);
                    break;

                case 2:
                    barkImage.gameObject.SetActive(true);
                    barkImage.sprite = barks[1];
                    AudioManager.Instance.StopAudioSource(audioSourceBark);
                    AudioManager.Instance.PlaySoundEffect(audioSourceBark, "OM_PlayerTrapped", 1f, true);
                    break;
                default:
                    Debug.Log($"Bark state {barkState} not found");
                    break;
            }
        }
    }
}
