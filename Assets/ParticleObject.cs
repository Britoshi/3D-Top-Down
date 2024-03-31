using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class ParticleObject : MonoBehaviour
{
    [SerializeField]
    float lifeTime;
    ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        Destroy(gameObject, lifeTime);
        particle.Play();
    }

    // Update is called once per frame
    void Update()
    { 

    }
}
