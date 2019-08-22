using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAASensor : MonoBehaviour {

    public ParticleSystem GasParticlesSystem;
    private ParticleSystem.Particle[] particles;
    private int density;
    // Use this for initialization
    void Start()
    {
        particles = new ParticleSystem.Particle[GasParticlesSystem.main.maxParticles];
    }

    public int TakeMeasure()
    {
        density = 0;
        GasParticlesSystem.GetParticles(particles);
        long N = GasParticlesSystem.particleCount;
        for (long i = 0; i < N; i++)
        {
            float distanceToSensor = Vector3.Distance(this.transform.position, particles[i].position);
            // print("bola: " + this.transform.position + " particula: " + i + " - " + particles[i].position + " dist: " + distanceToSensor);
            if (distanceToSensor < 1f)
            {
                //print(particles[i].position);
                density++;
            }
        }
        //print(density + " " + N);
        return density;
    }

}
