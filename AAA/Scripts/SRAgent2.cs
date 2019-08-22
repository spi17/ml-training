using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class SRAgent2 : Agent
{

    Rigidbody rBody;
    public GameObject area;

    private AAASensor sensor;
    private LaserController lasers;
    private RoomController room;

    private float speed = 0;
    private int measure;
    private int previousMeasure;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        sensor = GetComponentInChildren<AAASensor>();
        room = area.GetComponent<RoomController>();
        lasers = GetComponentInChildren<LaserController>();
    }

    public override void AgentReset()
    {

        // If the Agent fell, zero its momentum
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.position = new Vector3(8.5f, 0.4f, Random.value * 16 - 8);
        this.transform.rotation = new Quaternion(0, -0.7f, 0, 0.7f);

        speed = 1f;
        measure = 0;
        previousMeasure = 0;
        room.ResetArea();
    }

    public override void CollectObservations()
    {
        // Density and Agent position
        //print(measure);
        AddVectorObs(measure);
        AddVectorObs(this.transform.position);
        AddVectorObs(lasers.getMeasures());
        // Agent velocity
        AddVectorObs(transform.InverseTransformDirection(rBody.velocity));
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

        AddReward(-0.015f);
        measure = sensor.TakeMeasure();

        // Reached target
        if (measure > 60)
        {
            speed = 0.8f;
        }
        else
        {
            speed = 1f;
        }

        if (measure > previousMeasure)
        {
            AddReward(0.02f);
        }
        //else
        //{
        //    AddReward(-0.01f);
        //}

        previousMeasure = measure;

        // Reached target
        if (measure > 95)
        {
            //print("SUCCESS");
            AddReward(5f);
            Done();
        }

        MoveAgent(vectorAction);


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("floor"))
        {
            //print("FAILURE");
            AddReward(-10f);
            Done();
        }

    }

    public void MoveAgent(float[] act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = Mathf.FloorToInt(act[0]);
        switch (action)
        {
            case 1:
                AddReward(0.005f);
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                AddReward(-0.005f);
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                AddReward(-0.001f);
                rotateDir = transform.up * 1f;
                break;
            case 4:
                AddReward(-0.001f);
                rotateDir = transform.up * -1f;
                break;
            default:
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        rBody.AddForce(dirToGo * speed * 0.5f, ForceMode.VelocityChange);
    }
}
