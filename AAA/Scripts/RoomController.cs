using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

    private ParticleSystem GasParticlesSystem;
    // Use this for initialization
    void Start () {
        GasParticlesSystem = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	public void ResetArea () {
        GasParticlesSystem.Clear();
		GasParticlesSystem.transform.position = new Vector3(-10f, 1f, Random.value * 16 - 8);
        GasParticlesSystem.Simulate(2);
        GasParticlesSystem.Play();
    }
}
