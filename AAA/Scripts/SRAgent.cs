using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System.Linq;

public class SRAgent : Agent
{

    Rigidbody rBody;
    public GameObject area;

    private AAASensor sensor;
    private LaserController lasers;
    private RoomController room;

    private int[] previousMeasures = new int[20];
    private int[] currentMeasures = new int[5];
    private double meanPreviousMeasures;
    private double meanCurrentMeasures;
    private int measure = 0;
    private int timer = 0;
    private float speed = 0;

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

        // Move the target to a new spot
        previousMeasures = new int[10];
        currentMeasures = new int[3];
        meanPreviousMeasures = 0;
        timer = 0;
        speed = 1f;

        room.ResetArea();
    }

    public override void CollectObservations()
    {
        // Density and Agent position
        AddVectorObs((float)meanCurrentMeasures);
        AddVectorObs(this.transform.position);
        AddVectorObs(lasers.getMeasures());
        // Agent velocity
        AddVectorObs(transform.InverseTransformDirection(rBody.velocity));
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {


        AddReward(-0.1f);
        measure = sensor.TakeMeasure();

        // Reached target
        if (meanCurrentMeasures > 25)
        {
            speed = 0.5f;
        }
        else
        {
            speed = 1f;
            //AddReward(-0.1f);
        }

        normalizeCurrentMeasure(measure);

        //if (meanCurrentMeasures > meanPreviousMeasures)
        //{
        //    AddReward(0.1f);
        //    //print("00000000000000000000000000000000000000000000000000000");
        //}
        //print(meanPreviousMeasures + " " + meanCurrentMeasures + " " + measure);
        storeMeasure(measure);

        // Reached target
        if (meanCurrentMeasures > 45)
        {
            AddReward(3f);
            Done();
        }



        MoveAgent(vectorAction);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("floor"))
        {
            //print("boom");
            AddReward(-10f);
            Done();
        }

    }

    private void normalizeCurrentMeasure(int measure)
    {
        for (int i = 0; i < currentMeasures.Length - 1; i++)
        {
            currentMeasures[i] = currentMeasures[i + 1];
        }

        currentMeasures[currentMeasures.Length - 1] = measure;

        meanCurrentMeasures = currentMeasures.Average();
    }

    private void storeMeasure(int measure)
    {

        for (int i = 0; i < previousMeasures.Length - 1; i++)
        {
            previousMeasures[i] = previousMeasures[i + 1];
        }

        previousMeasures[previousMeasures.Length - 1] = measure;

        meanPreviousMeasures = previousMeasures.Average();
    }

    public void MoveAgent(float[] act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = Mathf.FloorToInt(act[0]);
        switch (action)
        {
            case 1:
                AddReward(0.05f);
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                AddReward(-0.01f);
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                //AddReward(-0.001f);
                rotateDir = transform.up * 1f;
                break;
            case 4:
                //AddReward(-0.001f);
                rotateDir = transform.up * -1f;
                break;
            default:
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        rBody.AddForce(dirToGo * speed * 1.5f, ForceMode.VelocityChange);
    }
}
