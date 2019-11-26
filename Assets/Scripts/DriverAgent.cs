using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityStandardAssets.Vehicles.Car;

public class DriverAgent : Agent
{
    public Transform target;

    private Rigidbody rb;
    private CarController carController; // the car controller we want to use
    private float initialDistance;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        carController = GetComponent<CarController>();
        initialDistance = Math.Abs(transform.position.x - target.position.x);
    }

    public override void AgentReset()
    {
        if (transform.position.y < 0)
        {
            transform.position = new Vector3( 0, 0, 0);
        }
    }

    public override void CollectObservations()
    {
        AddVectorObs(target.position);
        AddVectorObs(transform.position);
        
        AddVectorObs(rb.velocity.x);
        AddVectorObs(rb.velocity.z);
    }
    
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        
        carController.Move(controlSignal.x, controlSignal.z, controlSignal.z, 0);
        
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
            target.position);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            Done();
        }

        // Fell off platform
        if (transform.position.y < 0)
        {
            float x1 = transform.position.x;
            float y1 = transform.position.z;
            
            float x2 = target.position.x;
            float y2 = target.position.z;
            double distance = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2) * 2);
            SetReward((float) distance);
            Done();
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
