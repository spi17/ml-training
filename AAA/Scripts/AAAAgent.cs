using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AAAAgent : Agent
{

    Rigidbody rBody;
    private long previousMeasure = 0l;
    private long measure = 0l;
    private AAASensor sensor;
    private int timer = 0;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        sensor = GetComponent<AAASensor>();
    }

    public override void AgentReset()
    {

        // If the Agent fell, zero its momentum
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.position = new Vector3(8.4f, 0.4f, -6.8f);


        // Move the target to a new spot
        previousMeasure = 0l;
        timer = 0;
    }

    public override void CollectObservations()
    {
        // Target and Agent positions
        AddVectorObs(measure);
        AddVectorObs(this.transform.position);

        // Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //AddReward(-0.01f);
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0]/10;
        controlSignal.z = vectorAction[1]/10;
        rBody.AddForce(controlSignal * 5);
        //print(controlSignal);
        //print(timer);
        if(timer == 10)
        {
            
            measure = sensor.TakeMeasure();

            if (measure > previousMeasure)
            {
                AddReward(0.01f);
            }
            //else
            //{
            //    AddReward(-0.02f);
            //}

            previousMeasure = measure;

            // Reached target
            if (measure > 10)
            {
                SetReward(1.0f);
                Done();
            }
            timer = 0;
        }
        else
        {
            timer++;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("floor"))
        {
            SetReward(-1f);
            Done();
        }

    }
}
