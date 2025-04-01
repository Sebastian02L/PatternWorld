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

        private void Start()
        {
            FloorDroneBrain.OnPlayerInSight += Bark;
            FlyingDroneBrain.OnPlayerInSight += Bark;
            TurretBrain.OnPlayerInSight += Bark;
        }

        private void OnDestroy()
        {
            FloorDroneBrain.OnPlayerInSight -= Bark;
            FlyingDroneBrain.OnPlayerInSight -= Bark;
            TurretBrain.OnPlayerInSight -= Bark;
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
                    break;

                case 2:
                    barkImage.gameObject.SetActive(true);
                    barkImage.sprite = barks[1];
                    break;
                default:
                    Debug.Log($"Bark state {barkState} not found");
                    break;
            }
        }
    }
}
