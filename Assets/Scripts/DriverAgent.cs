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
    private float maxReward;
    public Transform sensorOrigin;
    public float minDistance;
    
    private RaycastHit leftSensor;
    private RaycastHit frontSensor;
    private RaycastHit rightSensor;
    private RaycastHit backSensor;
    private float lastZPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        carController = GetComponent<CarController>();
        collectSensorInput();
        maxReward = Math.Abs(leftSensor.distance + rightSensor.distance);
        lastZPosition = transform.position.z;
    }

    public override void AgentReset()
    {
        transform.position = new Vector3( 0, 0, 0);
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
        collectSensorInput();
        float currentDistance = Math.Abs(leftSensor.distance - rightSensor.distance);
        
        //SetReward(maxReward - currentDistance);

        // Fell off platform

        if (transform.position.z > lastZPosition)
        {
            lastZPosition = transform.position.z;
            SetReward(5);
        }
        else
        {
            SetReward(-10);
        }
        
        if (leftSensor.distance <= minDistance || rightSensor.distance <= minDistance || backSensor.distance <= minDistance || frontSensor.distance <= minDistance ) 
        {
            SetReward(-maxReward);
            Done();
        }
    }

    private void collectSensorInput()
    {
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(sensorOrigin.position, transform.TransformDirection(Vector3.left), out leftSensor, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(sensorOrigin.position, transform.TransformDirection(Vector3.left) * leftSensor.distance, Color.yellow);
        }
        
        if (Physics.Raycast(sensorOrigin.position, transform.TransformDirection(Vector3.forward), out frontSensor, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(sensorOrigin.position, transform.TransformDirection(Vector3.forward) * frontSensor.distance, Color.yellow); ;
        }
        
        if (Physics.Raycast(sensorOrigin.position, transform.TransformDirection(Vector3.right), out rightSensor, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(sensorOrigin.position, transform.TransformDirection(Vector3.right) * rightSensor.distance, Color.yellow);
        }
        
        if (Physics.Raycast(sensorOrigin.position, transform.TransformDirection(Vector3.back), out backSensor, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(sensorOrigin.position, transform.TransformDirection(Vector3.back) * backSensor.distance, Color.yellow);
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
