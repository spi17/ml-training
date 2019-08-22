using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSensor : MonoBehaviour {

    private float distance;
    private const float MAX_LENGTH_MEASURE = 1f;

    // Use this for initialization
    void Start()
    {
        distance = MAX_LENGTH_MEASURE;
    }

    // Update is called once per frame
    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MAX_LENGTH_MEASURE, layerMask))
        {
            distance = hit.distance;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
        else
        {
            distance = MAX_LENGTH_MEASURE;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * MAX_LENGTH_MEASURE, Color.white);
        }
    }

    private float FilterMeasure(float measure)
    {
        return (measure >= MAX_LENGTH_MEASURE) ? MAX_LENGTH_MEASURE : measure;
    }

    public float GetLaserMeasure()
    {
        return FilterMeasure(distance);
    }
}
