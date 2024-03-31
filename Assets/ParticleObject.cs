using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleObject : MonoBehaviour
{
    ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        
        particle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the main module of the Particle System
        var mainModule = particle.main;

        // Get the duration of the particle system in seconds
        float duration = mainModule.duration;
        if (particle.totalTime >= duration) Destroy(gameObject);
    }
}
