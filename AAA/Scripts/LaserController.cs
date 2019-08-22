using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    public LaserSensor[] laserSensors;
    private float[] measures;

    // Use this for initialization
    void Start () {
        measures = new float[laserSensors.Length];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float[] getMeasures()
    {
        for (int i=0; i< laserSensors.Length; i++)
        {
            measures[i] = laserSensors[i].GetLaserMeasure();
        }
        return measures;
    }
}
