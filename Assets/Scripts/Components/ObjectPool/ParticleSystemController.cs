using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }

    void Update()
    {
        if(particleSystem.IsAlive()) return;
        Destroy(gameObject);
    }
}
