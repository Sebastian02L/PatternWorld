using UnityEngine;

namespace ObserverMinigame
{
    public class SoundEffectsController : MonoBehaviour
    {
        AudioSource audioSourceShoot;
        AudioSource audioSourceMovement;

        public string audioNameMovement;
        public string audioNameSpecial;
        public string audioNameSpecial2;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            AudioSource[] aus = GetComponents<AudioSource>();
            audioSourceMovement = aus[0];
            audioSourceShoot = aus[1];
        }

        public void Movement(bool stopAudioSource)
        {
            if (!stopAudioSource) AudioManager.Instance.PlaySoundEffect(audioSourceMovement, audioNameMovement, 0.6f, false, true);
            else AudioManager.Instance.StopAudioSource(audioSourceMovement);
        }

        public void SpecialStateSound()
        {
            AudioManager.Instance.PlaySoundEffect(audioSourceMovement, audioNameSpecial, 0.2f);
        }

        public void SpecialStateSound2()
        {
            AudioManager.Instance.PlaySoundEffect(audioSourceMovement, audioNameSpecial2, 0.2f);
        }
        public void ShootPlayer()
        {
            AudioManager.Instance.PlaySoundEffect(audioSourceShoot, "OM_Shoot", 0.1f);
        }
    }
}
